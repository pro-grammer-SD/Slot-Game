using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
/// Handles coin fly animations for bet deductions and win payouts.
/// Deduction: coins fly FROM the gold text TO the slot machine (feeding the machine).
/// Win: coins fly FROM the slot machine TO the gold text (collecting winnings).
/// </summary>
public class CoinAnimator : MonoBehaviour
{
    [Header("Coin Prefab")]
    [Tooltip("A small UI Image prefab with your coin sprite + CanvasGroup.")]
    public GameObject coinPrefab;

    [Header("References")]
    [Tooltip("The parent RectTransform to spawn coins under (e.g. a full-screen panel).")]
    public RectTransform coinParent;
    [Tooltip("The RectTransform of the gold text — coins start here on deduction.")]
    public RectTransform goldTextTarget;
    [Tooltip("The RectTransform at the center of the slot machine / reels area — coins fly here on deduction.")]
    public RectTransform slotMachineTarget;
    [Tooltip("The Camera rendering the UI Canvas. Leave null for Screen Space - Overlay.")]
    public Camera uiCamera;

    [Header("Deduction Settings")]
    public int deductCoinCount = 6;
    public float deductFlyDuration = 0.6f;
    public float deductSpread = 40f;

    [Header("Win Settings")]
    public int winCoinCount = 10;
    public float winFlyDuration = 0.5f;
    public float winStaggerDelay = 0.06f;
    public float winSpread = 60f;

    [Header("Gold Counter Animation")]
    public float countDuration = 0.8f;

    /// <summary>
    /// Converts a RectTransform's world position into a local position inside coinParent.
    /// </summary>
    private Vector2 GetLocalPosition(RectTransform target)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(coinParent, screenPoint, uiCamera, out Vector2 localPoint);
        return localPoint;
    }

    /// <summary>
    /// Deduction: coins pop out of the gold text and arc toward the slot machine (feeding coins in).
    /// </summary>
    public void PlayDeduction(TextMeshProUGUI goldText, int fromValue, int toValue, System.Action onComplete = null)
    {
        Vector2 origin = GetLocalPosition(goldTextTarget);
        Vector2 destination = GetLocalPosition(slotMachineTarget);

        // Animate the gold counter down
        AnimateCounter(goldText, fromValue, toValue);

        // Shake the gold text
        goldText.rectTransform.DOComplete();
        goldText.rectTransform.DOShakeAnchorPos(0.4f, new Vector2(10f, 5f), 10, 90, false, true);

        for (int i = 0; i < deductCoinCount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, coinParent);
            RectTransform coinRect = coin.GetComponent<RectTransform>();
            CanvasGroup coinCG = coin.GetComponent<CanvasGroup>();
            if (coinCG == null) coinCG = coin.AddComponent<CanvasGroup>();

            coinRect.anchoredPosition = origin;
            coinRect.localScale = Vector3.one;
            coinCG.alpha = 1f;

            // Each coin lands at a slightly different spot on the machine
            float spreadX = Random.Range(-deductSpread, deductSpread);
            float spreadY = Random.Range(-deductSpread * 0.5f, deductSpread * 0.5f);
            Vector2 finalTarget = destination + new Vector2(spreadX, spreadY);

            // Arc control point — midpoint shifted upward for a toss arc
            Vector2 midPoint = (origin + finalTarget) * 0.5f + new Vector2(Random.Range(-30f, 30f), 120f);

            float delay = i * 0.06f;

            Sequence seq = DOTween.Sequence();
            seq.SetDelay(delay);

            // Pop up slightly from the gold text
            seq.Append(coinRect.DOAnchorPos(origin + new Vector2(Random.Range(-20f, 20f), 40f), 0.08f).SetEase(Ease.OutQuad));

            // Arc toward the slot machine using two-step path (up arc then down into machine)
            seq.Append(coinRect.DOAnchorPos(midPoint, deductFlyDuration * 0.5f).SetEase(Ease.OutQuad));
            seq.Append(coinRect.DOAnchorPos(finalTarget, deductFlyDuration * 0.5f).SetEase(Ease.InQuad));

            // Spin while flying
            seq.Join(coin.transform.DORotate(new Vector3(0, 0, Random.Range(-360f, 360f)), deductFlyDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear));

            // Shrink and fade as it arrives at the machine
            seq.Insert(delay + 0.08f + deductFlyDuration * 0.6f, coinCG.DOFade(0f, deductFlyDuration * 0.4f).SetEase(Ease.InQuad));
            seq.Insert(delay + 0.08f + deductFlyDuration * 0.6f, coin.transform.DOScale(0.3f, deductFlyDuration * 0.4f).SetEase(Ease.InCubic));

            seq.OnComplete(() => Destroy(coin));
        }

        float totalDuration = 0.08f + deductFlyDuration + (deductCoinCount * 0.06f) + 0.15f;
        DOVirtual.DelayedCall(totalDuration, () =>
        {
            onComplete?.Invoke();
        });
    }

    /// <summary>
    /// Win: coins burst out of the slot machine and fly toward the gold text (collecting winnings).
    /// </summary>
    public void PlayWin(TextMeshProUGUI goldText, int fromValue, int toValue, System.Action onComplete = null)
    {
        Vector2 origin = GetLocalPosition(slotMachineTarget);
        Vector2 destination = GetLocalPosition(goldTextTarget);

        for (int i = 0; i < winCoinCount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, coinParent);
            RectTransform coinRect = coin.GetComponent<RectTransform>();
            CanvasGroup coinCG = coin.GetComponent<CanvasGroup>();
            if (coinCG == null) coinCG = coin.AddComponent<CanvasGroup>();

            // Start at the slot machine with slight spread
            float spreadX = Random.Range(-winSpread, winSpread);
            float spreadY = Random.Range(-winSpread * 0.5f, winSpread * 0.5f);
            Vector2 spawnPos = origin + new Vector2(spreadX, spreadY);
            coinRect.anchoredPosition = spawnPos;
            coinRect.localScale = Vector3.zero;
            coinCG.alpha = 1f;

            // Arc control point — midpoint shifted upward for a celebratory arc
            Vector2 midPoint = (spawnPos + destination) * 0.5f + new Vector2(Random.Range(-50f, 50f), 150f);

            float delay = i * winStaggerDelay;

            Sequence seq = DOTween.Sequence();
            seq.SetDelay(delay);

            // Pop in with a burst scale
            seq.Append(coin.transform.DOScale(1.3f, 0.1f).SetEase(Ease.OutBack));
            seq.Append(coin.transform.DOScale(1f, 0.05f));

            // Arc toward the gold text (up then down into the counter)
            seq.Append(coinRect.DOAnchorPos(midPoint, winFlyDuration * 0.5f).SetEase(Ease.OutQuad));
            seq.Append(coinRect.DOAnchorPos(destination, winFlyDuration * 0.5f).SetEase(Ease.InQuad));

            // Spin while flying (Y axis for a coin-flip look)
            seq.Join(coin.transform.DORotate(new Vector3(0, 360f, Random.Range(-90f, 90f)), winFlyDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear));

            seq.OnComplete(() =>
            {
                // Bump the gold text each time a coin arrives
                goldText.rectTransform.DOComplete();
                goldText.rectTransform.DOPunchScale(Vector3.one * 0.15f, 0.15f, 4, 0.3f);
                Destroy(coin);
            });
        }

        // Start counting up when the first coins begin to arrive
        float counterDelay = 0.15f + (winFlyDuration * 0.7f);
        DOVirtual.DelayedCall(counterDelay, () =>
        {
            AnimateCounter(goldText, fromValue, toValue);
        });

        // Callback after all coins have landed
        float totalDuration = 0.15f + winFlyDuration + (winCoinCount * winStaggerDelay) + 0.3f;
        DOVirtual.DelayedCall(totalDuration, () =>
        {
            goldText.rectTransform.DOComplete();
            goldText.rectTransform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 6, 0.5f);
            onComplete?.Invoke();
        });
    }

    /// <summary>
    /// Smoothly counts the gold text between two values.
    /// </summary>
    private void AnimateCounter(TextMeshProUGUI goldText, int from, int to)
    {
        int displayValue = from;
        DOTween.To(() => displayValue, x =>
        {
            displayValue = x;
            goldText.text = $"{displayValue}G";
        }, to, countDuration).SetEase(Ease.OutQuad);
    }
}
