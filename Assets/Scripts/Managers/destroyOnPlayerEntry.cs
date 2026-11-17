using UnityEngine;

public class destroyOnPlayerEntry : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //when player enters this destroy the parent (one of the building blocks for world) this is offset and is used to destroy once player leaves and it is offscreen
            Destroy(transform.parent.gameObject);
        }
    }
}
