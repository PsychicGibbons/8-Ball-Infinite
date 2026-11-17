using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private GameObject Player;
    private Rigidbody rb, PlayerRB;
    private Vector3 jumpVector;
    private playerController playerController;
    private PointReader pointHolder;
    private float jumpTimer;
    public float health = 100;
    [SerializeField] private float detectionRadius, jumpSpeed, jumpTimeMin, jumpTimeMax;
    private bool playerDetected;
    public bool moving;
    public float enemySpeedThisFrame;

    private void Start()
    {
        transform.parent = null;

        Player = FindAnyObjectByType<playerController>().gameObject;
        pointHolder = FindAnyObjectByType<PointReader>();
        playerController = FindAnyObjectByType<playerController>();
        PlayerRB = Player.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
    }

    private void FixedUpdate()
    {
        enemySpeedThisFrame = rb.linearVelocity.magnitude;
        //Movement and player detection:
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -20, 0.1f), transform.position.z);
        
        if(rb.linearVelocity.magnitude <= 0.1f)
        {
            moving = false;
        }

        if (Vector3.Distance(Player.transform.position, gameObject.transform.position) < detectionRadius && PlayerRB.linearVelocity.magnitude < 1)
        {
            Debug.Log("Player Detected");
            playerDetected = true;
        }
        else
        {
            Debug.Log("Player Out of Range");
            playerDetected = false;
        }

        if(playerDetected && !moving)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
            {
                Attack();
            }
        }

        //Health check and death
        if(health <= 0)
        {
            Die();
        }
    }

    void Attack()
    {
        moving = true;
        jumpVector = new Vector3((gameObject.transform.position - Player.transform.position).x, 0 ,(gameObject.transform.position - Player.transform.position).z);
        rb.AddForce(jumpVector * -jumpSpeed, ForceMode.Impulse);
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
    }

    void Die()
    {
        pointHolder.UpdatePoints(10);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        //This represents the detection range of the enemy
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);    
    }
}
