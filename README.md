# Afterlife

> A 3D first-person puzzle platformer where every traversal ability costs energy — so the puzzle
> is never just "can you reach it", but "can you afford to".

![gameplay gif](gameplay_afterlife.gif)

[▶ Play on itch.io](https://schokolasbotter.itch.io/afterlife)

---

## Overview

**Platform:** PC | **Engine:** Unity / C# | **Role:** Sole programmer (team of 3, completed as 2)
**Module:** Games Project 2 — 2nd year, BSc Games Programming, Goldsmiths University of London

The player explores an environment, lighting totems and unlocking the way forward while
progressively gaining three traversal abilities — **launch** (vertical boost), **dash**
(horizontal boost) and **glide**. Each is granted by a specific interactable in the world, and
each is required for puzzles that come after it.

What ties it together is that two of the three abilities **spend a shared energy pool**. Launch
costs 40, dash costs 30, out of 100. Energy trickles back on its own and is topped up by feathers
scattered through the level. So a puzzle is rarely a question of whether you have the right
ability — it's whether you have enough left to use it when you get there.

Originally a team of three, completed by two. I wrote all of the gameplay code.

---

## Technical Contributions

- Built the **complete player controller** — `CharacterController`-based movement, gravity,
  ground detection, and all three traversal abilities
- Designed the **energy economy** that gates ability use, with per-ability costs, passive
  regeneration, and collectible restoration capped at maximum
- Implemented **ability-gated progression** — abilities are locked until the world grants them,
  and world objects declare which ability they unlock
- Wrote an **omnidirectional interaction system** using `Physics.OverlapSphere`, so interactables
  respond from any approach angle rather than requiring the player to face them
- Used **ScriptableObjects** to author the game's narrative and tutorial text as assets,
  separating writing from code
- Built the puzzle chain, checkpoint/respawn system, minimap, and a centralised audio manager

---

## Traversal and the Energy Economy

The three abilities are gated by boolean state on the player, all starting false:

```csharp
public bool hasGliding = false;
public bool hasLaunch  = false;
public bool hasDash    = false;
```

Each is flipped on by interacting with the object in the world that grants it. Every ability
check is a three-part condition — the input, the budget, and the unlock:

```csharp
if (playerInputActions.Player.launch.triggered && (energy - launchCost) >= 0 && hasLaunch)
{
    // ... apply launch, then:
    energy -= launchCost;
}
```

| Ability | Cost | Notes |
|---|---|---|
| **Launch** — vertical boost | 40 | The expensive one; roughly two per full bar |
| **Dash** — horizontal boost | 30 | Cheaper, so chaining is viable |
| **Glide** | free | Divides gravity while falling; ends on landing |

Glide being free is deliberate — it's the recovery ability. Running dry mid-traversal should
leave you able to control a descent rather than simply drop.

Energy regenerates slowly on its own, and **feather pickups restore 40**, clamped so the player
can never bank more than 100. Placing feathers is therefore level design, not decoration: a
feather immediately before a gap converts an impossible jump into a possible one, and a feather
placed after it does not.

---

## Interaction Detection

The interaction system needed to feel natural from any approach angle — the player should never
have to line themselves up with an object to use it.

Each frame the player sweeps its surroundings and evaluates everything it finds:

```csharp
Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactableRadius);
foreach (var hitCollider in hitColliders)
{
    if (hitCollider.tag == "Interactable")
    {
        nearInteractable = true;               // show the button prompt
        if (playerInputActions.Player.interact.triggered) { /* ... */ }
    }
}
```

A sphere has no facing, so proximity alone qualifies — walk up to a totem from behind and it
still works. Interactables are then split by **layer**, not by type checks: objects on the
`Totems` layer light up directly, while everything else opens a text panel. Interacting a second
time closes the panel, so a single input both opens and dismisses.

The panel is also where abilities are granted. Each interactable declares what it unlocks with
three booleans, checked at the moment the panel opens:

```csharp
if (hitCollider.GetComponent<textDisplay>().enablesGliding) { hasGliding = true; }
if (hitCollider.GetComponent<textDisplay>().enablesLaunch)  { hasLaunch  = true; }
if (hitCollider.GetComponent<textDisplay>().enablesDash)    { hasDash    = true; }
```

Making an object grant an ability is a matter of ticking a box in the Inspector — no new code,
no registration step.

---

## Narrative as Data

Story and tutorial text are **ScriptableObject assets**, not strings in code:

```csharp
[CreateAssetMenu(fileName = "New Interactable", menuName = "Interactable")]
public class interactable : ScriptableObject
{
    public string title;
    public string description;
    public string power;
    public string note;
}
```

There are nine of these — five story beats and three tutorials — and `textDisplay` reads whichever
one it's given into the UI panel. Writing and editing the game's text never required opening a
script or entering play mode, which mattered on a two-person team where the writing changed far
more often than the code did.

---

## Project Structure

```
Assets/Scripts/
├── Player Scripts/
│   ├── PlayerMovementScript.cs    movement, all three abilities, energy, interaction sweep
│   ├── CheckPointScript.cs        respawn position
│   ├── SoundEffectScripts.cs
│   ├── interactableScript.cs
│   └── CameraScript/MouseLookingScript.cs
├── Interactables/
│   ├── textDisplay.cs             panel display + which ability this object unlocks
│   ├── totemScript.cs             lit/unlit state and its lights
│   ├── altarScript.cs             watches the three totems, opens the endgame
│   └── featherScript.cs           energy pickup
├── Puzzle Scripts/                ButtonScript, PuzzleScript1, PuzzleScript2
├── Text Objects/interactable.cs   the narrative ScriptableObject
├── Sounds/                        AudioManagerScript, SoundScript
├── UI/                            UIScript, MinimapScript
└── Main Menu/MainmenuManager.cs
```

Eighteen scripts. `PlayerMovementScript` is by far the largest and does the most — see below.

---

## Retrospective

The interaction detection system was the weakest part. `OverlapSphere` returns everything in
radius with no notion of which interactable is *most* relevant, so with two objects overlapping
the behaviour depends on collider iteration order. It never mattered in practice because
interactables were deliberately spread out, but it would fall over immediately in a denser
environment. Selecting the closest hit rather than acting on all of them is a small change I
should have made at the time.

`PlayerMovementScript` is also doing far too much — movement, gravity, three abilities, the energy
economy, the interaction sweep, ability granting, checkpoint respawn, endgame triggering and
footstep audio all live in one 267-line file. Splitting the interaction sweep and the energy
system out would have made both testable and left the movement code readable.

There is also a dead stub: `interactableScript.cs` was an early attempt at interaction detection,
superseded by the sweep inside the player controller. It should have been deleted rather than left
in the project with its body commented out.

The energy economy is the part I would keep unchanged. Tying traversal to a spendable resource
made level design and puzzle design the same activity, and gave a small game more depth than three
unlockable abilities would have on their own.

---

## Tech Stack

`Unity` · `C#` · `ScriptableObjects` · `Unity Input System` · `CharacterController` · `3D Platformer`

---

## Credits

- **Programming:** Laurent Klein
- **Design & Art:** Stefan Markovski

Third-party assets: Stylized Water 2, Andtech Star Pack.
