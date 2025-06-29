# RepositoryForTechAward

<!-- ============================================== -->
<!--   🏆 Tech Award Submission: Hero Arena Code   -->
<!-- ============================================== -->

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2022.3.57f1-black?logo=unity" alt="Unity 2022.3.57f1" />
  <img src="https://img.shields.io/badge/Zenject-v10-blue?logo=zenject" alt="Zenject" />
  <img src="https://img.shields.io/badge/UniTask-v2.6-yellow?logo=unity" alt="UniTask" />
  <img src="https://img.shields.io/badge/UniRx-v7.10-purple?logo=reactivex" alt="UniRx" />
  <img src="https://img.shields.io/badge/DOTween-v2.2-orange?logo=dotnet" alt="DOTween" />
  <img src="https://img.shields.io/badge/Nova-MVVM-lightgrey" alt="Nova MVVM" />
</p>

Hey there, if you're reading this, then you must be a tech award judge.
---
> **🚧 DISCLAIMER**  
> This repo is **read-only**—cloning it won’t spin up a playable game. It’s all about showcasing **architecture**, **code style**, and **best practices**.

---
So, if you plan to actually read and understand what’s going on here, here’s a quick overview of our tech choices and why we made them.

A bit about me: I’m the lead developer—you can learn more on my [homepage](https://bit.ly/jey-homepage) or on [LinkedIn](https://www.linkedin.com/in/jeyodilkhujaev/). To get in touch, ping me on [Telegram](https://t.me/bloodyjey).

---
**Project duration**: Two years, driven mostly by me.

**Main Technology Stack**
- Unity 2022.3.57f1
- Zenject
- UniTask & UniRx (migrating to R3 soon)
- DOTween & FEEL
- Odin Inspector
- Easy Save & Unity Services
- Nova (MVVM)
- Git

---


We follow the **Composition Root** principle: every global dependency is bound in our `ProjectInstaller`. The game launches from the **Bootstrap** scene, which loads all core services—Unity’s database, meta-services, etc.—then transitions to the **Main Menu** (installed via Nova + MVVM). Level selection is coming soon; once a level is chosen, the `GameInstaller` sets up game-specific bindings and registers the `GameInitializer`, which brings each subsystem online in a controlled order.

If you peek into `GameInitializer`, you might think it breaks SOLID—but its sole responsibility is startup orchestration. Unity’s unpredictable execution order forced us to implement our own initialization flow for deterministic behavior.




> **TL;DR**  
> `ProjectInstaller` → `GameInstaller` → `GameInitializer` and you can read everything else after

One feature I’m particularly proud of is our **Stat System**, designed to support any RPG stats you can imagine.
It’s still evolving, but you could bootstrap an entire RPG around it in minutes.  