using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class SlotReel : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform reelContainer;
    public Image finalSymbolImage;
    public Image blurSymbolImageTop;
    public Image blurSymbolImageBottom;

    [Header("Animation Settings")]
    [Tooltip("Make this match the Height of your Symbol Images (e.g., 150)")]
    public float symbolHeight = 90f; 
    public float spinSpeed = 0.1f;
    public float snapDuration = 0.5f;
    public Ease snapEase = Ease.OutBack;

    private SlotSymbol landedSymbol;
    private Tween spinTween;
    private List<SlotSymbol> allSymbolsReference;

    public void Initialize(List<SlotSymbol> symbols)
    {
        allSymbolsReference = symbols;
    }

    public void StartSpinning()
    {
        blurSymbolImageTop.sprite = GetRandomSymbolSprite();
        blurSymbolImageBottom.sprite = GetRandomSymbolSprite();

        // Start exactly one symbol height above
        reelContainer.anchoredPosition = new Vector2(reelContainer.anchoredPosition.x, symbolHeight);

        // Drop exactly one symbol height below, then repeat
        spinTween = reelContainer.DOAnchorPosY(-symbolHeight, spinSpeed)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear)
            .OnStepComplete(() =>
            {
                blurSymbolImageTop.sprite = GetRandomSymbolSprite();
                blurSymbolImageBottom.sprite = GetRandomSymbolSprite();
            });
    }

    public void StopSpinning(SlotSymbol targetSymbol, System.Action onComplete)
    {
        landedSymbol = targetSymbol;
        spinTween.Kill();

        finalSymbolImage.sprite = targetSymbol.symbolSprite;

        // Position it just above view for the final drop
        reelContainer.anchoredPosition = new Vector2(reelContainer.anchoredPosition.x, symbolHeight * 1.5f);

        // Snap into center
        reelContainer.DOAnchorPosY(0, snapDuration)
            .SetEase(snapEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    public SlotSymbol GetLandedSymbol()
    {
        return landedSymbol;
    }

    private Sprite GetRandomSymbolSprite()
    {
        return allSymbolsReference[Random.Range(0, allSymbolsReference.Count)].symbolSprite;
    }
}