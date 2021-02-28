using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
   [SerializeField] private TMP_Text roomName;
   private GameObject MenuCanvases = null;

   //return the instance of the room listing
   public RoomInfo RoomInfo { get; private set; } 

   //On awake, attempt to find the menu canvases
   void Awake() => MenuCanvases = GameObject.Find("Lobby Menu Canvases");

   /*
   * Used to set the room info (can only be done privately, when a new room listing is instantiated....)
   */
   public void SetRoomInfo(RoomInfo info) {
        RoomInfo = info;
        roomName.text = info.MaxPlayers + ", " + info.Name; 
   }

   /*
   * Invoked when a Lobby button is clicked such that the player has joined
   */
   public void JoinRoomButton() {
      Debug.Log("Joining Room: " + RoomInfo.Name, this);
      PhotonNetwork.JoinRoom(RoomInfo.Name);
      //Hacky way to access the menu controller
      MenuCanvases.GetComponentInChildren<MenuController>().OpenJoinCanvas();
   } 
   
}
