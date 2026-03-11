<div align="center">

```
███████╗██╗      ██████╗ ████████╗    ███╗   ███╗ █████╗  ██████╗██╗  ██╗██╗███╗   ██╗███████╗
██╔════╝██║     ██╔═══██╗╚══██╔══╝    ████╗ ████║██╔══██╗██╔════╝██║  ██║██║████╗  ██║██╔════╝
███████╗██║     ██║   ██║   ██║       ██╔████╔██║███████║██║     ███████║██║██╔██╗ ██║█████╗  
╚════██║██║     ██║   ██║   ██║       ██║╚██╔╝██║██╔══██║██║     ██╔══██║██║██║╚██╗██║██╔══╝  
███████║███████╗╚██████╔╝   ██║       ██║ ╚═╝ ██║██║  ██║╚██████╗██║  ██║██║██║ ╚████║███████╗
╚══════╝╚══════╝ ╚═════╝    ╚═╝       ╚═╝     ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝╚══════╝
```

### *Pull the handle. Hold your breath. Feel the rush.*

<br>

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-white?style=for-the-badge&logo=unity&logoColor=black&labelColor=black)](https://unity.com/)
[![Language](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![DOTween](https://img.shields.io/badge/DOTween-Powered-ff6b35?style=for-the-badge&logoColor=white)](http://dotween.demigiant.com/)
[![Architecture](https://img.shields.io/badge/Component--Based-Architecture-6c63ff?style=for-the-badge)](/)
[![Status](https://img.shields.io/badge/Status-Complete%20%E2%9C%93-00c853?style=for-the-badge)](/)

<br>

```
+--------+--------+--------+--------+--------+
| CHERRY |  777   |  BELL  |  GEM   |  STAR  |
+--------+--------+--------+--------+--------+
|  BAR   | CHERRY |  GEM   |  777   |  BAR   |  <- WIN LINE
+--------+--------+--------+--------+--------+
|  $$$   |  STAR  | CHERRY |  BELL  |  $$$   |
+--------+--------+--------+--------+--------+
           * * *  J A C K P O T  * * *
```

</div>

---

<br>

## ✦ &nbsp; What Is This?

> **Slot Machine** is a fully animated, production-quality 2D casino game built from scratch in Unity.
> Every pixel, every bounce, every coin arc was crafted to deliver that visceral, satisfying casino feel —
> without a single trip to Las Vegas.

The game runs on a clean **component-based architecture**, powered by **DOTween** for all its animations.
ScriptableObjects keep symbols designer-friendly, while a tightly scoped game loop keeps the code maintainable and extensible.

**Start with 1000 Gold. Place your bet. Pull the handle. Win big — or try again.**

<br>

---

<br>

## 🎮 &nbsp; Gameplay Loop

<div align="center">

```
+----------------+     +----------------+     +----------------+     +----------------+
|   PLACE BET    |---->|  PULL HANDLE   |---->|  REELS SPIN   |---->|    RESULT      |
|   [Gold Out]   |     |  [Lever Down]  |     | [One by One]  |     |  WIN / LOSE    |
+----------------+     +----------------+     +----------------+     +----------------+
        |                                                                    |
        |                     <---------- Play Again ----------             |
        +--------------------------------------------------------------------+
```

</div>

<br>

### Step 1 — Place Your Bet
Click any bet button. **Gold is instantly deducted** — coins pop from your counter, arc upward, and fly straight into the machine. You *feel* the money leaving your hands.

### Step 2 — Pull the Handle
Click the lever. It physically switches between its **idle and pulled frames**, snapping down with satisfying rigidity. The machine accepts your fate.

### Step 3 — Watch the Reels Spin
All three reels ignite simultaneously, blur streaking past. Then they stop — **one by one** — each landing with a firm mechanical bounce that shakes the whole cabinet. The stagger is deliberate. The tension is real.

### Step 4 — Win or Walk Away
Three matching symbols? **Coins erupt from the machine.** They arc through the air and slam into your gold counter — each one punching the number higher with a satisfying pop. No match? The reels reset. The handle waits.

<br>

---

<br>

## ⚙️ &nbsp; Core Systems

<br>

### 🎰 &nbsp; The Reel Engine
Three independent reels, each spinning and stopping with **staggered timing** for maximum dramatic effect. Blur sprites overlay the reel during spin, then cut away cleanly on landing. A DOTween overshoot tween delivers the mechanical bounce — configurable per symbol weight class.

### 🪙 &nbsp; The Coin Economy
Two distinct animation modes power the gold flow:

| Flow | Direction | Description |
|:---|:---|:---|
| **Bet Deduction** | Counter → Machine | Coins pop out, arc upward, vanish into the slot |
| **Win Payout** | Machine → Counter | Coins burst outward, fly in, slam the counter with a punch animation |

Every coin is an **individual pooled object** — spawned, tweened, and recycled. The counter text counts up live as each coin arrives.

### 🃏 &nbsp; Symbol System
Symbols are defined as **ScriptableObjects** — `SlotSymbol` assets that designers can create, tune, and rebalance without touching a single line of code.

Each symbol exposes:
- **Sprite** — the reel face art
- **Payout Multiplier** — applied to the bet on a match
- **Is Wildcard** — wildcards substitute for any other symbol

Add a new symbol in 30 seconds. No code. No recompile.

### 🃑 &nbsp; Win Detection
After all reels settle, the system reads the **center row** across all three reels, resolves wildcards, and compares values. A match triggers the payout pipeline. The entire check is a single-pass O(1) comparison — fast, clean, provably correct.

<br>

---

<br>

## 🕹️ &nbsp; Controls

<div align="center">

| &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Action &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Input &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
|:---:|:---:|
| **Place Bet** | `Click` a Bet Button — 10G / 50G / 100G |
| **Pull Handle** | `Click` the Lever |

</div>

<br>

---

<br>

## 📐 &nbsp; Architecture At a Glance

```
SlotMachineGame/
├── 🎰  SlotMachineController    ← Master state machine & game flow
├── 🌀  ReelController           ← Spin, stop, bounce logic per reel
├── 🪙  CoinAnimator             ← Spawn, arc, collect coin particles
├── 💰  GoldCounter              ← Balance tracking + animated text punch
├── 🕹️  LeverController          ← Two-frame handle swap + click event
├── 🎨  BlurOverlay              ← Reel spin blur sprite toggling
└── 📦  SlotSymbol (SO)          ← Designer-facing symbol definition
```

The controller is the **only class that owns game state**. Every other system is a dumb, injectable component that receives commands and fires callbacks. Swapping the reel engine, coin animator, or payout formula requires touching exactly one file.

<br>

---

<br>

## 🔧 &nbsp; Tech Stack

<div align="center">

| Layer | Technology |
|:---|:---|
| **Engine** | Unity 2022.3 LTS |
| **Language** | C# 9.0 |
| **Animation** | DOTween Pro |
| **Data** | ScriptableObjects |
| **Architecture** | Component-Based / Event-Driven |
| **Rendering** | Unity 2D (Sprite Renderer) |

</div>

<br>

---

<br>

## 🚀 &nbsp; Getting Started

```bash
# 1. Clone the repository
git clone https://github.com/your-username/slot-machine.git

# 2. Open in Unity Hub
#    Unity 2022.3 LTS or newer required

# 3. Install DOTween
#    Window → Package Manager → Import DOTween

# 4. Open the main scene
#    Assets/Scenes/SlotMachine.unity

# 5. Hit ▶ Play
```

<br>

---

<br>

## 🎨 &nbsp; Adding Custom Symbols

```
1.  Right-click in Project → Create → SlotSymbol
2.  Assign a Sprite
3.  Set your Payout Multiplier
4.  Toggle Is Wildcard if needed
5.  Drag into the Reel's Symbol List in the Inspector
```

That's it. The reel randomizer picks it up automatically. No code changes required.

<br>

---

<br>

<div align="center">

```
+-------------------------------------------+
|                                           |
|    Built with  <3  and way too much       |
|    time perfecting coin arc curves.       |
|                                           |
+-------------------------------------------+
```

**🍒 &nbsp; 🍒 &nbsp; 🍒 &nbsp; — You Win!**

</div>
