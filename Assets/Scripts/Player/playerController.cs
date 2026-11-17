using System;
using Unity.Mathematics;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //Assignments
    private Vector3 mouseInputPos;
    private Vector3 mouseReleasePos;
    private Vector2 dragPath;
    private bool dragging = false;
    private float ballSpeed = 6f, clashDmg;
    private bool dragged = false;
    private Rigidbody rb;
    private Camera cam;
    private float cameraPanSpeed = 15f;
    private bool cameraPanning = false;
    private float minCamDistanceZ = 10f;
    private float maxCamDistanceZ = -10f;
    private float playerSpeedThisFrame;

    //Drag these into their slots in the editor
    [SerializeField] private HealthBar playerHealthBar;
    [SerializeField] private PointReader pointHolder;
    [SerializeField] private GameObject guidanceCue;
    [SerializeField] private GameObject DeathCanvas;
    [SerializeField] private GameObject InfoCanvas;
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
        MovementCheck();
        CameraPan();
    }
    private void FixedUpdate()
    {
        playerSpeedThisFrame = rb.linearVelocity.magnitude;
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -15f, 0.1f), transform.position.z);

        if (dragged)
        {
            ApplyMovement();
            guidanceCue.SetActive(false);
        }

        if(health <= 0)
        {
            Die();
        }

        if (dragging)
        {
            guidanceCue.SetActive(true);
            Vector3 cuePoint = gameObject.transform.position + new Vector3 (cam.ScreenToViewportPoint(Input.mousePosition).x * 40 + CueOffsetX, 0, cam.ScreenToViewportPoint(Input.mousePosition).y * 20 + CueOffsetZ);
            guidanceCue.transform.position = cuePoint;
            //guidanceCue.transform.LookAt(gameObject.transform, Vector3.up);
        }
    }

    private void MovementCheck()
    {
        if (Input.GetMouseButtonDown(0) && !dragging)
        {
            dragging = true;
            mouseInputPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            mouseReleasePos = Input.mousePosition;
            dragged = true;
        }
    }
    private void ApplyMovement()
    {
        dragged = false;
        if (rb.linearVelocity.magnitude < 1f && !cameraPanning)
        {
            dragPath = mouseReleasePos - mouseInputPos;
            Debug.Log(dragPath.magnitude);
            Vector3 force = new Vector3(dragPath.x, 0, dragPath.y);
            float hitForce = Mathf.Clamp(dragPath.magnitude / 100, 0f, 5f);
            rb.AddForce(-force.normalized * (ballSpeed * hitForce), ForceMode.Impulse); //negative force so if backwards drag it goes forwards
        }
    }

    private void CameraPan()
    {
        if (camera.transform.position.z - transform.position.z > minCamDistanceZ)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z + minCamDistanceZ);
        }
        else if (camera.transform.position.z - transform.position.z < maxCamDistanceZ)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z + maxCamDistanceZ);
        }
        if (rb.linearVelocity.magnitude < 1.2f)
        {
            Vector3 cameraTargetPos = new Vector3(0, 20f, transform.position.z + 8);
            if (Vector3.Distance(camera.transform.position, cameraTargetPos) > 0.05)
            {
                cameraPanning = true;
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, cameraTargetPos, cameraPanSpeed * Time.deltaTime);
            }
            else
            {
                cameraPanning = false;
            }
        }
    }

    public void Die()
    {
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
            //Because the data is gathered after the collision, thanks to Newton the speed of the enemy and player are essentially swapped

            //these if statements are to make sure the damage doesn't go negative and heal the enemy / player,
            //buffer stops the moving party from taking damage if hitting the other, if they are almost motionless
            if (clashDmg - playerSpeedThisFrame > damageBuffer && enemyData.moving) 
            {
                health -= (clashDmg - playerSpeedThisFrame * damageTakenMultiplier); // damage is shared with the enemy, enemy takes more % of the total damage if the player was moving faster than the enemy
            }
            if(clashDmg - enemyData.enemySpeedThisFrame > damageBuffer)
            {
                enemyData.health -= clashDmg * damageGivenMultiplier - (enemyData.enemySpeedThisFrame); // damage shared with the player, works the same as the enemy damage
            }

            //Update the health bars of Player and Enemy
            playerHealthBar.UpdateHealthBar(health, 100);
            enemyHealthBar.UpdateHealthBar(enemyData.health, 100);
        }
    }
}
