using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private GameObject Player;
    private Rigidbody rb;
    private Vector3 jumpVector;
    private float jumpTimer;
    public float health = 100;
    [SerializeField] private float detectionRadius, jumpSpeed, jumpTimeMin, jumpTimeMax;
    private bool playerDetected;

    private void Start()
    {
        Player = FindAnyObjectByType<playerController>().gameObject;
        rb = GetComponent<Rigidbody>();
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
    }

    private void FixedUpdate()
    {
        //Movement and player detection:

        if(Vector3.Distance(Player.transform.position, gameObject.transform.position) < detectionRadius)
        {
            Debug.Log("Player Detected");
            playerDetected = true;
        }
        else
        {
            Debug.Log("Player Out of Range");
            playerDetected = false;
        }

        if(playerDetected)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
            {
                Jump();
            }
        }

        //Health check and death
        if(health <= 0)
        {
            Die();
        }
    }

    void Jump()
    {
        jumpVector = new Vector3((gameObject.transform.position - Player.transform.position).x, 0 ,(gameObject.transform.position - Player.transform.position).z);
        rb.AddForce(jumpVector * -jumpSpeed, ForceMode.Impulse);
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        //This represents the detection range of the enemy
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);    
    }
}
