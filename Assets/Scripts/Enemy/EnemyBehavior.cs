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
        //Finding objects by the scripts they have attatched as for all of these scripts there is only a simgle instance of them in game
        Player = FindAnyObjectByType<playerController>().gameObject;
        pointHolder = FindAnyObjectByType<PointReader>();
        playerController = FindAnyObjectByType<playerController>();
        //getting own and players rigidbody so we can check speed
        PlayerRB = Player.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        //makes the enemy attack at a random time so they are a bit less predictable
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
    }

    private void FixedUpdate()
    {
        //Speed is an important part of the damage calculation in this game
        enemySpeedThisFrame = rb.linearVelocity.magnitude;
        //Movement and player detection:
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -10f, 10f), Mathf.Clamp(transform.position.y, -15f, 0.1f), transform.position.z);

        //check for whether the enemy is moving or not, this tells the enemy whether or not the player should take damage on a collision with the enemy
        if (rb.linearVelocity.magnitude <= 0.1f)
        {
            moving = false;
        }

        //This is the 'detection' code, if the player is within attack range the enemy starts counting down to attack
        if (Vector3.Distance(Player.transform.position, gameObject.transform.position) < detectionRadius && PlayerRB.linearVelocity.magnitude < 1)
        {
            playerDetected = true;
        }
        else
        {
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
        //Set moving to true (and the player is meant to be hurt) then launch the enemy at the player. Damage is calculated by the playerController
        moving = true;
        jumpVector = new Vector3((gameObject.transform.position - Player.transform.position).x, 0 ,(gameObject.transform.position - Player.transform.position).z);
        rb.AddForce(jumpVector * -jumpSpeed, ForceMode.Impulse);
        //randomly reset the timer for less predictability
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
    }

    void Die()
    {
        //The player gets points on defeating an enemy along with physically making it vanish
        pointHolder.UpdatePoints(10);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        //This represents the detection range of the enemy in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);    
    }
}
