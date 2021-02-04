using UnityEngine;
using TMPro;
using Photon.Realtime;
public class PlayerListing : MonoBehaviour
{
   [SerializeField] private TMP_Text _playerName;
   public Player Player { get; private set; } //only set within this script, but publically accessible

   public void SetPlayerInfo(Player player) {
       Player = player;
       _playerName.text = player.NickName;  
   }
}