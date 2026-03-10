# Afterlife
> A 3D first-person puzzle platformer with physics-based environmental interactions and a modular, designer-friendly interaction framework.

![gameplay gif](gameplay_afterlife.gif)

[▶ Play on itch.io](https://schokolasbotter.itch.io/afterlife) | [📁 View Source](https://github.com/Schokolasbotter/Afterlife)

---

## Overview

**Platform:** PC | **Engine:** Unity / C# | **Role:** Lead Programmer (Team of 2)

Afterlife is a 3D open-world puzzle platformer built as a collaborative university project. The player explores an environment, activating switches and unlocking gates while progressively gaining three traversal abilities — a vertical boost, horizontal boost, and glide — each required to solve increasingly complex puzzles. The game culminates in a final puzzle demanding mastery of all three abilities simultaneously.

Originally a team of three, the project was completed by two members — significantly increasing scope and responsibility per person.

---

## Technical Contributions

- Engineered the **complete player controller** including physics-based movement and ability system
- Architected a **modular, data-driven interaction framework** using Scriptable Objects
- Built **three distinct traversal abilities** (vertical boost, horizontal boost, glide) with ability-gated progression
- Implemented an **interaction detection system** supporting context-sensitive activation from any angle
- Designed puzzle logic with **Scriptable Object configuration** for rapid designer iteration
- Managed all gameplay systems end-to-end as sole programmer on the project

---

## Technical Deep Dive: The Interaction Framework

The most significant engineering challenge was building an interaction system flexible enough to handle every puzzle element in the game — buttons, gates, ability unlocks — without requiring code changes for each new interactable.

The solution was a **Scriptable Object-driven interaction framework**. Each interactable object in the scene is configured via a Scriptable Object asset rather than hardcoded values. Designers can create new interactable types entirely from the Unity Editor — defining behaviour, linked objects, and progression flags without touching code.

Key design decisions:
- A simple boolean flag on each Scriptable Object determines whether triggering the interactable grants a new player ability
- Interactables and their targets (gates, platforms, triggers) are linked via serialised references, not scene hierarchy dependencies
- New puzzle elements can be added as pure data — no new MonoBehaviours required

This made the system genuinely **plug and play**: drag a Scriptable Object onto an interactable, configure it in the Inspector, done.

---

## Technical Deep Dive: Interaction Detection

The interaction detection system needed to feel natural from any approach angle — the player should never need to position themselves precisely in front of an object to interact with it.

This required a detection approach that accounts for the player's position relative to the interactable in 3D space, handling overlapping colliders and prioritising the most relevant interactable when multiple are in range. The main pain point was ensuring reliable detection regardless of the player's facing direction or approach vector — a problem that required careful tuning of detection radii and priority logic.

---

## Architecture Overview

```
PlayerController
    ├── MovementSystem
    ├── AbilitySystem
    │       ├── VerticalBoost
    │       ├── HorizontalBoost  
    │       └── Glide
    └── InteractionDetector
            └── InteractableObject
                    └── [ScriptableObject Config]
                            ├── LinkedTarget
                            ├── AbilityUnlock (bool)
                            └── InteractionBehaviour
```

Puzzle logic is driven entirely by the Scriptable Object layer — level designers configure puzzle chains without code changes.

---

## Retrospective

The Scriptable Object interaction framework was the right architectural decision and held up well throughout development — adding new puzzle elements was genuinely fast once the system was in place.

Looking back, the interaction detection system was the weakest point. The logic for determining which interactable to prioritise when multiple overlap was more fragile than it needed to be. The OverlapSphere approach worked well within the constraints of this project — interactables were intentionally spread apart so overlap was never an issue in practice. If this system needed to scale to a denser environment, I'd add closest-target selection to handle priority between overlapping objects.

Completing this project as effectively a two-person team under deadline taught me more about scoping and consistent delivery than any single technical problem did.

---

## Tech Stack

`Unity` · `C#` · `Scriptable Objects` · `Physics-Based Movement` · `3D Platformer`

---

## Credits

- **Programming:** Laurent Klein
- **Design & Art:** Stefan Markovski
