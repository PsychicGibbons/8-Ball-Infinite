using System.Net.Http.Headers;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Vector3 mouseInputPos;
    private Vector3 mouseReleasePos;
    private Vector2 dragPath;
    private bool dragging = false;
    private float health = 100f, ballSpeed = 30f; // in future make it so ballspeed is dependent on dragpath.magnitude
    private bool dragged = false;

    private Rigidbody rb;

    public GameObject camera;
    [SerializeField] private float damageTakenMultiplier, damageGivenMultiplier;
    private float cameraPanSpeed = 15f;
    private bool cameraPanning = false;
    private float minCamDistanceZ = 10f;
    private float maxCamDistanceZ = -10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementCheck();
        CameraPan();
    }
    private void FixedUpdate()
    {
        if (dragged)
        {
            ApplyMovement();
        }

        if(health <= 0)
        {
            Die();
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
            Vector3 force = new Vector3(dragPath.x, 0, dragPath.y);
            rb.AddForce(-force.normalized * ballSpeed, ForceMode.Impulse); //negative force so if backwards drag it goes forwards
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

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody rbEnemy = other.gameObject.GetComponent<Rigidbody>();
            EnemyBehavior enemyData = other.gameObject.GetComponent<EnemyBehavior>();

            if (rbEnemy.linearVelocity.magnitude < rb.linearVelocity.magnitude)
            {
                health -= (rbEnemy.linearVelocity.magnitude + rb.linearVelocity.magnitude) * damageTakenMultiplier;
                Debug.Log("PHealth = " + health);
                Debug.Log("Enemy Health = " + enemyData.health);
            }
            else if ((rbEnemy.linearVelocity.magnitude > rb.linearVelocity.magnitude))
            {
                enemyData.health -= (rbEnemy.linearVelocity.magnitude + rb.linearVelocity.magnitude) * damageGivenMultiplier;
                Debug.Log("Enemy Health = " + enemyData.health);
                Debug.Log("PHealth = " + health);
            }
            else
            {
                Debug.Log("Collision with no damage set");
            }
        }
    }
}
