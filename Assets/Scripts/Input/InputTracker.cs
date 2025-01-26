using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTracker : MonoBehaviour, IPointerDownHandler
{
    //Track input from UI image by pointer down handler interface and sends an event according to click
    public void OnPointerDown(PointerEventData eventData)
    {
        Managers.EventManager.Instance.ONOnMouseDown(eventData.position);
    }
}
