using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject Player;
    public SphereCollider detectionRadius;
    private Rigidbody rb;
    private float jumpTimer, jumpTimeMin, jumpTimeMax;
    private bool playerDetected;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
    }

    private void FixedUpdate()
    {
        while(playerDetected)
        {
            jumpTimer -= Time.deltaTime;
            if(jumpTimer <= 0)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector3(1,0,1),ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
}
