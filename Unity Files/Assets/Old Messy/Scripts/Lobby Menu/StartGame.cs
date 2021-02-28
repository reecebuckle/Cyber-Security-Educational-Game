using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartGame : MonoBehaviour
{
    /*
    * Invoked when game is started
    *
    * TODO: check that this is only interactable when 2-4 players have joined
    *
    */
    public void StartGameButton() => PhotonNetwork.LoadLevel(1);
    
}
