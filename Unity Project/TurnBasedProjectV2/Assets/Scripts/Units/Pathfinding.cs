using System.Collections;
using System.Collections.Generic;
using Map;
using Photon.Pun;
using UnityEngine;

/*
 * This script is used to find the path for potential units
 * This script was written from a discontinued Youtube tutorial available here:
 * https://www.youtube.com/watch?v=cK2wzBCh9cg&t=1558s&ab_channel=GameProgrammingAcademy
 */
namespace Units
{
    public class Pathfinding : MonoBehaviour // TODO reverse this to pun if bugged
    {
        //is the unit currently in movement?
        public bool moving = false;

        private Vector3 _velocity = new Vector3();
        private Vector3 _heading = new Vector3();
        private float _halfHeight = 0;

        //selectableTiles show when it's the units turn 
        private List<Tile> selectableTiles = new List<Tile>();
        private GameObject[] _tiles;

        //Stack gives us the path in reverse order
        private Stack<Tile> path = new Stack<Tile>();
        private Tile _currentTile; // current tile in the pathfinding process
        
        /*
        * Cache all tiles right away
        * Needs to be initialised once at the beginning of the game
        */
        protected void CacheAllTiles()
        {
            _tiles = GameObject.FindGameObjectsWithTag("Tile");
            // Find where the unit sits on the tile
            // Going to be calculated a lot, so performance friendly to calculate once at the beginning
            _halfHeight = GetComponent<Collider>().bounds.extents.y;
        }

        /*
         * Utility function to return current tile underneath a selected Unit
        */
        private void GetCurrentTile()
        {
            _currentTile = GetTargetTile();
            _currentTile.MarkCurrent();
        }

        /*
        * Returns the target tile underneath the unit, or from the units collider
        */
        private Tile GetTargetTile()
        {
            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
                tile = hit.collider.GetComponent<Tile>();

            return tile;
        }

        /*
        * Finds all adjacent neighbours 
        */
        private void ComputeAdjacencyList()
        {
            foreach (GameObject tile in _tiles)
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
            GetCurrentTile();
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
                t.MarkSelectable();

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
            tile.MarkTarget();
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
            //as long as there's something in path we can move
            if (path.Count > 0)
            {
                Tile t = path.Peek();
                Vector3 targetPos = t.transform.position;

                targetPos.y += _halfHeight + t.GetComponent<Collider>().bounds.extents.y;

                //if distance is very small then....
                if (Vector3.Distance(transform.position, targetPos) >= 0.05f)
                {
                    CalculateHeading(targetPos);
                    CalculateHorizontalVelocity(unit);

                    //face direction of movement
                    transform.forward = _heading;
                    transform.position += _velocity * Time.deltaTime;
                }
                else
                {
                    transform.position = targetPos;
                    path.Pop(); // we don't need that child on stack anymore as we've reached it
                }
            }
            else
            {
                unit.ToggleMovedThisTurn(true);
                RemoveSelectableTiles();
                moving = false;
            }
        }

        /*
        * Move forward in the direction of the heading
        */
        private void CalculateHorizontalVelocity(Unit unit) => _velocity = _heading * unit.GetMovementSpeed();


        /*
        * Simple function used to calculate heading ... ?
         */
        private void CalculateHeading(Vector3 targetPos)
        {
            _heading = targetPos - transform.position;
            //normalise (unit vector magnitude of 1)
            _heading.Normalize();
        }

        /*
        * Used to remove selectable tiles
         */
        protected void RemoveSelectableTiles()
        {
            if (_currentTile != null)
            {
                //if there is a current tile, set it to false
                _currentTile.current = false;
                _currentTile.MarkReset();
                _currentTile = null;
            }

            foreach (Tile tile in selectableTiles)
                tile.Reset();

            selectableTiles.Clear();
        }
    }
}