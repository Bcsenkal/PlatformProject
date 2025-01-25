using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RebuildWindow : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private TMP_InputField sizeInput;
    private Button rebuildButton;
    private Transform window;
    private void Awake() 
    {
        window = transform.GetChild(0);
        canvasGroup = GetComponent<CanvasGroup>();
        sizeInput = transform.GetChild(0).GetChild(2).GetComponent<TMP_InputField>();
        rebuildButton = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        rebuildButton.onClick.AddListener(OnRebuildButtonClick);
        canvasGroup.alpha = 0;
        
    }
    private void Start() 
    {
        Managers.EventManager.Instance.OnSettingsButtonClick += Show;
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        Debug.Log("Settings button clicked");
        StartCoroutine(ShowWindow());
    }

    // try to rebuild the grid with the new size if the input is valid
    private void OnRebuildButtonClick()
    {
        
        if(int.TryParse(sizeInput.text, out int size))
        {
            if(size <= 0) return;
            Managers.EventManager.Instance.ONOnRebuildButtonClick(size);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            window.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            StartCoroutine(HideWindow());
        }
    }

    // smoothly show the window
    IEnumerator ShowWindow()
    {
        canvasGroup.DOFade(1, 0.5f);
        window.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.5f);
        canvasGroup.interactable = true;
    }

    // smoothly hide the window
    IEnumerator HideWindow()
    {
        canvasGroup.DOFade(0, 0.5f);
        window.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.5f);
        canvasGroup.interactable = false;
        Managers.EventManager.Instance.ONOnSettingsClosed();
    }
}
