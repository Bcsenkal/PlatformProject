using UnityEngine;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource extraSfxSource;
        
        [Space(10)]
        [Header("Audio Clips")] 
        [SerializeField] private AudioClip collectSfx;
        [SerializeField] private AudioClip buttonClickSfx;

#region SFX

        public void PlayButtonClick()
        {
            sfxSource.PlayOneShot(buttonClickSfx);
        }

        public void PlayCollectSfx()
        {
            sfxSource.PlayOneShot(collectSfx);
        }

        

#endregion
    }
}
