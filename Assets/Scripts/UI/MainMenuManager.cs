using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private playerController controller;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject camTarget;
    [SerializeField] private GameObject[] Buttons;
    [SerializeField] private GameObject infoCanvas;
    private bool gameStarter;

    private void Start()
    {
        controller = FindAnyObjectByType<playerController>();
    }

    public void StartGame()
    {
        gameStarter = true;
        foreach (var button in Buttons)
        {
            button.SetActive(false);
        }
    }
    public void closeGame()
    {
        Application.Quit();
    }

    private void FixedUpdate()
    {

        Debug.Log(Vector3.Distance(camTarget.transform.position, mainCam.transform.position));
        if(gameStarter && Vector3.Distance(camTarget.transform.position, mainCam.transform.position) >= 1)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, camTarget.transform.position, 1f);
            mainCam.transform.LookAt(new Vector3(controller.gameObject.transform.position.x, controller.gameObject.transform.position.y, controller.gameObject.transform.position.z + 2), Vector3.up);
        }
        else if(Vector3.Distance(camTarget.transform.position, mainCam.transform.position) <= 1)
        {
            mainCam.transform.rotation = Quaternion.Euler(90, 0, 0);
            controller.enabled = true;
            infoCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
