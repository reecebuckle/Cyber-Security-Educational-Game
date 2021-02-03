using UnityEngine;

[CreateAssetMenu(menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string _gameVersion = "0.1";
    public string getGameVersion
    {
        get { return _gameVersion; }
    }

    [SerializeField] private string _nickname = "RB";

    public string getNickname
    {
        get
        {
            // use unity's randon number generator to produce random nicknames
            int value = Random.Range(0, 9999);
            return _nickname + value.ToString();
        }
    }
}