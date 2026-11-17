using UnityEngine;

public class backBlockActivation : MonoBehaviour
{
    public GameObject BackBlock;
    private void OnTriggerEnter(Collider other)
    {
        //if player enters this box (offset forward from main block) the back block is activated
        if (other.CompareTag("Player"))
        {
            BackBlock.SetActive(true);
        }
    }
}
