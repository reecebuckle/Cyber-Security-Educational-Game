using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* At least one instance of this has to be placed within one scene (usually loading menu) when the game is compiled and built
*/
public class SingletonReferences : MonoBehaviour
{
    [SerializeField] private MasterManager masterManager;
}
