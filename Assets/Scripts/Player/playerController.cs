using System;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    //First 4 variables are to handle movement and angle checking so you can drag away from player and it moves
    private Vector3 mouseInputPos;
    private Vector3 mouseReleasePos;
    private Vector2 dragPath;
    private bool dragging = false;
    //Base stats for movement
    private float ballSpeed = 6f, clashDmg;
    //Bool dragged so same input isn't taken multiple times
    private bool dragged = false;
    private Rigidbody rb;
    //All variables for moving camera, min and max describe bounds for how far the camera can be
    private Camera cam;
    private float cameraPanSpeed = 15f;
    private bool cameraPanning = false;
    private float minCamDistanceZ = 10f;
    private float maxCamDistanceZ = -10f;
    //For use in damage calculations
    private float playerSpeedThisFrame;
    //UI related to the player
    [SerializeField] private HealthBar playerHealthBar;
    [SerializeField] private PointReader pointHolder;
    [SerializeField] private GameObject guidanceCue;
    [SerializeField] private GameObject guidanceCueVisual;
    [SerializeField] private GameObject DeathCanvas;
    [SerializeField] private GameObject InfoCanvas;
    //For the cue visual so the player knows where they are hitting
    [SerializeField] private float CueOffsetX;
    [SerializeField] private float CueOffsetZ;
    public GameObject camera;
    public float damageTakenMultiplier, damageGivenMultiplier, damageBuffer, health = 100f;

    
    //Functionality
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //non physics based functions are run in update
        MovementCheck();
        CameraPan();
    }
    private void FixedUpdate()
    {
        //all physics functions and checks are in fixed update
        playerSpeedThisFrame = rb.linearVelocity.magnitude;
        //Lock player so it cant exit area (such as if it is pussed through wall by enemy
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -10f, 10f), Mathf.Clamp(transform.position.y, -15f, 0.1f), transform.position.z);
        //keep the tip of cue at player position
        guidanceCue.transform.position = transform.position;

        //Triggers once when the player releases left click
        if (dragged)
        {
            ApplyMovement();
        }

        if(health <= 0)
        {
            Die();
        }

        if (dragging)
        {
            if (rb.linearVelocity.z < 1f && !cameraPanning) //cue is visible under same conditions as shooting so that player knows when they can shoot
            {
                guidanceCue.SetActive(true);
            }
            //handle cue visual while player holds left click
            float angleAboutY = Mathf.Atan2((mouseInputPos.x - Input.mousePosition.x), (mouseInputPos.y - Input.mousePosition.y));  //use atan to find angle with the adjacent (x2-x1) and opposite (y2-y1), this returns in radians
            guidanceCue.transform.rotation = Quaternion.Euler(0f, Mathf.Rad2Deg * angleAboutY, 0f);  //turn radians into degrees and set guidance cue to that angle around the y axis
            Vector2 pull = Input.mousePosition - mouseInputPos;
            float pullDistance = Mathf.Clamp(pull.magnitude / 100, 0f, 5f);//uses the same fomula as the actual hit so that when the cue stops moving you know you are at max power
            guidanceCueVisual.transform.localPosition = new Vector3(0,0,-1.1f-(pullDistance/10)); //shift the cue + a base offset of -1.1 (so it is already outside ball)

            //in future when a release occurs, the cue should move to the ball to visually strike it
        }
    }

    private void MovementCheck()
    {
        //if it detects a click and you arent already clicking
        if (Input.GetMouseButtonDown(0) && !dragging)
        {
            dragging = true;
            mouseInputPos = Input.mousePosition; //get mouses position on screen
        }
        //if mouse button is released trigger this once
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            guidanceCue.SetActive(false);
            dragging = false;
            mouseReleasePos = Input.mousePosition;
            dragged = true;
        }
    }
    private void ApplyMovement()
    {
        dragged = false;
        if (rb.linearVelocity.z < 1f && !cameraPanning) //only allow player to move themself if they are slower than a certain speed in the forwards direction and if the camera isnt moving back to them
        {
            dragPath = mouseReleasePos - mouseInputPos;
            Vector3 force = new Vector3(dragPath.x, 0, dragPath.y);
            float hitForce = Mathf.Clamp(dragPath.magnitude / 100, 0f, 5f);
            rb.AddForce(-force.normalized * (ballSpeed * hitForce), ForceMode.Impulse); //negative force so if backwards drag it goes forwards
        }
    }

    private void CameraPan()
    {
        //setting the camera to follow player by using clamp caused awful jittering, so instead check distance from player and hard follow at the edge of range
        if (camera.transform.position.z - transform.position.z > minCamDistanceZ)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z + minCamDistanceZ);
        }
        else if (camera.transform.position.z - transform.position.z < maxCamDistanceZ)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z + maxCamDistanceZ);
        }
        //if the player is slow enough in the z axis, use players position to calculate a new target position for the camera and move it there until it is close enough
        if (rb.linearVelocity.z < 1f)
        {
            Vector3 cameraTargetPos = new Vector3(0, 20f, transform.position.z + 8);
            if (Vector3.Distance(camera.transform.position, cameraTargetPos) > 0.05)
            {
                cameraPanning = true;
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, cameraTargetPos, cameraPanSpeed * Time.deltaTime);
            }
            else
            {
                cam.transform.position = cameraTargetPos;
                cameraPanning = false;
            }
        }
    }

    public void Die()
    {
        //on death set active death canvas
        DeathCanvas.SetActive(true);
        InfoCanvas.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Assignment of Enemy Components from the Enemy we just collided with
            Rigidbody rbEnemy = other.gameObject.GetComponent<Rigidbody>();
            EnemyBehavior enemyData = other.gameObject.GetComponent<EnemyBehavior>();
            HealthBar enemyHealthBar = other.gameObject.GetComponentInChildren<HealthBar>();
            

            clashDmg = playerSpeedThisFrame + enemyData.enemySpeedThisFrame; // The combined speed of the player and the enemy

            //these if statements are to make sure the damage doesn't go negative and heal the enemy / player,
            //buffer stops the moving party from taking damage if hitting the other, if they are almost motionless
            if (clashDmg - playerSpeedThisFrame > damageBuffer && enemyData.moving) 
            {
                health -= (clashDmg - playerSpeedThisFrame * damageTakenMultiplier); // damage  shared with the player, player takes more % of the total damage if the enemy was moving faster than the player
            }
            if(clashDmg - enemyData.enemySpeedThisFrame > damageBuffer)
            {
                enemyData.health -= clashDmg * damageGivenMultiplier - (enemyData.enemySpeedThisFrame); // damage shared with the enemy, works the same as the enemy damage
            }

            //Update the health bars of Player and Enemy
            playerHealthBar.UpdateHealthBar(health, 100);
            enemyHealthBar.UpdateHealthBar(enemyData.health, 100);
        }
    }
}
