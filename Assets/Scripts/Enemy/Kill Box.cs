using UnityEngine;

public class KillBox : MonoBehaviour
{
    private PointReader pointReader;
    private playerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointReader = FindAnyObjectByType<PointReader>();
        playerController = FindAnyObjectByType<playerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            pointReader.UpdatePoints(20);
        }
        else if (other.CompareTag("Player"))
        {
            playerController.Die();
        }
    }
}
