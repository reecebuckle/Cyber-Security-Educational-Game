using System;
using System.Collections.Generic;
using Managers;
using UI;
using Units;
using UnityEngine;


namespace SoloDebugging
{
    /*
    * Solo player controller move used for performance analysis without connecting multiplayer
    */
    public class SoloPlayerController : MonoBehaviour
    {
        public Unit selectedUnit; // currently selected unit
        public static SoloPlayerController me; // local player
        public List<Unit> units = new List<Unit>(); // list of all our units
        public Unit unit1;
        public Unit unit2;
        public Unit unit3;
        
        private void Start()
        {
            me = this;
            units.Add(unit1);
            units.Add(unit2);
            units.Add(unit3);
        }

        private void Update() => WaitToSelectUnit();

        /*
        * Checks if a unit has been selected
        */
        private void WaitToSelectUnit()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (!hit.collider.CompareTag("Unit")) return;

                    Unit clickedUnit = hit.collider.GetComponent<Unit>();

                    SelectUnit(clickedUnit);
                }
            }
        }

        /*
        * Invoked when we select a unit and it belongs to us
        */
        private void SelectUnit(Unit clickedUnit)
        {
         
            // If we click a unit we've already selected, DO nothing
            if (clickedUnit.IsSelected()) return;

            // Unselect the current unit IF one is selected
            if (selectedUnit != null)
                DeselectUnit();
            
            clickedUnit.ToggleSelect(true);
            clickedUnit.GetComponent<SoloController>().FindTiles();
            selectedUnit = clickedUnit;
            SoundManager.instance.PlaySelectUnit();

            // Will display selected unit for us or enemy
            GameUI.instance.ToggleUnitBar(selectedUnit);
            GameUI.instance.DisplayUnitStats(selectedUnit);
            GameUI.instance.UpdateStatusBar("Selecting unit: " + selectedUnit.GetUnitName());
        }

        /*
        * Deselects the currently selected unity
        */
        public void DeselectUnit()
        {
            if (selectedUnit != null)
            {
                //exclude database and web server
                if (selectedUnit.GetUnitID() < 5)
                    selectedUnit.GetComponent<SoloController>().DeselectTiles();

                selectedUnit.ToggleSelect(false);
                selectedUnit.WaitingToAttack = false;
            }

            selectedUnit = null;
        }
    }
}