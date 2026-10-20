# Virtual Football Stadium

An interactive 3D football stadium built with **Unity** and **C#**. Explore the stadium from multiple camera angles, interact with the football, trigger crowd waves, and customize the stadium's lighting and seating appearance.

## 🎥 Video Demo

**Watch the project demo:** [Add your video demo link here](PASTE_YOUR_VIDEO_LINK_HERE)

> Replace the placeholder above with your public YouTube, Google Drive, or other accessible demo URL.

## ✨ Features

- **3D Stadium Environment:** Football pitch with grass stripes, field markings, goals, spectator stands, player models, and floodlight structures.
- **Multiple Camera Views:** Switch between Broadcast, Top View, Inside, and Follow Ball.
- **Interactive Football:** Click the pitch to kick the ball and use the reset control to return it to its starting position.
- **Crowd Wave:** Trigger a crowd-wave effect using the on-screen button or the Space key.
- **Day/Night Mode:** Switch the stadium's lighting and appearance.
- **Seat Colour Customization:** Change the colour of the stadium seating.
- **Goal Counter:** View the current goal count.
- **Camera Controls:** Drag to orbit around the stadium and use the scroll wheel to zoom.
- **Match Configuration:** `FootballMatchManager` stores references to the player prefab and ball transform, and configures the number of players per team.

## 🕹️ Controls

| Action | How to use |
|---|---|
| Kick the ball | Click the playing pitch |
| Rotate/orbit the camera | Drag with the mouse |
| Zoom in/out | Use the mouse scroll wheel |
| Trigger a crowd wave | Press `Space` or click **Crowd Wave** |
| Reset the ball | Press `R` or click **Reset Ball** |
| Change camera view | Click **Broadcast**, **Top View**, **Inside**, or **Follow Ball** |
| Change lighting | Click **Day/Night** |
| Change seat colour | Click **Seat Colour** |

## 🛠️ Built With

- **Unity** — 3D scene development and rendering
- **C#** — interaction and game logic
- **Unity UI / OnGUI controls** — in-game buttons and status display

## 🚀 Getting Started

### Requirements

- Unity Editor (a version compatible with the project)
- A computer capable of running the Unity Editor or a compatible desktop build

### Run the Project

1. Clone or download this repository.
   ```bash
   git clone YOUR_GITHUB_REPOSITORY_URL
   ```
2. Open Unity Hub.
3. Select **Add** and choose the downloaded project folder.
4. Open the project with a compatible Unity version.
5. Open the project's main scene.
6. Press **Play** in the Unity Editor to run the stadium.

> If you publish a standalone build, add its download link and any platform requirements here.

## 📁 Main Scripts

| Script / Component | Purpose |
|---|---|
| `StadiumGenerator.cs` | Generates stadium elements such as the pitch, field markings, stands, goals, crowd sections, and floodlight structures. |
| `StadiumInteractive.cs` | Handles stadium interactions, camera movement, ball kicking and resetting, goal counting, lighting changes, and interface controls. |
| `CrowdWave.cs` | Triggers and animates the crowd-wave effect. |
| `FootballMatchManager` | Holds match-related references and player configuration, including the player prefab, ball transform, and players per team. |

*Note: Script responsibilities are described from the project documentation; adjust names or descriptions if your repository uses a different file structure.*

## 🔮 Future Improvements

- Add VR headset support through a compatible Unity XR setup.
- Improve spectator animations and crowd reactions.
- Add crowd ambience, referee whistles, ball sounds, and goal celebrations.
- Expand stadium customization options.
- Test and optimize performance across different computers.

## 👩‍💻 Author

**Pranjal Keshav Patil**

## 📄 License

No license has been specified yet. Add a license file if you want others to reuse or distribute this project.
