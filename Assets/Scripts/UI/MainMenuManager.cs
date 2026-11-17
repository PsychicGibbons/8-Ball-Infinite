using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    //Assignments
    private playerController controller;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject camTarget;
    [SerializeField] private GameObject[] Buttons;
    [SerializeField] private GameObject infoCanvas;
    private bool gameStarter;

    private void Start()
    {   //only one instance of playerController so it is safe to use this Find
        controller = FindAnyObjectByType<playerController>();
    }

    public void StartGame()
    {
        //On the Start button press, initiate the camera movement and make visual components of the menu disappear
        gameStarter = true;
        foreach (var button in Buttons)
        {
            button.SetActive(false);
        }
    }
    public void closeGame()
    {
        //We need a way to shut down the game
        Application.Quit();
    }

    private void FixedUpdate()
    {
        //If the player has clicked Start button, initiate camera movement and rotation to transition into gameplay
        if(gameStarter && Vector3.Distance(camTarget.transform.position, mainCam.transform.position) >= 1)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, camTarget.transform.position, 1f);
            mainCam.transform.LookAt(new Vector3(controller.gameObject.transform.position.x, controller.gameObject.transform.position.y, controller.gameObject.transform.position.z + 2), Vector3.up);
        }
        //When the camera gets to the standard gameplay position, ensure rotation is correct, allow the player to be controlled, deactivate this canvas and activate the canvas for gameplay
        else if(Vector3.Distance(camTarget.transform.position, mainCam.transform.position) <= 1)
        {
            mainCam.transform.rotation = Quaternion.Euler(90, 0, 0);
            controller.enabled = true;
            infoCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
