using System.Runtime.InteropServices;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public GameObject player;
    private float offset = 4f; //offset on z axis by this amount
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + offset); //maintain x and y but follow player on z
    }
}
