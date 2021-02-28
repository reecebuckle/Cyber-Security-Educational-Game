using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

/*
 * 
 */
namespace Movement
{
    public class MovementRules : MonoBehaviour
    {
        //only true when it's this units turn
        public bool turn = false;
        
        public int move = 5;
        public int moveSpeed = 2;
        public bool moving = false;

        private Vector3 velocity = new Vector3();
        private Vector3 heading = new Vector3();

        float halfHeight = 0;

        private List<Tile> selectableTiles = new List<Tile>();
        private GameObject[] tiles;

        //Stack gives us the path in reverse order
        public Stack<Tile> path = new Stack<Tile>();
        private Tile _currentTile;


        /*
         * Cache all tiles on the board right away
         */
        protected void Init()
        {
            tiles = GameObject.FindGameObjectsWithTag("Tile");

            halfHeight = GetComponent<Collider>().bounds.extents.y;

            TurnManager.AddUnit(this);
        }

        /*
         * 
         */
        private void GetCurrentTile()
        {
            _currentTile = GetTargetTile(gameObject);
            _currentTile.currentTile = true;
        }

        /*
         * 
         */
        private static Tile GetTargetTile(GameObject target)
        {
            RaycastHit hit;
            Tile tile = null;

            //may need to outline raycast here
            if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
                tile = hit.collider.GetComponent<Tile>();

            return tile;
        }

        /*
         * 
         */
        private void ComputeAdjacencyList()
        {
            // tiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (GameObject tile in tiles)
            {
                Tile t = tile.GetComponent<Tile>();
                t.FindNeighbours();
            }
        }

        /*
         * Find all selectable tiles
         */
        public void FindSelectableTiles()
        {
            ComputeAdjacencyList();
            GetCurrentTile();
            BreadthFirstSearch();
        }

        /*
         * Find Selectable tiles by applying Breadth First Search
         */
        private void BreadthFirstSearch()
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
                if (t.distance < move)
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
        public void MoveToTile(Tile tile)
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
        public void Move()
        {
            //as lon gas there's something in path we can move
            if (path.Count > 0)
            {
                Tile t = path.Peek();
                Vector3 targetPos = t.transform.position;

                targetPos.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

                //if distance is very small then....
                if (Vector3.Distance(transform.position, targetPos) >= 0.05f)
                {
                    CalculateHeading(targetPos);
                    CalculateHorizontalVelocity();

                    //face direction of movement
                    transform.forward = heading;
                    transform.position += velocity * Time.deltaTime;
                }
                else
                {
                    transform.position = targetPos;
                    path.Pop(); // we don't need that child on stack anymore as we've reached it
                }
            }
            else
            {
                RemoveSelectableTiles();
                moving = false;

                TurnManager.EndTurn();
            }
        }

        /*
         * Move forward in the direction of the heading
         */
        private void CalculateHorizontalVelocity() => velocity = heading * moveSpeed;


        /*
         * Simple function used to calculate heading ... ?
         */
        private void CalculateHeading(Vector3 targetPos)
        {
            heading = targetPos - transform.position;
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
         * Invoked when a unit can begin their turn / move
         */
        public void BeginUnitTurn() => turn = true;

        /*
         * To be invoked when movement ends, or an action is taken!
         */
        public void EndUnitTurn() => turn = false;
        
    }
}