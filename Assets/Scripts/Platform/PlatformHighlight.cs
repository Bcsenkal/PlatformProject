using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;

public class PlatformHighlight : MonoBehaviour
{
    private HighlightEffect highlightEffect;
    public void CacheComponents()
    {
        highlightEffect = GetComponent<HighlightEffect>();
    }

    public void Highlight(bool isPerfect)
    {
        if(!isPerfect) return;
        highlightEffect.HitFX();
    }
}
