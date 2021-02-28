using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class NameInput : MonoBehaviour
{
    [Header("Name Input Field")]
    [SerializeField] private TMP_InputField nicknameField = null;
    [SerializeField] private Button continueButton = null;
    private const string PlayerPrefsNameKey = "PlayerName"; //constant for player nickname

    private void Start() {
        SetUpInputField();
        continueButton.interactable = false;
    }

    private void SetUpInputField()
    {
        //check if name exists
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
            return;

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nicknameField.text = defaultName;

        SetPlayerName(defaultName);
    }

    //TODO, DO IF STATEMENT SETTING DEFAULT NAME
    public void SetPlayerName(string name)
    {
        //enable conintue button if name field is non null
        continueButton.interactable = !string.IsNullOrEmpty(nicknameField.text);
    }
    public void SavePlayerName()
    {
        string playerName = nicknameField.text;

        //ON PUN, clients nickname is equal to playername
        PhotonNetwork.NickName = playerName;
        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
    }
}
