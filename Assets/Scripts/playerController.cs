using UnityEngine;

public class playerController : MonoBehaviour
{
    private Vector3 mouseInputPos;
    private Vector3 mouseReleasePos;
    private Vector2 dragPath;
    private bool dragging = false;
    private float ballSpeed = 30f; // in future make it so ballspeed is dependent on dragpath.magnitude
    private bool dragged = false;

    private Rigidbody rb;

    public GameObject camera;
    private float cameraPanSpeed = 15f;
    private bool cameraPanning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementCheck();
        if (rb.linearVelocity.magnitude < 1f)
        {
            Vector3 cameraTargetPos = new Vector3(0, 20f, transform.position.z + 5);
            if (Vector3.Distance(camera.transform.position, cameraTargetPos) > 0.05)
            {
                cameraPanning = true;
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, cameraTargetPos, cameraPanSpeed * Time.deltaTime);
            }
            else
            {
                cameraPanning = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (dragged)
        {
            ApplyMovement();
        }  
    }

    private void MovementCheck()
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
            dragged = true;
        }
    }
    private void ApplyMovement()
    {
        dragged = false;
        if (rb.linearVelocity.magnitude < 1f && !cameraPanning)
        {
            dragPath = mouseReleasePos - mouseInputPos;
            Vector3 force = new Vector3(dragPath.x, 0, dragPath.y);
            rb.AddForce(-force.normalized * ballSpeed, ForceMode.Impulse); //negative force so if backwards drag it goes forwards
        }
    }
}
