# Terminal Dogma

A 2D speed platformer built in Unity 6 LTS for Intro to Game Design, Project 1.

Run through a night-time sky ruin, collect rings, stomp enemies and avoid spikes, then reach the portal at the end of the level. The game rewards two competing goals: finishing fast and finishing with as many rings as possible. Each run is graded S to D on a results screen, and the best time and highest ring count are saved between play sessions.

---

## How to Play

### Running the build

1. Extract the zip file completely.
2. Open the extracted folder and run `Terminal Dogma.exe`.
3. If Windows shows a "Windows protected your PC" screen, click **More info**, then **Run anyway**. The game is not code-signed, so Windows shows this for any unsigned download.

### Playing WebGL
1. Click link: https://play.unity.com/en/games/c75cdd3b-981c-494b-a8f6-24d22a413a37/terminal-dogma
### Controls

| Action | Keys |
|---|---|
| Move | A / D or Left / Right arrow |
| Jump / Double jump | Space, W or Up arrow |
| Pause / Resume | Escape |

### Rules

- **Rings are your health.** Getting hit while holding rings scatters them across the ground. You have a few seconds to grab some back before they vanish.
- **Getting hit with zero rings restarts the level.** Falling into a pit also restarts it. The timer keeps running through restarts, so a death costs time.
- **Speed builds up the longer you keep moving.** Turning around, stopping or running into a wall drops you back to walking speed.
- **Jump on an enemy's head to defeat it.** A stomp also bounces you upward and gives you back your air jump. Touching an enemy from the side or below counts as a hit.
- **Reach the portal** to finish. The results screen shows your time, rings and grade.

---

## Scenes

The game has two scenes. Both are in the build settings, with `MainMenu` first.

### MainMenu

The title screen.

| GameObject | Description |
|---|---|
| `Canvas` | Holds all menu UI: `TitleLogo` (the game logo image), `StartGameButton`, `QuitGameButton`, `BestTimeText` and `BestRingsText` |
| `Canvas/ControlsCanvas` | Keyboard icons and labels showing the move and jump controls |
| `MainMenuManager` | Has the `MainMenu` script. Loads the saved records into the two text fields and handles the Start and Quit buttons |
| `Background1` | The sky prefab. In this scene the parallax component is removed and `MainMenuBackgroundScroller` slides it slowly to the left in a seamless loop |
| `Main Camera` | Fixed camera |
| `EventSystem` | Routes clicks to the buttons using the Input System UI module |

### World1

The gameplay level.

| GameObject | Description |
|---|---|
| `Player` | The player character. Has a Rigidbody2D, CapsuleCollider2D, `PlayerInput`, `PlayerController`, `PlayerHealth`, an Animator, and a Trail Renderer used as a speed trail |
| `Grid/Tilemap` | The brick level layout. Uses a Tilemap Collider 2D merged by a Composite Collider 2D so the player runs smoothly across tile seams. On the `Ground` layer |
| `Ground`, `Bridge`, `Tree` | Additional Tilemaps for decoration and extra platforms |
| `Rings` | Parent object holding the 213 `Ring` prefab instances placed through the level |
| `Enemies` | Parent object holding the 5 `Enemy` prefab instances |
| `Spikes` | Parent object holding the `Spikes` prefab instances |
| `CrystalLights` | Crystal lamp prefabs, each with a cyan Light 2D, placed to light the path and mark hazards |
| `Signs` | Decorative signposts |
| `Global Light 2D` | Darkens the whole scene to create the night-time look |
| `DroppedRingPool` | `SimplePool` holding 30 pre-made `DroppedRing` objects that are reused whenever the player is hit |
| `KillZone` | A long trigger strip below the level. Touching it restarts the level |
| `LevelGoal` | The portal at the end of the level. Has a trigger collider, the `LevelGoal` script and a cyan spot light |
| `HUDCanvas` | Shows `RingText` and `TimerText` during play, and contains `ResultsPanel`, which appears when the level is finished |
| `HUDCanvas/ResultsPanel` | Shows time, rings and grade, with `RetryButton` and `MenuButton` |
| `PauseManager/PauseMenuCanvas` | The pause screen, with `ResumeButton` and `QuitButton` |
| `Main Camera` | Follows the player using `CameraControl` |
| `Background1` | The sky prefab, using the `Background` parallax script |
| `EventSystem` | Routes clicks to the buttons using the Input System UI module |

---

## Prefabs

| Prefab | Description |
|---|---|
| `Ring` | A collectible ring with a spinning animation, a trigger collider and a small gold light. Uses the `Ring` script |
| `DroppedRing` | The ring that scatters when the player is hit. Has a Rigidbody2D and a bouncy physics material, sits on the `DroppedRing` layer, and uses the `DropRings` script. Created and reused by `DroppedRingPool` |
| `Enemy` | A slime enemy with a Rigidbody2D, Box Collider 2D, `Enemy` and `EnemyHealth`, and a red danger light |
| `Spikes` | A spike hazard with a trigger collider, the `Spike` script and a red danger light |
| `CrystalLight` | A decorative crystal lamp with a cyan Light 2D |
| `Background1` | The night sky, built from several copies of the sky image placed side by side, with every other copy mirrored so the seams match |

---

## Scripts

All scripts are in `Assets/Scripts`.

| Script | Attached to | Summary |
|---|---|---|
| `PlayerController` | Player | Reads Move and Jump from the Input System. Builds speed gradually from `minSpeed` to `maxSpeed` with `Mathf.MoveTowards`, and resets speed when turning, stopping or blocked by a wall. Handles the ground check, double jump, coyote time (a short grace period for jumping after leaving a ledge) and the stomp bounce. Sends speed and grounded state to the Animator, speeds up the run animation as the player accelerates, and turns on the speed trail near top speed |
| `PlayerHealth` | Player | Tracks the ring count and updates the HUD. `TakeHit` scatters the player's rings using the dropped-ring pool, or restarts the level if the player has none. A short invincibility timer stops one hazard from hitting twice in a row |
| `Enemy` | Enemy prefab | Chases the player when within range, stops at ledges so it doesn't walk off platforms, and flips to face its direction of travel. On contact, decides whether the player stomped it (the player's feet are above the enemy's middle) or was hit |
| `EnemyHealth` | Enemy prefab | Holds the enemy's health and removes the enemy when it reaches zero |
| `Ring` | Ring prefab | When the player touches the ring, adds one ring and hides the ring |
| `DropRings` | DroppedRing prefab | Controls a scattered ring: launched with a random velocity, can be collected after a short delay, and returns to the pool when collected or when its lifetime runs out |
| `SimplePool` | DroppedRingPool | Object pool, following the Assignment 3 pattern. Creates a fixed number of objects in `Awake`, hands out inactive ones with `GetFromPool`, and takes them back with `ReturnToPool` |
| `Spike` | Spikes prefab | Calls `TakeHit` on the player on contact |
| `KillZone` | KillZone | Restarts the level when the player falls into it |
| `LevelGoal` | LevelGoal (portal) | Runs the level timer, which persists through restarts. When the player reaches the portal: saves new records, calculates the grade, shows the results screen, and freezes the game. Also handles the Retry and Menu buttons |
| `PauseMenu` | PauseManager and Player | Pauses and resumes the game with `Time.timeScale` and shows or hides the pause canvas. The copy on the Player receives the Escape key from `PlayerInput`, and the copy on PauseManager receives the button clicks. Both share the static `IsPaused` flag and the same canvas |
| `MainMenu` | MainMenuManager | Displays the saved best time and most rings, and handles the Start and Quit buttons |
| `RingsAndTimePlayerPrefs` | Not attached (static class) | Wraps PlayerPrefs to save and load the best time and most rings |
| `CameraControl` | Main Camera (World1) | Moves the camera to follow the player with an offset |
| `Background` | Background1 (World1) | Parallax: moves the sky only part of the distance the camera moves, so it drifts slowly behind the level |
| `MainMenuBackgroundScroller` | Background1 (MainMenu) | Scrolls the sky to the left and jumps it back by exactly one pattern width, creating a seamless loop |

### Grading formula

The grade is the average of two scores, each from 0 to 1:

- **Ring score:** rings at the finish divided by the total rings in the level.
- **Time score:** 1 at `fastTime` (45 seconds) or faster, 0 at `slowTime` (120 seconds) or slower, and in between proportionally, using `Mathf.InverseLerp`.

| Average | Grade |
|---|---|
| 0.90 or higher | S |
| 0.75 | A |
| 0.60 | B |
| 0.40 | C |
| Below 0.40 | D |

---

## Requirement Satisfaction

| # | Requirement | How it is met | Where to look |
|---|---|---|---|
| 1 | Scenes and scene management | Two scenes, `MainMenu` and `World1`. The Start button loads `World1`. The pause menu's Quit button and the results screen's Menu button both load `MainMenu`. Both scenes are in the build settings | `MainMenu.cs` (`OnStartGameClick`), `PauseMenu.cs` (`QuitToMenu`), `LevelGoal.cs` (`OnMenuButtonClick`) |
| 2 | Input System player control | The Player has a `PlayerInput` component using the `BasicMovement` Input Actions asset. Move, Jump and Pause are received as Input System messages. Movement is applied through the Player's Rigidbody2D, with a CapsuleCollider2D for collision. Active Input Handling is set to the Input System package | `Assets/InputSystems/BasicMovement.inputactions`, `PlayerController.cs` (`OnMove`, `OnJump`, `FixedUpdate`) |
| 3 | Tilemap level layout | The level is painted on Tilemaps under `Grid`. The main Tilemap has a Tilemap Collider 2D, merged by a Composite Collider 2D, and is on the `Ground` layer used by the player's and enemies' ground checks | `World1` scene, `Grid/Tilemap` |
| 4 | Prefab pooling | `DroppedRingPool` uses `SimplePool` to create 30 `DroppedRing` prefabs once at startup. When the player is hit, rings are taken from the pool with `GetFromPool` and returned with `ReturnToPool`. No dropped rings are created or destroyed during play | `SimplePool.cs`, `PlayerHealth.cs` (`ScatterRings`), `DropRings.cs` |
| 5 | Tags, Layers and triggers | Several mechanics use `OnTriggerEnter2D` with `CompareTag("Player")`: ring pickup, spikes, the kill zone and the level goal. The enemy stomp uses `OnCollisionEnter2D` with `CompareTag`. The Layer Collision Matrix is configured so the `DroppedRing` layer only collides with `Ground`, which keeps scattered rings from pushing the player, enemies or each other | `Ring.cs`, `Spike.cs`, `KillZone.cs`, `LevelGoal.cs`, `Enemy.cs`, Project Settings > Physics 2D |
| 6 | Pause menu | Pressing Escape shows a Canvas pause menu and sets `Time.timeScale` to 0. Resume (button or Escape) restores it to 1. Quit returns to the main menu and resets the time scale first | `PauseMenu.cs`, `PauseManager/PauseMenuCanvas` |
| 7 | PlayerPrefs persistence | The best time and highest ring count are saved with PlayerPrefs when the player reaches the goal, but only if they beat the old records. The main menu reads and displays them on launch, so the values are still there after quitting and relaunching | `RingsAndTimePlayerPrefs.cs`, `LevelGoal.cs` (`SaveRecords`), `MainMenu.cs` (`Start`) |
| 8 | Lighting / material touch | A Global Light 2D darkens the level for a night setting. Cyan Light 2D lamps light the path, the portal has its own spot light, red lights mark spikes and enemies, and each ring has a small gold light. The speed trail and several sprites use the unlit sprite material so they stay bright in the dark | `World1` scene, `Global Light 2D`, `CrystalLight`, `Spikes`, `Enemy` and `Ring` prefabs |

### Cohesion

The requirements are built into one game loop. Rings are both the score and the player's health, so the trigger-based pickups, the hazards and the pooled scattered rings all feed into the same decision: take risks to move faster, or slow down to protect your rings. The lighting carries meaning: cyan lamps mark the safe path, red lights mark danger, gold lights mark rings, and the glowing portal marks the goal. The timer and ring count feed the grade, and the saved records give the player a reason to replay from the main menu.

---

## Third-Party Assets

| Asset | Used for | Source |
|---|---|---|
| Simple Pixel Art Spikes, by kairosolo | Spike hazard sprite | https://kairosolo.itch.io/simple-pixel-art-spikes |
| Free Sky Ruins 2D Platformer Tileset, by CraftPix | Brick tiles, bridge, tree, signposts, crystal lamp, portal and night sky background | https://craftpix.net/freebies/free-sky-ruins-2d-platformer-tileset/ |
| Free Slime Mobs Pixel Art Top-Down Sprite Pack, by CraftPix | Enemy sprite | https://craftpix.net/freebies/free-slime-mobs-pixel-art-top-down-sprite-pack/ |
| Free 3 Cyberpunk Characters Pixel Art, by CraftPix | Player sprite sheets (idle, run, jump, double jump) | https://files.craftpix.net/2024/06/093f65e080a295f8076b1c5722a46aa2/craftpix-net-856554-free-3-cyberpunk-characters-pixel-art.zip |
| Ring sprites, from the Free Sky Ruins 2D Platformer Tileset by CraftPix | Collectible ring animation. Made from the pack's coin animation frames; I erased the center of each frame in Photopea to turn the coins into rings | https://craftpix.net/freebies/free-sky-ruins-2d-platformer-tileset/ |
| Keyboard key icons (`kb_dark_all.png`), from itch.io | Keyboard key icons on the main menu's controls display | https://vryell.itch.io/controller-keyboard-icons |
| Inter typeface (SIL Open Font License) | Lettering in the title logo | https://rsms.me/inter/ |
| TextMesh Pro, Universal Render Pipeline, Input System | Unity packages | Unity Technologies |

---

## References and Inspiration

These works inspired the game's name and design. No assets, characters, music or code from either series are used; all mechanics were written from scratch for this project.

| Work | What it inspired |
|---|---|
| *Neon Genesis Evangelion* (anime series, 1995) | The title. "Terminal Dogma" is the name of a location in the series |
| *Sonic the Hedgehog* series (Sega, 1991 onward) | **Rings as health:** a hit scatters your rings, and a hit with none ends the attempt. **Scattered rings:** dropped rings bounce away and can be recollected for a few seconds before vanishing. **Momentum:** speed builds the longer you keep moving and is lost when you stop or hit a wall. **Enemy stomp:** jumping on an enemy defeats it and bounces you upward. **End-of-level rank:** the results screen grades each run with a letter based on time and rings |

---

## AI Assistance Disclosure

Claude (Anthropic), an AI assistant, was used during development for the following:

| Area | Files | What the AI contributed |
|---|---|---|
| This README | `README.md` | Drafted the documentation from the project's scripts, scenes and settings, which I then reviewed |
| Scattered rings | `PlayerHealth.cs` (`ScatterRings`), `DropRings.cs` | Generated the logic that launches rings from the pool in random directions, delays pickup, and returns them to the pool after a time limit |
| Background parallax and scrolling | `Background.cs`, `MainMenuBackgroundScroller.cs` | Generated the parallax movement and the seamless looping scroll for the main menu |
| Logic using `Mathf` | `PlayerController.cs`, `LevelGoal.cs` | Helped write and explain the `Mathf.MoveTowards` acceleration, the `Mathf.InverseLerp` / `Mathf.Lerp` mapping that speeds up the run animation, and the `Mathf.InverseLerp` time score in the grading formula |
| Results screen and grading | `LevelGoal.cs` (`SaveRecords`, `ShowResults`, `CalculateGrade`, `OnRetryButtonClick`, `OnMenuButtonClick`) | Generated the results screen logic: saving new records, showing time, rings and grade, calculating the S to D grade, and the Retry and Menu buttons |
| Coyote time and double jump | `PlayerController.cs` (`OnJump`, `Update`) | Helped design and debug the jump logic: a free jump from the ground, one air jump that refills on landing or after a stomp, and the coyote-time window that allows a ground jump just after leaving a ledge |
| Camera | `CameraControl.cs` | Assisted with the camera follow script |
| Title logo | `TerminalDogma_Logo_HairPink.png` | Generated the logo image, colored to match the player character's hair |

AI assistance was also used for debugging Unity setup issues (input actions, animator transitions, colliders and lighting), but all code was integrated, tested and tuned by me in the editor.
