using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform targetPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool MoveableUI;

    public void UpdateHealthBar(float newValue, float totalValue)
    {
        slider.value = newValue / totalValue;
    }
    
    private void Update()
    {
        if (MoveableUI)
        {
            transform.rotation = Camera.main.transform.rotation;
            transform.position = targetPos.position + offset;
        }
    }
}
