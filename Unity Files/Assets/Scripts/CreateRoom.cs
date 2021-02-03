using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomnameField = null;

    /*
    * Called when the create lobby button is intiated
    */
    public void InitiateCreateLobby()
    {
        
        //prevent user from creating a room if not connected
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        //can use CreateRoom or JoinOrCreateRoom, if you use the former and it already exists, 
        //it'll fail whereas if you use the latter and alreaxy exists, you'll join instead of creating
        //Expecting a typed lobby, no need to define any specific users

        if (!string.IsNullOrEmpty(roomnameField.text))
            PhotonNetwork.JoinOrCreateRoom(roomnameField.text, options, TypedLobby.Default);
        else 
            Debug.Log("Room name field is null, put a check in here! ", this);
    }

    public override void OnCreatedRoom() {
        Debug.Log("Successful creation of room: " + PhotonNetwork.CurrentRoom.Name, this);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Failure when creeating room: " + message, this);
    }
}
