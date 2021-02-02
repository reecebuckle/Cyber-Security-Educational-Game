using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class TestConnect : MonoBehaviourPunCallbacks 
{
    private void Start() {

        Debug.Log("connecting to server");
        //use server settings 
        PhotonNetwork.NickName = "RB";
        PhotonNetwork.GameVersion = "0.1";

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server: " + cause.ToString());
    }

    public override void OnConnectedToMaster() {
        Debug.Log("connected to server");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);

    }
}
