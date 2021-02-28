using UnityEngine;
using Photon.Pun;

public class LeaveRoom : MonoBehaviour
{
    /*
    * When player leaves a room, signal this to photon network
    * true means you're intentionally leaving
    */
    public void LeaveRoomButton() => PhotonNetwork.LeaveRoom(true);
    
}
