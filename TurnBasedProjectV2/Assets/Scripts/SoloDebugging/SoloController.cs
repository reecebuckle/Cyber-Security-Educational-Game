using Movement;
using Photon.Pun;
using Tiles;
using UnityEngine;

namespace SoloDebugging
{
    /*
     * This version of the player controller is used solely to set up pathfinding
     */
    public class SoloController : Pathfinding 
    {
        /*
         * Cache all tiles on the map on the first frame when starting the player controller
         */
        private void Start() => CacheAllTiles();

        private void Update()
        {
            if (!moving)
            {
                FindSelectableTiles();
                CheckMouseClick();
            }
            else
            {
                Move();
            }
            
             
        }


        private void CheckMouseClick()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Tile"))
                    {
                        Tile t = hit.collider.GetComponent<Tile>();

                        if (t.selectedTile)
                        {
                            MoveToTile(t);
                        }
                    }
                }
            }
        }
    }
}