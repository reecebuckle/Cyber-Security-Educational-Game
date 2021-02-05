using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Used to initialise set up
*/
public class PlayerInitialise : MonoBehaviour {

    public static PlayerInitialise initialiser;
    public Transform[] spawnPoints; 

    private void OnEnable() {
        if (PlayerInitialise.initialiser == null)
            PlayerInitialise.initialiser = this;
    }

}



