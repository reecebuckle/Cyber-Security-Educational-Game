using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameField = null;
    private RoomOptions options = new RoomOptions { MaxPlayers = 4 };

    /*
    * Called when the create lobby button is intiated
    */
    public void InitiateCreateLobby()
    {
        //prevent user from creating a room if not connected
        if (!PhotonNetwork.IsConnected) {
             Debug.Log("Not connected to photon network", this);
             return;
        }
            
        //can use CreateRoom or JoinOrCreateRoom, if you use the former and it already exists, 
        //it'll fail whereas if you use the latter and alreaxy exists, you'll join instead of creating
        //Expecting a typed lobby, no need to define any specific users

        if (!string.IsNullOrEmpty(roomNameField.text)) 
            PhotonNetwork.JoinOrCreateRoom(roomNameField.text, options, TypedLobby.Default);
    
        else
            Debug.Log("Room name field is null, put a check pop up here! ", this);
    }

    /*
    * Invoked when a room is successfully created
    */
    public override void OnCreatedRoom()
    {
        Debug.Log("Successful creation of room: " + PhotonNetwork.CurrentRoom.Name, this);
        
    }

    /*
    * Invoked when a room is failed to create, handle errors here
    */
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failure when creeating room: " + message, this);
    }
}
