using System.IO;
using UnityEngine;

namespace Movement
{
    public class PlayerMovementController : MovementRules //extends movement rules
    {
        private void Start() => Init();

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
                    if (hit.collider.tag == "Tile")
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