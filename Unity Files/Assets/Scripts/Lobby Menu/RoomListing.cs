using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
   [SerializeField] private TMP_Text _roomname;

   public RoomInfo getRoomInfo { get; private set; } //to get a reference to the room info

   public void SetRoomInfo(RoomInfo info) {
        getRoomInfo = info;
       _roomname.text = info.MaxPlayers + ", " + info.Name; 
   }

   public void JoinRoomButton() {
       PhotonNetwork.JoinRoom(getRoomInfo.Name); 
   }
}
