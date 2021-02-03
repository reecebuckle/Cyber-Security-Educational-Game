using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class TestConnect : MonoBehaviourPunCallbacks 
{
    private void Start() {

        Debug.Log("connecting to server", this);
        //use server settings 
        PhotonNetwork.NickName = MasterManager.GameSettings.getNickname;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.getGameVersion;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server: " + cause.ToString());
    }

    public override void OnConnectedToMaster() {
        Debug.Log("connected to server", this);
        Debug.Log("Nickname: " + PhotonNetwork.LocalPlayer.NickName, this);

    }
}
