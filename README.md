Here's the improved `README.md` file, incorporating the new content while maintaining the existing structure and information:


# 🎰 Slot Machine Game

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?style=flat&logo=unity)
![Language](https://img.shields.io/badge/Language-C%23-blue)
![Architecture](https://img.shields.io/badge/Architecture-Component%20Based-orange)
![Animation](https://img.shields.io/badge/Animation-DOTween-green)
![Status](https://img.shields.io/badge/Status-Complete-success)
    
**Slot Machine Game** is a fully animated 2D slot machine built in Unity with a classic casino feel. The game features a realistic bet-and-pull flow, animated spinning reels with DOTween-powered mechanical bounce, a two-frame lever handle, and polished coin fly animations for both deductions and jackpot payouts.

## 🎮 Game Overview

A classic 3-reel slot machine game where the player starts with **1000 Gold** and places bets to spin the reels. If all three reels land on matching symbols, the player wins a payout based on the symbol's multiplier. The game features a complete gameplay loop:

1. **Place a Bet** — Choose a bet amount from the available buttons. Gold is deducted with an animated coin-fly effect.
2. **Pull the Handle** — Click the slot machine lever. The handle visually switches between idle and pulled frames.
3. **Watch the Reels Spin** — All three reels spin simultaneously with blur effects (top and bottom), then stop one-by-one with a satisfying mechanical bounce.
4. **Win or Try Again** — If all three symbols match (with wildcard support), coins fly from the machine into the gold counter with a counting animation. Otherwise, try again!

### Key Mechanics

* **Gold Economy**: Start with 1000G. Bets are deducted before spinning, and payouts are added on wins.
* **3-Reel Slot Machine**: Each reel independently spins and stops with staggered timing for dramatic effect.
* **Symbol Matching**: All three reels must match to win. Wildcard symbols substitute for any other symbol.
* **Payout Multipliers**: Each symbol has a unique payout multiplier applied to the bet amount.
* **ScriptableObject Symbols**: Designers can create and balance new symbols without touching code via `SlotSymbol` ScriptableObjects.

## 🕹️ How to Play

| Action | Input |
| :--- | :--- |
| **Place Bet** | Click a Bet Button (e.g., 10G, 50G, 100G) |
| **Pull Handle** | Click the Lever Handle |

### Gameplay Flow


Place Bet → Gold Deducted (coin animation) → Pull Handle → Reels Spin → Reels Stop → Win/Lose


## 🎁 Bonus Features

* **Animated Coin Economy**
    * **Bet Deduction**: Coins pop out of the gold counter, arc upward, and fly into the slot machine — visually representing the money being fed into the machine.
    * **Win Payout**: Coins burst out of the slot machine, arc through the air, and collect into the gold counter — each coin bumps the text on arrival with a final celebration punch.
    * **Counting Animation**: The gold text smoothly counts up/down between values using DOTween, synced with the coin arrivals.

* **Mechanical Handle with Two-Frame Animation**
    * Two separate Image GameObjects (idle and pulled) toggle on click for a clean visual swap with no stretching issues.
    * The handle only becomes interactive after a bet is placed, enforcing the correct gameplay flow.

* **Reel Blur Effect (Top & Bottom)**
    * Gradient fade overlays at the top and bottom edges of each reel create the illusion of motion blur during spinning.
    * Blur images randomize their sprites every loop cycle to enhance the spinning illusion.

* **Wildcard Symbol System**
    * Any symbol can be flagged as a wildcard via ScriptableObject.
    * Wildcards automatically match with any other symbol during win evaluation.

* **Mechanical Bounce on Reel Stop**
    * Reels snap into place using DOTween's `Ease.OutBack` — creating the classic overshooting bounce that real slot machines have.

## 🛠️ Technical Architecture

### Component Architecture

* **`SlotGameManager.cs`** — Core game controller. Manages the gold economy, bet flow, spin routine, win/loss evaluation, and coordinates between the handle, reels, and coin animator.
* **`SlotReel.cs`** — Individual reel controller. Handles the infinite spin loop via `DOAnchorPosY` with `LoopType.Restart`, random blur sprite swapping, and the final snap-to-center animation with `Ease.OutBack` bounce.
* **`SlotSymbol.cs`** — ScriptableObject data container. Stores symbol name, ID, sprite, payout multiplier, and wildcard flag. Created via `[CreateAssetMenu]` for designer-friendly workflow.
* **`SlotHandle.cs`** — Lever controller. Uses two separate GameObjects (idle/pulled images) toggled via `SetActive()` to avoid sprite sizing issues. Coordinates with `SlotGameManager` via `Initialize()` pattern.
* **`CoinAnimator.cs`** — Coin fly animation system. Uses world-to-local position conversion (`RectTransformUtility`) so coins always appear at the correct UI position regardless of anchor/pivot setup. Supports both deduction (gold → machine) and win (machine → gold) arcs.

### Animation Pipeline (DOTween)

| Animation | Technique |
| :--- | :--- |
| Reel Spinning | `DOAnchorPosY` with infinite `LoopType.Restart` |
| Reel Stop Bounce | `DOAnchorPosY` with `Ease.OutBack` |
| Coin Deduction Arc | Two-step `DOAnchorPos` (pop-up → arc midpoint → machine target) |
| Coin Win Arc | Scale pop-in → Two-step `DOAnchorPos` (machine → arc midpoint → gold text) |
| Gold Counter | `DOTween.To` integer tween with `Ease.OutQuad` |
| Text Feedback | `DOShakeAnchorPos`, `DOPunchScale` |
| Handle Pull | `DOVirtual.DelayedCall` for timed frame swap |

## 📁 Project Structure


Assets/
├── Scripts/
│   ├── SlotGameManager.cs      # Core game controller & economy
│   ├── SlotReel.cs             # Individual reel spin & stop logic
│   ├── SlotSymbol.cs           # ScriptableObject symbol data
│   ├── SlotHandle.cs           # Two-frame lever handle controller
│   └── CoinAnimator.cs         # Coin fly animations (deduct & win)
└── Plugins/
    └── Demigiant/
        └── DOTween/            # Tweening animation library


## 🌐 Instructions to Run WebGL Build

1. **Play Online**: Visit the hosted WebGL build link *(if deployed)*.
2. **Build Locally**:
   * Open the project in **Unity 2022.3+**.
   * Go to **File → Build Settings**.
   * Select **WebGL** as the platform and click **Switch Platform**.
   * Click **Build and Run**.
   * Unity will open the build in your default browser.

> **Note**: WebGL builds may take a few minutes. Ensure your browser supports WebGL 2.0.

## 💡 Thought Process & Approach

### Design Philosophy
The goal was to create a slot machine that **feels mechanical and tactile** — not just functional. Every interaction has a visual consequence: betting shows coins leaving, winning shows coins arriving, and the handle physically changes state.

### Outcome-First Spinning
The spin result is determined **mathematically before the reels stop** (RNG runs immediately on spin start). The visual animation is purely theatrical — the reels are told which symbol to land on. This separates game logic from presentation, making it easy to swap in weighted probability tables for a production game.

### ScriptableObject-Driven Design
All symbol data (sprites, multipliers, wildcards) lives in `SlotSymbol` ScriptableObjects. This means designers can create new symbols, adjust payouts, and add wildcards from the Inspector without modifying any C# code.

### Two-Phase Bet Flow
Instead of "click bet → instant spin," the flow is deliberately split: **Bet → Handle Pull → Spin**. This adds anticipation and mirrors real slot machines where you insert coins first, then pull the lever.

### Position-Agnostic Coin Animation
The `CoinAnimator` uses `RectTransformUtility.WorldToScreenPoint` + `ScreenPointToLocalPointInRectangle` to convert positions — meaning it works correctly regardless of how UI elements are anchored, pivoted, or parented. This avoids the common "coins fly to the wrong place" bug.

## 📦 Dependencies

| Package | Purpose |
| :--- | :--- |
| DOTween (Free) | All UI animations — spin, bounce, coin fly, counter, shake, punch |
| TextMeshPro | Gold and status text rendering |

## 📄 License

This project is for educational purposes.

---

*Developed by Sagnik Dasgupta*


This revised README maintains the original structure while enhancing clarity and coherence, ensuring that all new content is seamlessly integrated.
