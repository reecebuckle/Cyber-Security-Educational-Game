using System.Collections;
using System.Collections.Generic;
using Map;
using Photon.Pun;
using UnityEngine;

/*
 * This script provides breadth-first-search functionality for finding potential tiles a unit can to move to.
 * It was adapted from the following youtube tutorial:
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
        */
        private void BreadthFirstSearch(Unit unit)
        {
            // Create a Queue data structure to enqueue tiles to process
            Queue<Tile> tiles = new Queue<Tile>();
            
            // Enqueue the current tile (starting from underneath the selected unit)
            tiles.Enqueue(_currentTile);
            
            // Mark as visited so we do not process this tile twice
            _currentTile.visited = true;

            // Iterate through the queue, whilst enqueuing adjacent tiles of each tile
            // If this reaches 0, then there are no legal moves
            while (tiles.Count > 0)
            {
                // Dequeue the first tile and add it to a list of selectable tiles
                Tile tile = tiles.Dequeue();
                selectableTiles.Add(tile);

                // Mark tile as selectable (highlights the tile green)
                tile.MarkSelectable();

                // If tile is out of range from the unit, dequeue the next tile
                if (tile.distance >= unit.GetMovementDistance()) continue;
                
                // Iterate through the 4 adjacent tiles to the current dequeued tile
                foreach (Tile t in tile.adjacencyList)
                {
                    // If tile has already been processed, dequeue next tile
                    if (t.visited) continue;
                    
                    // Assign the parent of the adjacent tile and mark as visited
                    t.parent = tile;
                    t.visited = true;
                    
                    // Update distance oto keep track of how far we are from the starting tile
                    t.distance = 1 + tile.distance;
                    
                    // Enqueue tile from adjacency list
                    tiles.Enqueue(t);
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