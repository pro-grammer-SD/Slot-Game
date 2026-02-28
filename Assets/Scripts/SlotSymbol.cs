using UnityEngine;

/// <summary>
/// Data container for slot machine symbols. 
/// </summary>
[CreateAssetMenu(fileName = "NewSlotSymbol", menuName = "Slot Game/Symbol")]
public class SlotSymbol : ScriptableObject
{
    [Header("Symbol Data")]
    public string symbolName;
    public int symbolID; // Unique identifier 
    public Sprite symbolSprite;

    [Header("Payout Rules")]
    public int payoutMultiplier; // How much the bet is multiplied by if this wins
    public bool isWildcard;      // Bonus Feature: Can this substitute for other symbols?
}