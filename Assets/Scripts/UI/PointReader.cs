using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointReader : MonoBehaviour
{
    [SerializeField] private playerController Controller;
    [SerializeField] private TextMeshProUGUI pointText;
    private float points = 100;
    public void UpdatePoints(float pointsAdded)
    {
        points += pointsAdded;
        pointText.text = "Points: " + points;
    }
    public float GetPoints()
    {
        return points;
    }
}
