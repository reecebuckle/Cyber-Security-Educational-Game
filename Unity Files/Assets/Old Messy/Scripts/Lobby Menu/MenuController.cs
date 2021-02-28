
using UnityEngine;

public class MenuController : MonoBehaviour {
    [Header("Menu Canvases")]
    public  GameObject lobbyMatchmakingCanvas;
    public  GameObject lobbyHostingCanvas;
    public  GameObject lobbyJoiningCanvas;

    /*
    * Invoked when returning to main menu
    */
    void Awake() => ReturnToMainMenu();

    /*
    * Invoked when returning to main menu
    */
    public void ReturnToMainMenu()
    {
        lobbyMatchmakingCanvas.SetActive(true);
        lobbyHostingCanvas.SetActive(false);
        lobbyJoiningCanvas.SetActive(false);
    }

    /*
    * Invoked when opening the hosting screen
    */
    public  void OpenHostCanvas()
    {
        lobbyMatchmakingCanvas.SetActive(false);
        lobbyHostingCanvas.SetActive(true);
        lobbyJoiningCanvas.SetActive(false);
    }

    /*
    * Invoked when opening the joining screen
    */
    public  void OpenJoinCanvas()
    {
        lobbyMatchmakingCanvas.SetActive(false);
        lobbyHostingCanvas.SetActive(false);
        lobbyJoiningCanvas.SetActive(true);
    }

}
