using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetKits.ParticleImage;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Managers;

public class EndScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Transform endScreenWindow;
    private ParticleImage confetti;
    private Transform topTextTransform;
    private TextMeshProUGUI topText;
    private Button reloadButton;
    private TextMeshProUGUI reloadButtonText;
    private bool isWinScreen;

    void Start()
    {
        CacheComponents();
        EventManager.Instance.OnLevelEnd += ShowScreen;
    }

    private void CacheComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        endScreenWindow = transform.GetChild(0);
        confetti = endScreenWindow.GetChild(1).GetComponent<ParticleImage>();
        topTextTransform = endScreenWindow.GetChild(0);
        topText = topTextTransform.GetComponent<TextMeshProUGUI>();
        reloadButton = endScreenWindow.GetChild(2).GetComponent<Button>();
        reloadButtonText = reloadButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        isWinScreen = false;
        reloadButton.onClick.AddListener(RestartLevel);
    }

    private void ShowScreen(bool isSuccess)
    {
        
        canvasGroup.blocksRaycasts = true;
        reloadButton.interactable = false;
        isWinScreen = isSuccess;
        StartCoroutine(ScreenShowUp(isSuccess));
    }

    private IEnumerator ScreenShowUp(bool isSuccess)
    {
        var delay = isSuccess ? 3f : 1f;
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlayEndMusic(isSuccess);
        canvasGroup.alpha = 1;
        endScreenWindow.DOScale(1, 0.35f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.35f);
        topText.text = isSuccess ? "Amazing!" : "Nice Try!";
        topTextTransform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        if(isSuccess) confetti.Play();
        yield return new WaitForSeconds(0.5f);
        reloadButtonText.text = isSuccess ? "Continue" : "Restart";
        reloadButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.5f);
        canvasGroup.interactable = true;
        reloadButton.interactable = true;
    }
    private IEnumerator CloseScreen()
    {
        canvasGroup.interactable = false;
        endScreenWindow.DOScale(0, 0.35f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.35f);
        topTextTransform.localScale = Vector3.zero;
        reloadButton.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        EventManager.Instance.ONOnLevelRestart(isWinScreen);
    }

    private void RestartLevel()
    {
        AudioManager.Instance.PlayButtonClick();
        StartCoroutine(CloseScreen());
        reloadButton.interactable = false;
    }
}
