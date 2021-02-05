using UnityEngine;
using TMPro;
using Photon.Realtime;
public class PlayerListing : MonoBehaviour
{
   [SerializeField] private TMP_Text playerName;

   //return the instance of the player listing
   public Player Player { get; private set; } 

    /*
    * Used to set the player info (can only be down privately, when a new player listing is instantiated...)
    */
   public void SetPlayerInfo(Player player) {
       Player = player;
       playerName.text = player.NickName;  
   }
}