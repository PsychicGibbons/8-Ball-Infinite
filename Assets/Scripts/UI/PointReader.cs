using TMPro;
using UnityEngine;

public class PointReader : MonoBehaviour
{
    [SerializeField] private playerController Controller;
    [SerializeField] private TextMeshProUGUI pointText;
    private float points;

    //When the game begins, give the player enough points for their first upgrade
    private void Start()
    {
        UpdatePoints(20);
    }

    //Coroutine called when points need to be increased or decreased, then text is updated so the player knows how many points they have
    public void UpdatePoints(float pointsAdded)
    {
        points += pointsAdded;
        pointText.text = "Points: " + points;
    }

    //Getter for when the number of points need to be found
    public float GetPoints()
    {
        return points;
    }
}
