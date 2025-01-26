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
        [SerializeField] private AudioClip musicClip;
        [SerializeField] private AudioClip winMusic;
        [SerializeField] private AudioClip failMusic;
        [SerializeField] private AudioClip platformPopSfx;

#region Music

        public void PlayMusic(bool isPlaying)
        {
            if (isPlaying)
            {
                musicSource.clip = musicClip;
                musicSource.loop = true;
                musicSource.Play();
            }
            else
            {
                musicSource.Stop();
                musicSource.loop = false;
            }
        }

        public void PlayEndMusic(bool isSuccess)
        {
            musicSource.Stop();
            musicSource.loop = false;
            musicSource.PlayOneShot(isSuccess ? winMusic :failMusic);
        }

        
#endregion
#region SFX

        public void PlayButtonClick()
        {
            sfxSource.PlayOneShot(buttonClickSfx);
        }

        public void PlayCollectSfx()
        {
            sfxSource.PlayOneShot(collectSfx);
        }

        public void PlayPlatformPopSfx(int comboAmount)
        {
            extraSfxSource.pitch = 1 + comboAmount * 0.2f;
            extraSfxSource.PlayOneShot(platformPopSfx);
        }

        

#endregion
    }
}
