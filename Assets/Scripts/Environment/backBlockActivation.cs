using UnityEngine;

public class backBlockActivation : MonoBehaviour
{
    public GameObject BackBlock;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BackBlock.SetActive(true);
        }
    }
}
