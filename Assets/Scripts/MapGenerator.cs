using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MapGenerator : MonoBehaviour
{
    public GameObject[] setPieces;
    public static List<GameObject> currentPieces = new List<GameObject>();

    void Start()
    {
        currentPieces.Clear();
        currentPieces.Add(Instantiate(setPieces[0], new Vector3(0, 0, -10), Quaternion.identity));
    }
    private void Update()
    {
        for (int i = 0; i < currentPieces.Count; i++)
        {
            if (currentPieces[i] == null)
            {
                currentPieces.Remove(currentPieces[i]);
                i--;
            }
        }
        if (currentPieces.Count < 5)
        {
            int listLength = currentPieces.Count;
            GameObject spawnPos = currentPieces[listLength - 1].transform.GetChild(1).gameObject;
            int randPiece = Random.Range(0, setPieces.Length);
            currentPieces.Add(Instantiate(setPieces[randPiece], spawnPos.transform.position, Quaternion.identity));
        }
    }
}
