using Movement;
using Photon.Pun;
using Tiles;
using Units;
using UnityEngine;

namespace SoloDebugging
{
    /*
     * This version of the player controller is used solely to set up pathfinding
     */

    public class SoloController : Pathfinding
    {
        [Header("Current Units")] [SerializeField]
        private Unit selectedUnit; // currently selected unit

        public Unit unit1;
        public Unit unit2;


        /*
         * Cache all tiles on the map on the first frame when starting the player controller
         */
        private void Start() => CacheAllTiles();

        private void Update()
        {
            SelectUnit();

            if (selectedUnit)
            {
                if (!moving)
                {
                    FindSelectableTiles(selectedUnit);
                    CheckMouseClick();
                }
                else
                {
                    Move(selectedUnit);
                }
            }
        }

        /*
         * Checks if a unit has been selected!
         */
        private void SelectUnit()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //TODO check if this is our unit somewhere
                    if (hit.collider.CompareTag("Unit"))
                    {
                        Unit u = hit.collider.GetComponent<Unit>();

                        Debug.Log("Unit Selected");
                        selectedUnit = u;
                    }
                }
            }
        }

        /*
         * Invoked IF a tile is selected within the selected units range
         */
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
                        Debug.Log("Tile selected");
                        Tile t = hit.collider.GetComponent<Tile>();

                        if (t.selectedTile)
                        {
                            MoveToTile(t);
                        }
                    }
                }
            }
        }


/*
private void TrySelect (Vector3 selectPos)
{
// see if we're selecting one of our units
Unit unit = units.Find(x => x.transform.position == selectPos);

// if we're selecting our unit - select it
if(unit != null)
{
   SelectUnit(unit);
   return;
}

// if we don't have a selected unit - don't do anything else
if(!selectedUnit) return;

// are we selecting an enemy unit?
Unit enemyUnit = enemy.units.Find(x => x.transform.position == selectPos);

if(enemyUnit != null)
{
   TryAttack(enemyUnit);
   return;
}

// if we're not selecting a unit or attacking an enemy, try to move the selected unit
TryMove(selectPos);
}

// called when we click on a unit
void SelectUnit (Unit unitToselect)
{
// can we select the unit?
if(!unitToselect.CanSelect())
   return;

// un-select the current unit
if(selectedUnit != null)
   selectedUnit.ToggleSelect(false);

// select the new unit
selectedUnit = unitToselect;
selectedUnit.ToggleSelect(true);

// set the unit info text
GameUI.instance.SetUnitInfoText(unitToselect);
}

// de-selects the selected unit
void DeSelectUnit ()
{
selectedUnit.ToggleSelect(false);
selectedUnit = null;

// disable unit info text
GameUI.instance.unitInfoText.gameObject.SetActive(false);
}

// selects a unit which is able to move / attack
void SelectNextAvailableUnit ()
{
Unit availableUnit = units.Find(x => x.CanSelect());

if(availableUnit != null)
   SelectUnit(availableUnit);
else
   DeSelectUnit();
}

// attempts to attack the requested enemy unit
void TryAttack (Unit enemyUnit)
{
// can we attack the enemy unit?
if(selectedUnit.CanAttack(enemyUnit.transform.position))
{
   selectedUnit.Attack(enemyUnit);
   SelectNextAvailableUnit();
   GameUI.instance.UpdateWaitingUnitsText(units.FindAll(x => x.CanSelect()).Count);
}
}

// attempts to move to the requested position
void TryMove (Vector3 movePos)
{
// can we move to the position?
if(selectedUnit.CanMove(movePos))
{
   selectedUnit.Move(movePos);
   SelectNextAvailableUnit();
   GameUI.instance.UpdateWaitingUnitsText(units.FindAll(x => x.CanSelect()).Count);
}
}

// called when our turn ends
public void EndTurn ()
{
// de-select unit
if(selectedUnit != null)
   DeSelectUnit();

// start the next turn
GameManager.instance.photonView.RPC("SetNextTurn", RpcTarget.All);
}

// called when our turn has begun
public void BeginTurn ()
{
foreach(Unit unit in units)
   unit.usedThisTurn = false;

// update the UI
GameUI.instance.UpdateWaitingUnitsText(units.Count);
}*/
    }
}