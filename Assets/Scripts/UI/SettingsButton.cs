using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class SettingsButton : MonoBehaviour
{
    private Button settingsButton;
    void Start()
    {
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(OpenSettings);
        EventManager.Instance.OnSettingsClosed += EnableButton;
    }

    
    void OpenSettings()
    {
        EventManager.Instance.ONOnSettingsButtonClick();
        settingsButton.interactable = false;
    }

    void EnableButton()
    {
        settingsButton.interactable = true;
    }
}
