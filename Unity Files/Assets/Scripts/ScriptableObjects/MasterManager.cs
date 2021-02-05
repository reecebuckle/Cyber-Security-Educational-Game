using UnityEngine;

[CreateAssetMenu(menuName = "Master Manager")]

public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField] private GameSettings gameSettings;
    
    public static GameSettings GameSettings { get { return Instance.gameSettings; } }
   
   
    
}
