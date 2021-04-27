using UnityEngine;
using TMPro;
using Photon.Realtime;
public class MessageLog : MonoBehaviour
{
    [SerializeField] private TMP_Text content;
    
    /*
    * Used to set the player info (can only be down privately, when a new player listing is instantiated...)
    */
    public void SetMessageContent(string message) {
        content.text = message;
    }
}