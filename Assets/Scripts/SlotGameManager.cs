using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Core controller for the slot machine logic, economy, and RNG.
/// </summary>
public class SlotGameManager : MonoBehaviour
{
    [Header("Game Economy")]
    public int startingGold = 1000;
    private int currentGold;
    private int currentBet;

    [Header("Slot Data")]
    public List<SlotSymbol> availableSymbols;
    public SlotReel[] reels;

    [Header("UI References")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI statusText;
    public Button[] betButtons;
    public SlotHandle slotHandle;
    public CoinAnimator coinAnimator;

    private bool isSpinning = false;
    private bool hasBet = false;
    private int reelsStoppedCount = 0;

    void Start()
    {
        currentGold = startingGold;
        UpdateUI();
        slotHandle.Initialize(this);

        foreach (var reel in reels)
        {
            reel.Initialize(availableSymbols);
        }
    }

    /// <summary>
    /// Called by the Bet UI Buttons. Deducts gold with animation, then enables the handle.
    /// </summary>
    public void PlaceBet(int betAmount)
    {
        if (isSpinning || hasBet || currentGold < betAmount) return;

        currentBet = betAmount;
        int previousGold = currentGold;
        currentGold -= currentBet;
        hasBet = true;
        ToggleBetButtons(false);

        // Animate the coin deduction, then enable the handle
        coinAnimator.PlayDeduction(goldText, previousGold, currentGold, () =>
        {
            slotHandle.SetInteractable(true);
            statusText.text = "Pull the handle!";
        });
    }

    /// <summary>
    /// Called by SlotHandle when the player pulls the lever.
    /// </summary>
    public void OnHandlePulled()
    {
        if (isSpinning || !hasBet) return;

        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        hasBet = false;
        reelsStoppedCount = 0;
        statusText.text = "Spinning...";

        // Start all reels
        foreach (var reel in reels)
        {
            reel.StartSpinning();
        }

        // Determine the mathematical outcome BEFORE reels stop
        SlotSymbol[] spinResults = new SlotSymbol[reels.Length];
        for (int i = 0; i < reels.Length; i++)
        {
            int randomIndex = Random.Range(0, availableSymbols.Count);
            spinResults[i] = availableSymbols[randomIndex];
        }

        // Stagger the stopping of the reels for dramatic effect
        for (int i = 0; i < reels.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            reels[i].StopSpinning(spinResults[i], OnReelStopped);
        }
    }

    private void OnReelStopped()
    {
        reelsStoppedCount++;
        if (reelsStoppedCount >= reels.Length)
        {
            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        SlotSymbol r1 = reels[0].GetLandedSymbol();
        SlotSymbol r2 = reels[1].GetLandedSymbol();
        SlotSymbol r3 = reels[2].GetLandedSymbol();

        bool isWin = false;
        int payout = 0;

        if (SymbolsMatch(r1, r2) && SymbolsMatch(r2, r3))
        {
            isWin = true;
            SlotSymbol winningSymbol = r1.isWildcard ? r2 : r1;
            payout = currentBet * winningSymbol.payoutMultiplier;
        }

        if (isWin)
        {
            int previousGold = currentGold;
            currentGold += payout;
            statusText.text = $"JACKPOT! Won {payout}G";

            // Animate coins flying in and gold counting up, then re-enable buttons
            coinAnimator.PlayWin(goldText, previousGold, currentGold, () =>
            {
                isSpinning = false;
                ToggleBetButtons(true);
            });
        }
        else
        {
            statusText.text = "Try Again!";
            isSpinning = false;
            UpdateUI();
            ToggleBetButtons(true);
        }
    }

    private bool SymbolsMatch(SlotSymbol a, SlotSymbol b)
    {
        return a.symbolID == b.symbolID || a.isWildcard || b.isWildcard;
    }

    private void UpdateUI()
    {
        goldText.text = $"{currentGold}G";
    }

    private void ToggleBetButtons(bool state)
    {
        foreach (var btn in betButtons)
        {
            btn.interactable = state;
        }
    }   
}