using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    private MouseManager mm;
   // private Unit unit; //TODO leave for now, probably not necessary
    [SerializeField] private GameObject quad;

    /*
     * Initialised on frame one
     */
    private void Start()
    {
        mm = GameObject.FindObjectOfType<MouseManager>();
        //unit = transform.parent.GetComponent<Unit>(); //TODO leave for now, probably not necessary
        quad.SetActive(false);
    }

    /*
     * Called once per frame, shows a selection box if hovered over
     */
    private void Update()
    {
        if (mm.hoveredUnit != null)
            quad.SetActive(true);
        else
            quad.SetActive(false);
    }
}