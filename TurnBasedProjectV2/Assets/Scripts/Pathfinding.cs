using System.Collections.Generic;
using Photon.Pun;
using Tiles;
using Units;
using UnityEngine;

/*
 * This script is used to find the path for potential units
 * This script was written from a discontinued Youtube tutorial available here:
 * https://www.youtube.com/watch?v=cK2wzBCh9cg&t=1558s&ab_channel=GameProgrammingAcademy
 */
public class Pathfinding : MonoBehaviourPun
{
    //is the unit currently in movement?
    public bool moving = false;

    private Vector3 velocity = new Vector3();
    private Vector3 heading = new Vector3();
    private float halfHeight = 0;

    //selectableTiles show when it's the units turn 
    private List<Tile> selectableTiles = new List<Tile>();
    private GameObject[] tiles;

    //Stack gives us the path in reverse order
    public Stack<Tile> path = new Stack<Tile>();
    private Tile _currentTile;


    /*
     * Cache all tiles right away
     * Needs to be initialised once at the beginning of the game
     */
    protected void CacheAllTiles()
    {
        Debug.Log("Caching all tiles");
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        // Find where the unit sits on the tile
        // Going to be calculated a lot, so performance friendly to calculate once at the beginning
        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    /*
     * Utility function to return current tile underneath a selected Unit
     * 
     * TODO BUG LIST: when you get current unit, sometimes you get a null reference exception!
     */
    private void GetCurrentTile(Unit unit)
    {
        _currentTile = GetTargetTile(unit);
        _currentTile.currentTile = true;
    }

    /*
     * Returns the target tile underneath the unit, or from the units collider
     */
    private static Tile GetTargetTile(Unit unit)
    {
        RaycastHit hit;
        Tile tile = null;

        //may need to outline raycast here
        if (Physics.Raycast(unit.gameObject.transform.position, -Vector3.up, out hit, 2))
        {
            tile = hit.collider.GetComponent<Tile>();
            Debug.Log("Tile is underneathe!");
        }

        return tile;
    }

    /*
    * Finds all adjacent neighbours 
    */
    private void ComputeAdjacencyList()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbours();
        }
    }

    /*
    * Find all selectable tiles
    */
    protected void FindSelectableTiles(Unit unit)
    {
        ComputeAdjacencyList();
        GetCurrentTile(unit);
        BreadthFirstSearch(unit);
    }

    /*
     * Find movable tiles within range of current selected unit
     * Utilises Breadth First Search
     * 
     */
    private void BreadthFirstSearch(Unit unit)
    {
        //Begin BFS
        Queue<Tile> BFS = new Queue<Tile>();

        BFS.Enqueue(_currentTile);
        _currentTile.visited = true;

        //if this ever reached 0, then player cannot move at all
        while (BFS.Count > 0)
        {
            Tile t = BFS.Dequeue();
            selectableTiles.Add(t);

            //activate selectable trigger (red) of tile
            t.selectedTile = true;

            //if distance is greater than move amount, skip BFS
            if (t.distance < unit.GetMovementDistance())
            {
                //only process a tile once (if it's been visited already, ignore it)
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        //need to keep track of how far we are from the start tile
                        //we start at 0 at the starting node, and add 1 with each tile away
                        tile.distance = 1 + t.distance;
                        BFS.Enqueue(tile);
                    }
                }
            }
        }
    }

    /*
     * Move to target tile
     */
    protected void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.targetTile = true;
        moving = true;

        Tile nextTile = tile;

        while (nextTile != null)
        {
            path.Push(nextTile);
            nextTile = nextTile.parent;
        }
    }

    /*
    * Purpose is to move from just one tile to the next until we run out of tiles
    */
    protected void Move(Unit unit)
    {
        //as lon gas there's something in path we can move
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 targetPos = t.transform.position;

            targetPos.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            //if distance is very small then....
            if (Vector3.Distance(unit.transform.position, targetPos) >= 0.05f)
            {
                CalculateHeading(targetPos, unit);
                CalculateHorizontalVelocity(unit);

                //face direction of movement
                unit.transform.forward = heading;
                unit.transform.position += velocity * Time.deltaTime;
            }
            else
            {
                unit.transform.position = targetPos;
                path.Pop(); // we don't need that child on stack anymore as we've reached it
            }
        }
        else
        {
            //TODO maybe relocate this moved this turn
            unit.ToggleMovedThisTurn(true);
            RemoveSelectableTiles();
            moving = false;
        }
    }

    /*
    * Move forward in the direction of the heading
    */
    private void CalculateHorizontalVelocity(Unit unit) => velocity = heading * unit.GetMovementSpeed();


    /*
     * Simple function used to calculate heading ... ?
     */
    private void CalculateHeading(Vector3 targetPos, Unit unit)
    {
        heading = targetPos - unit.transform.position;
        //normalise (unit vector magnitude of 1)
        heading.Normalize();
    }

    /*
    * Used to remove selectable tiles
    */
    protected void RemoveSelectableTiles()
    {
        if (_currentTile != null)
        {
            //if there is a current tile, set it to false
            _currentTile.currentTile = false;
            _currentTile = null;
        }

        foreach (var tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    /*
     * Returns true if tiles are found, false if not (prevents double checking)
     */
    public bool AreTilesFound()
    {
        if (selectableTiles.Count > 0)
            return true;
        else return false;
    }
}