using UnityEngine;

public class playerController : MonoBehaviour
{
    private Vector3 mouseInputPos;
    private Vector3 mouseReleasePos;
    private Vector3 dragPath;
    private bool dragging = false;
    private float ballSpeed = 2f; // in future make it so ballspeed is dependent on dragpath.magnitude

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        if (Input.GetMouseButtonDown(0) && !dragging)
        {
            dragging = true;
            mouseInputPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            mouseReleasePos = Input.mousePosition;

            dragPath = mouseReleasePos - mouseInputPos;
            rb.AddForce(dragPath.normalized * ballSpeed, ForceMode.Impulse);
        }
    }
}
