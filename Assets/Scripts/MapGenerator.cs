using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] Setpieces;
    private GameObject lastGeneratedPiece, nextPiece, generatePoint;

    void Generate()
    {
        nextPiece = Setpieces[Random.Range(0,Setpieces.Length)];
        lastGeneratedPiece = Instantiate(nextPiece, generatePoint.transform.position, Quaternion.identity);
    }
}
