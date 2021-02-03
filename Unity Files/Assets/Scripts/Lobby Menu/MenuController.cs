using UnityEngine;

public class MenuController : MonoBehaviour
{
   public GameObject lobbyMatchmakingCanvas;
   public GameObject lobbyWaitingCanvas;

   void Awake() {
       lobbyMatchmakingCanvas.SetActive(true);
       lobbyWaitingCanvas.SetActive(false);
   }
   public void OpenWaitingCanvas() {
       lobbyMatchmakingCanvas.SetActive(false);
       lobbyWaitingCanvas.SetActive(true);
   }

   public void OpenMatchmakingCanvas() {
       lobbyMatchmakingCanvas.SetActive(true);
       lobbyWaitingCanvas.SetActive(false);
   }

}
