using TMPro;
using UnityEngine;

public class PointReader : MonoBehaviour
{
    [SerializeField] private playerController Controller;
    [SerializeField] private TextMeshProUGUI pointText;
    private float points;

    private void Start()
    {
        UpdatePoints(20);
    }
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
