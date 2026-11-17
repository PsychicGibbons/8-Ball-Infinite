using UnityEngine;

public class KillBox : MonoBehaviour
{
    private PointReader pointReader;
    private playerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find objects with these scripts as there is only 1 per scene
        pointReader = FindAnyObjectByType<PointReader>();
        playerController = FindAnyObjectByType<playerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //kill enemy and update players points with public function
            Destroy(other.gameObject);
            pointReader.UpdatePoints(20);
        }
        else if (other.CompareTag("Player"))
        {
            //kill player, same as if it had gone to 0 health
            playerController.Die();
        }
    }
}
