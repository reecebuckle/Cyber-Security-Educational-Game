using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

/*
 * Attached to every tile/node in order to highlight and calculate distance
 * This script was written from a discontinued Youtube tutorial available here:
 * https://www.youtube.com/watch?v=cK2wzBCh9cg&t=1558s&ab_channel=GameProgrammingAcademy
 */
namespace Map
{
    public class Tile : MonoBehaviour
    {
        [Header("Boolean Variables")] 
        //
        public bool walkable = true;
        public bool changed = false;
        public bool current = false;
        public bool target = false;
        public bool selectable = false;
        public bool attack = false;

        [Header("BFS Variables")] 
        //
        public List<Tile> adjacencyList = new List<Tile>();
        public bool visited = false;
        public Tile parent = null;
        public int distance = 0;

        private Material _matInstance;

        private void Start()
        {
            _matInstance = GetComponent<Renderer>().material;
            _matInstance.EnableKeyword("_DisplayEmitAmount"); //uncomment for web version
        }

        /*
         * These methods update the tile colour (initially was in update)
         */
        public void MarkSelectable()
        {
            selectable = true;
            _matInstance.SetFloat("_Metallic", 0.0f);
            _matInstance.color = Color.green;
        }
        
        public void MarkAttack()
        {
            _matInstance.SetFloat("_Metallic", 0.0f);
            _matInstance.color = Color.magenta;
        }
        
        public void MarkTarget()
        {
            target = true;
            _matInstance.SetFloat("_Metallic", 0.0f);
            _matInstance.color = Color.red;
        }
        
        public void MarkCurrent()
        {
            current = true; 
            _matInstance.SetFloat("_Metallic", 0.0f);
            _matInstance.color = Color.green;
        }
        
        public void MarkReset()
        {
            _matInstance.SetFloat("_Metallic", 1.0f);
            // todo change this to white for non web servers
            _matInstance.color = Color.black;
        }
        

        /*
        * INVOKED BY THE PATHFINDING SCRIPT TO FIND VALID PATHS TO MOVE TOWARDS (IN BFS MOTION)
        */
        public void FindNeighbours()
        {
            Reset();

            //Check the 4 directions (forwards, left, down and right)
            CheckTile(Vector3.forward);
            CheckTile(-Vector3.right);
            CheckTile(-Vector3.forward);
            CheckTile(Vector3.right);
           
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
         * INVOKED BY ATTACK RANGE ALGORITHM FOR 1 OR MORE TILES IN EACH DIRECTION
         */
        public void FindNeighboursInExtendedRange(int attackRange)
        {
            Reset();
            //Check the 4 directions (forwards, down, left, right)
            CheckTilesInExtendedDirection(Vector3.forward, attackRange);
            CheckTilesInExtendedDirection(-Vector3.right, attackRange);
            CheckTilesInExtendedDirection(-Vector3.forward, attackRange);
            CheckTilesInExtendedDirection(Vector3.right,attackRange);
        }

        /*
        * For the first tile!
        */
        private void CheckTilesInExtendedDirection(Vector3 direction, int attackRange)
        {
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);

            //returns an array of colliders
            Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

            //Iterate through colliders, check if there's a tile present
            foreach (Collider item in colliders)
            {
                Tile tile = item.GetComponent<Tile>();
                
                if (tile != null)
                {
                    RaycastHit hit;

                    //from centre of tile, look to see if something is there, if it is, add it to our adjacency list
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                    {
                        adjacencyList.Add(tile);
                        tile.MarkAttack();

                        //if attack range is still above 1, recursively call method and decrement range until it reaches 1 (and do this on the new tile)
                        if (attackRange > 1)
                            tile.CheckTilesInExtendedDirection(direction, (attackRange - 1));
                    }
                }
            }
        }
        
        /*
        * Reset all variables after every turn or when FindNeighbours is invoked
        */
        public void Reset()
        {
            //Clear Colour and fix all variables 
            MarkReset();
            adjacencyList.Clear();

            current = false;
            target = false;
            selectable = false;
            attack = false;

            visited = false;
            parent = null;
            distance = 0;
        }
        
        /*
         * Deprecated method that highlights the selected tile (old method, could also be called from within the code
        */
        private void SelectTile() {

            if (current)
            {
                _matInstance.SetFloat("_Metallic", 0.0f);
                _matInstance.color = Color.green;
            }

            else if (target)
            {
                _matInstance.SetFloat("_Metallic", 0.0f);
                _matInstance.color = Color.red;
            }

            else if (attack)
            {
                _matInstance.SetFloat("_Metallic", 0.0f);
                _matInstance.color = Color.magenta;
            }

            else if (selectable)
            {
                _matInstance.SetFloat("_Metallic", 0.0f);
                _matInstance.color = Color.green;
            }

            else
            {
                _matInstance.SetFloat("_Metallic", 1.0f);
                _matInstance.color = Color.black;
            }
        }
    }
}