using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        PlayMusic();
        Managers.EventManager.Instance.OnLevelRestart += OnLevelRestart;
    }

    private void PlayMusic()
    {
        Managers.AudioManager.Instance.PlayMusic(true);
    }

    private void OnLevelRestart(bool args)
    {
        PlayMusic();
    }
}
