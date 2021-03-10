using System.Collections.Generic;
using UnityEngine;

/*
 * Attached to every tile/node in order to highlight and calculate distance
 * This script was written from a discontinued Youtube tutorial available here:
 * https://www.youtube.com/watch?v=cK2wzBCh9cg&t=1558s&ab_channel=GameProgrammingAcademy
 */
public class Tile : MonoBehaviour
{
    [Header("Boolean Variables")]
    public bool walkable = true; //is the tile walkable? Cannot step on opposing players!
    public bool currentTile = false;
    public bool targetTile = false;
    public bool selectedTile = false;

    [Header("BFS Variables")] public List<Tile> adjacencyList = new List<Tile>();
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0; //set default to 5

    private void Start() => Reset();

    private void Update() => SelectTile();

    /*
    * Highlights the selected tile
    */
    private void SelectTile()
    {
        if (currentTile)
            GetComponent<Renderer>().material.color = Color.magenta;
        else if (targetTile)
            GetComponent<Renderer>().material.color = Color.green;
        else if (selectedTile)
            GetComponent<Renderer>().material.color = Color.red;
        else
            GetComponent<Renderer>().material.color = Color.white;
    }

    /*
    * When this is invoked, the tile selected is reset into it's original state
    */
    public void FindNeighbours()
    {
        Reset();

        //Check the 4 directions (forwards, backwards, right and left)
        CheckTile(Vector3.forward);
        CheckTile(-Vector3.forward);
        CheckTile(Vector3.right);
        CheckTile(-Vector3.right);
    }

    /*
    * Checks adjacent tiles by observing their colliders with a raycast.
    * If it's a valid tile to move to, this method will add it to an adjacency list which is used in BFS
    */
    private void CheckTile(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            
        //returns an array of colliders
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        //Iterate through colliders, check if there's a tile present
        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();

            //if the tile is null or not walkable, move onto the next collider..
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;

                //from centre of tile, look to see if something is there, if it is, add it to our adjacency list
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                    adjacencyList.Add(tile);
            }
        }
    }

    /*
    * Reset all variables after every turn or when FindNeighbours is invoked
    */
    public void Reset()
    {
        adjacencyList.Clear();
        currentTile = false;
        targetTile = false;
        selectedTile = false;
        visited = false;
        parent = null;
        distance = 0;
    }
}