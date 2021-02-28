using System.IO;
using UnityEngine;
using Photon.Pun;

namespace Movement
{
    public class PlayerMovementController : MovementRules //extends movement rules
    {
        [SerializeField] private GameObject myAvatar;
        [SerializeField] private PhotonView PV;
        private void Start() => Init();

        private void Update()
        {
            //if not instantiated yet
            // if (PV != null)
            //   return;

            if (PV.IsMine)
            {
                //if it's not their turn, disable any movement
                if (!turn)
                    return;

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