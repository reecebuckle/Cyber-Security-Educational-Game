using System.Collections.Generic;
using Map;
using Photon.Pun;
using UI;
using Units;
using UnityEngine;

namespace SoloDebugging
{
    public class SoloUnit : MonoBehaviourPun
    {
        private List<Unit> _unitsInRange = new List<Unit>();
        private List<Tile> _tilesInRange = new List<Tile>();
        protected bool moveSelected;
        private GameObject[] tiles;

        [Header("Move Information")]
        //
        [SerializeField] private string abilityName;
        [SerializeField] private string[] information;

        /*
        * Cache all tiles right away for used when resetting tile options
        */
        private void Start() => tiles = GameObject.FindGameObjectsWithTag("Tile");

        /*
         * Returns units one tile in range
         */
        protected List<Unit> FindUnitsInRange(Unit sourceUnit, int distance)
        {
            _unitsInRange.Clear();
            FindUnitWithDirection(sourceUnit, Vector3.forward, distance);
            FindUnitWithDirection(sourceUnit, -Vector3.forward, distance);
            FindUnitWithDirection(sourceUnit, Vector3.right, distance);
            FindUnitWithDirection(sourceUnit, -Vector3.right, distance);
            
            return _unitsInRange;
        }

        /*
         * Searches a particular direction, if an enemy is in range, adds it to the list!
         */
        private void FindUnitWithDirection(Unit sourceUnit, Vector3 direction, int distance)
        {
            RaycastHit hit;
            Unit otherUnit = null;

            if (Physics.Raycast(sourceUnit.gameObject.transform.position, direction, out hit, distance))
            {
                otherUnit = hit.collider.GetComponent<Unit>();
                _unitsInRange.Add(otherUnit);
            }
        }

        /*
         * Checks the attack flow process to validate attacks
         */
        protected bool AttackFlowProcess(Unit unitAttacking, int actionPoints, int attackRange)
        {
            // Reset other selected units if swapping
            ResetSelection();
            unitAttacking.ToggleWaitingToAttack(false);

            // return if unit has already attacked this turn,
            if (unitAttacking.AttackedThisTurn())
            {
                UnitHasAlreadyAttacked();
                return false;
            }

            // if unit instructed to miss
            if (unitAttacking.ShouldMissTurn())
            {
                UnitCannotMove();
                return false;
            }

            // if unit does not have enough action points
            if (unitAttacking.GetActionPoints() < actionPoints)
            {
                NotEnoughActionPoints();
                return false;
            }

            //highlight tiles in range anyway
            moveSelected = true;
            unitAttacking.ToggleWaitingToAttack(true);

            HighlightTilesInExtendedRange(unitAttacking, attackRange);
            return true;
        }
        
        /*
        * Invokes method to attack an enemy unit
         */
        protected void DefendAllyUnit(Unit unitActing, Unit unitToDefend, int defenceAmount)
        {
            //Toggle that unit has attacked, only need to do once, but no harm doing multiple times
            unitActing.ToggleAttackedThisTurn(true);

            unitToDefend.BoostDefence(defenceAmount);

            //update status bar
            GameUI.instance.UpdateStatusBar("Restoring shields of " + unitToDefend.GetUnitName() + " by " +
                                            defenceAmount);
        }
        
        /*
         * Invoked at the start of selection in case any old units are selected
         */
        private void ResetSelection()
        {
            moveSelected = false;

            foreach (Unit u in SoloPlayerController.me.units)
                u.ToggleSelect(false);
        }

        /*
        * Updates status bar
        */
        private void UnitHasAlreadyAttacked() =>
            GameUI.instance.UpdateStatusBar("Unit has already attacked this turn...");

        /*
        * Updates status bar
        */
        private void UnitCannotMove() => GameUI.instance.UpdateStatusBar("Unit has had it's turn disabled...");

        /*
        * Updates status bar
        */
        protected void NoUnitsInRange() => GameUI.instance.UpdateStatusBar("No valid units in range...");

        /*
        * Updates status bar
        */
        private void NotEnoughActionPoints() =>
            GameUI.instance.UpdateStatusBar("Not enough action points to use action...");


        private void HighlightTilesInExtendedRange(Unit unit, int attackRange)
        {
            //Deselect any previous tiles
            ResetAllTiles();

            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(unit.gameObject.transform.position, Vector3.down, out hit, 1))
            {
                tile = hit.collider.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.MarkAttack();
                    tile.FindNeighboursInExtendedRange(attackRange);
                }
            }
        }

        /*
         * Resets all tiles
         */
        protected void ResetAllTiles()
        {
            foreach (var tile in tiles)
                tile.GetComponent<Tile>().Reset();
        }

        /*
        * Use expression body to return read only values pertaining to information about a unit ability 
        */
        public string Name() => abilityName;
        public string[] Information() => information;
    }
}