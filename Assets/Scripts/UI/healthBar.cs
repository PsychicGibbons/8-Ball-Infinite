using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //Assignments
    [SerializeField] private Slider slider;
    [SerializeField] private Transform targetPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool MoveableUI;

    //Coroutine is called when the health bar's value needs to be updated
    public void UpdateHealthBar(float newValue, float totalValue)
    {
        //Current health / Max health makes a decimal compatible with a canvas slider
        slider.value = newValue / totalValue;
    }
    
    private void Update()
    {
        //if the bar needs to move around on screen i.e. for each enemy
        if (MoveableUI)
        {
            //Set the bar to a rotation visible to the camera (the same as the camera since the bar is 2D)
            transform.rotation = Camera.main.transform.rotation;
            //Set the position of the health bar to the target's position + a manually set offset so it isn't inside the target
            transform.position = targetPos.position + offset;
        }
    }
}
