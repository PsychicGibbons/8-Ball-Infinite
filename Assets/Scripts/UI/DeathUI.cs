using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    //When the player clicks the button to restart, reload the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
