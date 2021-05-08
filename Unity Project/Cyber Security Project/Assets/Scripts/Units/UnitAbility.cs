using UnityEngine;

namespace Units
{
    public class UnitAbility : MonoBehaviour
    {
        [SerializeField] private string abilityName;
        [SerializeField] private string information;
    
        /*
        * Use expression body to return read only values pertaining to information about a unit ability 
        */
        public string Name() => abilityName;
        public string Information() => information;
    }
}
