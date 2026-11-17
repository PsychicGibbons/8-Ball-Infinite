using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MapGenerator : MonoBehaviour
{
    public GameObject[] setPieces; //stores all spawnable setpiece prefabs
    public static List<GameObject> currentPieces = new List<GameObject>(); //stores all of the currently spawned pieces

    void Start()
    {
        currentPieces.Clear(); //clear current pieces (just to make sure it is empty
        currentPieces.Add(Instantiate(setPieces[0], new Vector3(0, 0, -10), Quaternion.identity)); //start with a blank set piece so player cant spawn inside something
    }
    private void Update()
    {
        for (int i = 0; i < currentPieces.Count; i++)
        {
            if (currentPieces[i] == null)
            {
                //if a piece is null (which happens when they are destroyed after player leaves them) remove it from the list
                currentPieces.Remove(currentPieces[i]);
                i--; //lower i to avoid index out of range exception
            }
        }
        if (currentPieces.Count < 5)
        {
            int listLength = currentPieces.Count; //get length of current list so you can find final item in list
            GameObject spawnPos = currentPieces[listLength - 1].transform.GetChild(0).gameObject; //from final item in list, get its first child (spawn point) and assign it to gameobject spawnpos
            int randPiece = Random.Range(0, setPieces.Length); //pick a random piece from the prefabs
            currentPieces.Add(Instantiate(setPieces[randPiece], spawnPos.transform.position, Quaternion.identity)); //spawn it at spawnpos
        }
    }
}
