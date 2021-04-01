using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    private MouseManager mm;
    [SerializeField] private Unit unit; //TODO leave for now, probably not necessary
    [SerializeField] private GameObject quad;

    /*
     * Initialised on frame one
     */
    private void Start()
    {
        mm = GameObject.FindObjectOfType<MouseManager>();
        quad.SetActive(false);
    }

    /*
     * Called once per frame, shows a selection box if hovered over
     */
    private void Update()
    {
        /*if (mm.hoveredUnit == unit)
            quad.SetActive(true);
        else
            quad.SetActive(false);*/
    }
    
    /*
     * Toggles when a move is selected
     */
    private void ToggleUnitInRange(bool inRange)
    {
        if (inRange)
            quad.SetActive(true);
        else
            quad.SetActive(false);
    }   
}