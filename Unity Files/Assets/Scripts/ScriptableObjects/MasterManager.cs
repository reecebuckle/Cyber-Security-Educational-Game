using UnityEngine;

[CreateAssetMenu(menuName = "Master Manager")]

public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField] private GameSettings _gameSettings;
    public static GameSettings GameSettings { get { return Instance._gameSettings; } }
   
    
}
