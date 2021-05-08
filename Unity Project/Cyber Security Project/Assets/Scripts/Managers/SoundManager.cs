using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Audio Manager")]
        public AudioClip damageHit;
        public AudioClip disableHit;
        public AudioClip shieldBuff;
        public AudioClip selectUnit;
        public AudioClip deathSound;
        public AudioClip nextTurn;
        public AudioClip selectTile;
        public AudioClip uiClick;
        public AudioSource soundManager;
        
        //Create a singleton instance of sound manager
        public static SoundManager instance;

        /*
        * Assign singleton Instance
        */
        private void Awake() => instance = this;
        

        public void PlayShieldBuff()
        {
            soundManager.clip = shieldBuff;
            soundManager.volume = 1f;
            soundManager.Play();
        }
    
        public void PlayDamageHit()
        {
            soundManager.clip = damageHit;
            soundManager.volume = 5f;
            soundManager.Play();
        }
        
        public void PlayDisableHit()
        {
            soundManager.clip = disableHit;
            soundManager.volume = 8f;
            soundManager.Play();
        }

        public void PlaySelectUnit()
        {
            soundManager.clip = selectUnit;
            soundManager.volume = 0.3f;
            soundManager.Play();
        }
        
        public void PlayDeathSound()
        {
            soundManager.clip = deathSound;
            soundManager.volume = 1f;
            soundManager.Play();
        }
        
        public void PlayNextTurn()
        {
            soundManager.clip = nextTurn;
            soundManager.volume = 0.8f;
            soundManager.Play();
        }
        
        public void PlaySelectTile()
        {
            soundManager.clip = selectTile;
            soundManager.volume = 1f;
            soundManager.Play();
        }

        public void PlayUIClick()
        {
            soundManager.clip = uiClick;
            soundManager.volume = 1f;
            soundManager.Play();
        }
    
    }
}
