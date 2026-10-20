Abstract

Virtual Football Stadium is an interactive 3D football stadium application developed using
Unity. It features a generated stadium environment with a striped football pitch, field
markings, goal and ball objects, spectator stands, floodlight poles, player models, and an
interactive user interface. The interface provides Broadcast, Top View, Inside, Follow Ball,
Crowd Wave, Reset Ball, Day/Night, and Seat Colour controls, along with a goals counter.
The visible instructions indicate that the user can click the pitch to kick, drag to orbit the
camera, scroll to zoom, press Space to trigger a crowd wave, and press R to reset the ball.
The Unity Hierarchy/Inspector provided image also shows a FootballMatchManager
component with Player Prefab, Ball (Transform), and Players Per Team set to 11. These
observations provide a more accurate basis for describing the implementation than a generic
VR template.
The project is developed in Unity. The current implementation provides a desktop 3D
experience with interactive camera views and stadium controls. Immersive headset support
and navigation across four separate Unity scenes should be included only if they have been
configured and implemented in the project.

Table of Contents

Chapter
No.

Title Page
Number
Abstract ii
1 Introduction 1

1.1 Background
1.2 Aim and Objectives
1.3 Scope

2 Problem Statement 3

2.1 Existing Approach and Limitations

3 Proposed System 4

3.1 System Architecture
3.2 Working
3.3 Main Components

4 Implementation Result and Analysis 8

4.1 Implementation
4.2 Results

5 Code 11
6 Conclusion 20
References

1. INTRODUCTION

1.1 Background
Virtual Reality (VR) and interactive 3D environments provide users with opportunities to explore and
interact with virtual sports venues from different viewpoints. Unity is a powerful game development
platform that supports 3D object creation, scene management, rendering, camera control, animation,
user input handling, and C#-based scripting.
The Virtual Football Stadium project aims to develop an interactive 3D football stadium environment
that simulates the appearance and basic interactions of a football match setting. The environment
consists of a football pitch with field markings, spectator stands, player models, stadium lighting, and
interactive controls.
The application provides multiple camera views, ball interaction, crowd-wave functionality, day and
night modes, seat-colour customization, and a goal counter. These features enhance user engagement
by allowing users to explore the stadium environment and interact with its elements through a
desktop-based Unity application.
1.2 Aim and Objectives
Aim: To develop an interactive virtual football stadium using Unity, incorporating a 3D stadium
environment, multiple camera viewpoints, ball interaction, and configurable stadium effects.
Objectives:
● To design and develop a 3D football stadium with a playing field, field markings, and
surrounding spectator stands.
● To implement multiple camera viewpoints, including Broadcast, Top View, Inside, and Follow
Ball.
● To enable ball interaction through mouse input and provide a reset mechanism using the R key
or the user interface.
● To implement a crowd-wave feature triggered through the Space key or the corresponding user
interface control.

1

● To provide Day/Night and Seat Colour controls for modifying the stadium's appearance.
● To display a goal counter for tracking match-related scoring information.
● To integrate a FootballMatchManager component for coordinating match-related objects and
player configuration.
1.3 Scope
The scope of the project includes the development of a 3D football stadium environment with
interactive camera controls, player objects, ball interaction, stadium lighting controls, crowd-wave
functionality, seat-colour customization, and a goal-count display.
The application focuses on providing an interactive desktop-based experience using Unity's rendering
and scripting capabilities. It can serve as a foundation for further development into a more immersive
virtual sports environment.
Future extensions may include VR headset integration, advanced player animations, realistic crowd
behaviour, sound effects, match simulation, and additional interactive features.

2

2. PROBLEM STATEMENT

Traditional static representations of football stadiums provide limited opportunities for exploration
and interaction. Users cannot freely change viewpoints, interact with the ball, or modify stadium
conditions when using a fixed image or video.
The Virtual Football Stadium project addresses these limitations by developing an interactive 3D
environment in Unity. It enables users to explore the stadium through different camera perspectives
and interact with selected elements through keyboard and mouse inputs. Features such as ball control,
crowd-wave activation, lighting changes, and seat-colour customization improve the overall
interactivity of the virtual stadium.
2.1 Existing Approach and Limitations
Conventional stadium representations commonly use photographs, videos, or static 3D models to
present the structure and appearance of a sports venue. Although these approaches are useful for
visualization, they provide limited real-time interaction.
The major limitations of traditional approaches include:
● Restricted viewing angles and limited user control over the viewpoint.
● Inability to interact directly with objects in the environment.
● Limited customization of stadium appearance and lighting.
● Lack of dynamic interaction between the user and the virtual environment.
An interactive 3D application overcomes these limitations by allowing users to change camera
perspectives, control selected objects, and activate environmental effects. However, immersive VR
functionality requires additional configuration of compatible XR systems and hardware.

3

3. PROPOSED SYSTEM

3.1 System Architecture
The system architecture consists of four major layers: User Input, User Interface, Game Logic, and
Stadium Environment and Rendering.
1. User Input Layer
This layer accepts keyboard and mouse inputs from the user. Mouse interactions support ball control,
camera orbiting, and zooming, while keyboard inputs activate actions such as crowd-wave behaviour
and ball resetting.
2. User Interface Layer
The user interface provides controls for selecting camera views, resetting the ball, activating the
crowd wave, switching between day and night modes, and changing seat colours. A goal counter
displays the current scoring information.
3. Game Logic Layer
The game logic processes user inputs and coordinates interactions between the application
components. C# scripts manage camera switching, ball behaviour, stadium controls, and
match-related object configuration. The FootballMatchManager component provides references to
player and ball objects and stores the players-per-team configuration.
4. Stadium Environment and Rendering Layer
This layer contains the 3D football pitch, field markings, spectator stands, player models, ball, and
lighting structures. Unity's rendering system generates the final visual output based on the active
camera, scene objects, materials, lighting, and environmental settings.

4

The interaction flow begins with user input, which is processed through the interface and game logic.
The corresponding scene objects are updated, and Unity renders the modified environment in the
Game view.

Fig. 3.1: System Architecture of Virtual Football Stadium

3.2 Working
1. Camera View Controls
The application provides multiple camera viewpoints to support different ways of exploring the
stadium.
- Broadcast View: Provides a wide-angle perspective suitable for viewing the overall playing
field.
- Top View: Provides an elevated perspective of the stadium and football pitch.
- Inside View: Provides a viewpoint from within or near the playing area.
- Follow Ball: Allows the camera to follow the ball during interaction.
Users can also orbit the camera by dragging the mouse and zoom in or out using the scroll wheel.

5

2. Ball Interaction
The ball interaction feature enables users to interact with the football through mouse input. Clicking
the playing field triggers the ball-kick interaction. The ball can be returned to its initial or designated
reset position using the R key or the Reset Ball button.
3. Crowd-Wave Functionality
The crowd-wave feature introduces an interactive stadium effect. Users can activate the behaviour
using the Space key or the Crowd Wave button, providing a more engaging stadium experience.
4. Day/Night Mode
The Day/Night control modifies the stadium's lighting or time-of-day appearance. This feature allows
users to experience the stadium under different visual conditions.
5. Seat Colour Customization
The Seat Colour control provides an option to modify the appearance of the stadium seating, enabling
customization of the spectator stands.
6. Goal Counter
The goal counter displays the current number of goals during the application session. It provides a
simple match-related status indicator within the user interface.
3.3 Main Components
3.3.1 Stadium Environment
The stadium environment forms the primary component of the application. It consists of a green
football pitch with alternating grass stripes and white field markings, surrounded by spectator stands
with multicoloured seating details.

6

Additional environmental elements include stadium roof sections, floodlight poles, player models,
and a football. Together, these elements create a recognizable football stadium setting within a
three-dimensional environment.
3.3.2 Player and Team Objects
The application includes player objects represented through separate body parts, such as the body,
head, left leg, and right leg. Team-related objects are organized within the Unity scene hierarchy.
The FootballMatchManager component includes a Player Prefab reference and a Players Per Team
parameter set to 11. These components provide a foundation for configuring player objects and
representing football teams within the environment.
3.3.3 FootballMatchManager
FootballMatchManager is a C# script component responsible for organizing match-related
configuration and object references within the application.
Its main configuration fields include:
● Player Prefab: Reference to the player object prefab.
● Ball Transform: Reference to the football's transform.
● Players Per Team: Specifies the configured number of players per team.
These fields support the organization of player and ball objects and provide a basis for extending the
application with additional match-management functionality.

7

4. IMPLEMENTATION RESULTS AND ANALYSIS

4.1 Implementation
The Virtual Football Stadium application is developed using Unity's 3D environment and C# scripting
capabilities. The stadium is constructed from scene objects representing the playing field, spectator
stands, player models, ball, and lighting structures.
Interactive controls are integrated into the Game view to provide access to camera switching, ball
interaction, crowd-wave activation, stadium appearance controls, and match-related information.
The camera controls provide different perspectives of the playing area, while the ball interaction and
reset mechanisms support basic object interaction. The crowd-wave feature adds an interactive
stadium effect, and the Day/Night and Seat Colour controls provide options for modifying the
environment's appearance.
The goal counter presents scoring information within the interface. The FootballMatchManager
component provides references and configuration parameters for player and ball objects.

4.2 Results
The implemented application provides an interactive 3D football stadium with the following features:
● A football pitch with grass patterns and standard field markings.
● Spectator stands with multicoloured seating details.
● Player objects and a football within the stadium environment.
● Multiple camera viewpoints for exploring the playing area.
● Mouse-based camera orbiting and zooming.
● Ball interaction and reset controls.
● Crowd-wave activation through keyboard or user interface input.
● Day/Night and Seat Colour controls.
● A goal counter for displaying match-related scoring information.
● A FootballMatchManager component with player and ball configuration fields.

8

Figure 4.1: Stadium from the Inside / Low Camera View

Figure 4.2: Elevated / Top View of the Stadium

9

Figure 4.3: Unity Game View and Stadium Controls

These features establish a foundation for interactive stadium visualization and demonstrate how Unity
can be used to combine 3D scene construction, user interface design, input handling, and game logic
in a single application.

10

5. CODE

CrowdWave.cs
using System.Collections.Generic;
using UnityEngine;
public class CrowdWave : MonoBehaviour
{
public float speed = 6f;
public float height = 1.1f;
public float frequency = 3f;
readonly List<Transform> sections = new List<Transform>();
readonly List<float> angles = new List<float>();
float timeLeft, t;
public void Clear() { sections.Clear(); angles.Clear(); }
public void Add(Transform section, float angle)
{
sections.Add(section);
angles.Add(angle);
}
public void Trigger(float duration = 3f)
{
timeLeft = duration;
t = 0f;
}
void Update()
{
if (timeLeft <= 0f) return;
timeLeft -= Time.deltaTime;
t += Time.deltaTime;
for (int i = 0; i < sections.Count; i++)
{
if (sections[i] == null) continue;
float y = timeLeft > 0f ? Mathf.Max(0f, Mathf.Sin(angles[i] * frequency - t * speed)) * height : 0f;
Vector3 p = sections[i].localPosition;
p.y = y;
sections[i].localPosition = p;
}
}
}
StadiumGenerator.cs
float sw = pitchLength / n;
for (int i = 0; i < n; i += 2)

11

Box("Stripe", p, new Vector3(-hl + sw * (i + 0.5f), 0.005f, 0), Quaternion.identity,
new Vector3(sw, 0.01f, pitchWidth), grassLight, false);
}
void BuildPitchMarkings(Transform p)
{
Line(p, new Vector3(-hl, 0, -hw), new Vector3(hl, 0, -hw));
Line(p, new Vector3(-hl, 0, hw), new Vector3(hl, 0, hw));
Line(p, new Vector3(-hl, 0, -hw), new Vector3(-hl, 0, hw));
Line(p, new Vector3(hl, 0, -hw), new Vector3(hl, 0, hw));
Line(p, new Vector3(0, 0, -hw), new Vector3(0, 0, hw));
int seg = 48;
float r = 9.15f;
for (int i = 0; i < seg; i++)
{
float a1 = i * Mathf.PI * 2f / seg, a2 = (i + 1) * Mathf.PI * 2f / seg;
Line(p, new Vector3(Mathf.Cos(a1) * r, 0, Mathf.Sin(a1) * r),
new Vector3(Mathf.Cos(a2) * r, 0, Mathf.Sin(a2) * r));
}
Box("CentreSpot", p, new Vector3(0, 0.03f, 0), Quaternion.identity, new Vector3(0.4f, 0.02f, 0.4f), lineMat, false);
foreach (float s in new[] { -1f, 1f })
{
BoxArea(p, s, 16.5f, 20.16f);
BoxArea(p, s, 5.5f, 9.16f);
Box("PenaltySpot", p, new Vector3(s * (hl - 11f), 0.03f, 0), Quaternion.identity,
new Vector3(0.4f, 0.02f, 0.4f), lineMat, false);
}
}
void BoxArea(Transform p, float s, float depth, float halfWidth)
{
float x1 = s * hl, x2 = s * (hl - depth);
Line(p, new Vector3(x2, 0, -halfWidth), new Vector3(x2, 0, halfWidth));
Line(p, new Vector3(x1, 0, -halfWidth), new Vector3(x2, 0, -halfWidth));
Line(p, new Vector3(x1, 0, halfWidth), new Vector3(x2, 0, halfWidth));
}
void BuildGoal(Transform p, float s)
{

12

float x = s * hl;
Box("PostL", p, new Vector3(x, 1.22f, -3.66f), Quaternion.identity, new Vector3(0.12f, 2.44f, 0.12f), lineMat);
Box("PostR", p, new Vector3(x, 1.22f, 3.66f), Quaternion.identity, new Vector3(0.12f, 2.44f, 0.12f), lineMat);
Box("Crossbar", p, new Vector3(x, 2.44f, 0), Quaternion.identity, new Vector3(0.12f, 0.12f, 7.44f), lineMat);
}
void BuildStand(Transform p, Vector3 center, Quaternion rot, float length, int standId)
{
float total = tiers * tierDepth;
for (int i = 0; i < tiers; i++)
{
float h = tierHeight * (i + 1);
Vector3 local = new Vector3(0, h / 2f, i * tierDepth + tierDepth / 2f);
Box("Tier" + i, p, center + rot * local, rot, new Vector3(length, h, tierDepth),
((i / 4) % 2 == 0) ? seatA : seatB);
if (!addCrowd) continue;
float y = h + 0.45f;
for (float x = -length / 2f + 0.5f; x < length / 2f - 0.5f; x += 0.55f)
{
if (Random.value > crowdFill) continue;
Vector3 lp = new Vector3(x, y, i * tierDepth + tierDepth * 0.55f);
int g = Random.Range(0, shirtMats.Length);
int sec = standId * 1000 + Mathf.FloorToInt((x + length / 2f) / sectionSize);
if (!crowd.TryGetValue(sec, out var lists))
{
lists = new List<CombineInstance>[shirtMats.Length];
for (int k = 0; k < lists.Length; k++) lists[k] = new List<CombineInstance>();
crowd[sec] = lists;
}
lists[g].Add(new CombineInstance
{
mesh = cubeMesh,
transform = Matrix4x4.TRS(center + rot * lp, rot, new Vector3(0.4f, 0.9f, 0.4f))
});
}
}
float wallH = tiers * tierHeight + 6f;

13

Box("BackWall", p, center + rot * new Vector3(0, wallH / 2f, total + 0.25f), rot,
new Vector3(length, wallH, 0.5f), concrete);
float roofD = total * 0.85f;
Box("Roof", p, center + rot * new Vector3(0, wallH, total + 0.5f - roofD / 2f), rot,
new Vector3(length, 0.4f, roofD), roofMat, false);
}
void BuildCrowdMeshes(Transform p)
{
var wave = GetComponent<CrowdWave>();
if (wave == null) wave = gameObject.AddComponent<CrowdWave>();
wave.Clear();
foreach (var kv in crowd)
{
var section = new GameObject("CrowdSection\_" + kv.Key);
section.transform.SetParent(p, false);
Vector3 first = Vector3.zero;
bool hasFirst = false;
for (int g = 0; g < kv.Value.Length; g++)
{
var list = kv.Value[g];
if (list.Count == 0) continue;
if (!hasFirst) { first = list[0].transform.GetColumn(3); hasFirst = true; }
var mesh = new Mesh { name = "Crowd" + g, indexFormat = IndexFormat.UInt32 };
mesh.CombineMeshes(list.ToArray(), true, true);
var go = new GameObject("Crowd\_" + g);
go.transform.SetParent(section.transform, false);
go.AddComponent<MeshFilter>().sharedMesh = mesh;
go.AddComponent<MeshRenderer>().sharedMaterial = shirtMats[g];
}
wave.Add(section.transform, Mathf.Atan2(first.z, first.x));
}
}
void BuildTower(Transform p, Vector3 basePos)
{
Box("Pole", p, basePos + Vector3.up * towerHeight / 2f, Quaternion.identity,
new Vector3(0.8f, towerHeight, 0.8f), metal);
var head = Box("LampPanel", p, basePos + Vector3.up * towerHeight, Quaternion.identity,

14

new Vector3(7f, 3.5f, 0.6f), lampMat, false);
head.transform.LookAt(transform.position + Vector3.up * 0f);
var lightGo = new GameObject("Spot");
lightGo.transform.SetParent(head.transform, false);
lightGo.transform.localPosition = new Vector3(0, 0, 1f);
var l = lightGo.AddComponent<Light>();
l.type = LightType.Spot;
l.range = 170f;
l.spotAngle = 80f;
l.intensity = lightIntensity;
l.color = new Color(1f, 0.97f, 0.9f);
l.shadows = LightShadows.None;
}
}
StadiumInteractive.cs
if (Mathf.Abs(s) > 0.01f && !overUI)
{
auto = false; hasGoal = false;
rad = Mathf.Clamp(rad * (1f - s * 0.1f), 18f, 320f);
}
}
void Kick()
{
if (resetTimer > 0f) return;
Ray ray = cam.ScreenPointToRay(Input.mousePosition);
var plane = new Plane(stadium.transform.up, stadium.transform.position);

if (!plane.Raycast(ray, out float dist)) return;

15

Vector3 hit = stadium.transform.InverseTransformPoint(ray.GetPoint(dist));
Vector2 d = new Vector2(hit.x - ball.localPosition.x, hit.z - ball.localPosition.z);
float len = Mathf.Max(d.magnitude, 0.01f);
vel = d / len * Mathf.Min(48f, 8f + len * 1.2f);
}
void ResetBall()
{
ball.localPosition = new Vector3(0f, ballRadius, 0f);
vel = Vector2.zero;
resetTimer = 0f;
}
void StepBall(float dt)
{
if (resetTimer > 0f)
{
resetTimer -= dt;
if (resetTimer <= 0f) ResetBall();
return;
}
Vector3 p = ball.localPosition;
p.x += vel.x * dt;
p.z += vel.y * dt;
vel *= Mathf.Pow(0.45f, dt);
float hl = stadium.pitchLength / 2f, hw = stadium.pitchWidth / 2f;
if(Mathf.Abs(p.x) > hl + 0.4f && Mathf.Abs(p.x) < hl + 4f && Mathf.Abs(p.z) < 3.66f)

16

{
score++;
if (wave) wave.Trigger();
resetTimer = 1.8f;
vel *= 0.2f;
ball.localPosition = p;
return;
}
float mx = hl + stadium.runoff - 0.5f, mz = hw + stadium.runoff - 0.5f;
if (Mathf.Abs(p.x) > mx) { p.x = Mathf.Sign(p.x) * mx; vel.x *= -0.6f; }
if (Mathf.Abs(p.z) > mz) { p.z = Mathf.Sign(p.z) * mz; vel.y *= -0.6f; }
ball.localPosition = p;
}
void GoTo(float a, float b, float r)
{
float d = Mathf.DeltaAngle(th * Mathf.Rad2Deg, a * Mathf.Rad2Deg) * Mathf.Deg2Rad;
goal = new Vector3(th + d, b, r);
hasGoal = true; auto = false;
}
void SetNight(bool n)
{
night = n;
if (sun) sun.intensity = n ? 0.1f : 1f;
foreach (var l in lights)

17

if
(l.type == LightType.Spot) l.intensity = n ? stadium.lightIntensity : 0f;
RenderSettings.ambientMode = AmbientMode.Flat;
RenderSettings.ambientLight = n ? new Color(.08f, .10f, .20f) : new Color(.60f, .65f, .75f);
cam.clearFlags = n ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
cam.backgroundColor = new Color(.04f, .07f, .18f);
}
void OnGUI()
{
if (ball == null) return;
if (btn == null)
{
btn = new GUIStyle(GUI.skin.button) { fontSize = 16 };
lbl = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
}
GUI.Label(new Rect(15, 10, 700, 25), "Click the pitch to kick | Drag = orbit | Scroll = zoom | Space = wave | R =
reset ball");
GUI.Label(new Rect(Screen.width - 220, 10, 210, 32), "Goals: " + score, lbl);
GUILayout.BeginArea(new Rect(10, Screen.height - 50, Screen.width - 20, 44));
GUILayout.BeginHorizontal();
if (GUILayout.Button("Broadcast", btn, GUILayout.Height(40))) { follow = false; GoTo(0f, 1.2f, 150f); }
if (GUILayout.Button("Top view", btn, GUILayout.Height(40))) { follow = false; GoTo(0f, 0.12f, 170f); }
if (GUILayout.Button("Inside", btn, GUILayout.Height(40))) { follow = false; GoTo(0.4f, 1.42f, 45f); }
if (GUILayout.Button(follow ? "Follow ball: ON" : "Follow ball", btn, GUILayout.Height(40)))
{
follow = !follow;

18

if (follow) GoTo(th, 1.15f, 50f); else GoTo(th, 0.95f, 170f);
}
if (GUILayout.Button("Crowd wave", btn, GUILayout.Height(40)) && wave) wave.Trigger();
if (GUILayout.Button("Reset ball", btn, GUILayout.Height(40))) ResetBall();
if (GUILayout.Button(night ? "Day" : "Night", btn, GUILayout.Height(40))) SetNight(!night);
if (GUILayout.Button("Seat colour", btn, GUILayout.Height(40)) && seatMat)
{
colorIdx = (colorIdx + 1) % seatColors.Length;
seatMat.color = seatColors[colorIdx];
}
GUILayout.EndHorizontal();
GUILayout.EndArea(); )

19

6. CONCLUSION

6.1 Conclusion
The Virtual Football Stadium project demonstrates the development of an interactive 3D football
stadium environment using Unity. The application combines a football pitch, spectator stands, player
objects, lighting structures, camera controls, ball interaction, crowd-wave functionality, stadium
appearance controls, and a goal counter.
The integration of multiple camera viewpoints enables users to explore the stadium from different
perspectives, while interactive controls provide basic engagement with the environment. Overall, the
project demonstrates the application of 3D modelling, scene management, rendering, user interface
design, and C# scripting in developing an interactive virtual sports environment. It also provides a
foundation for extending the application with more advanced gameplay and immersive technologies.

6.2 Future Scope
The project can be enhanced through the following improvements:
● VR Integration: Integrate a compatible XR system and VR headset to provide an immersive
first-person stadium experience.
● Realistic Crowd Behaviour: Add animated spectators, coordinated crowd reactions, and
more dynamic stadium effects.
● Audio Integration: Include crowd ambience, referee whistles, ball-kicking sounds, and goal
celebrations.
● Enhanced Stadium Customization: Provide additional options for changing seating patterns,
lighting intensity, and stadium appearance.
● Testing and Optimization: Evaluate performance, usability, camera controls, and interaction
reliability across different hardware configurations.
These enhancements can improve realism, interactivity, and usability while providing a pathway
toward a more comprehensive virtual football experience.
20

REFERENCES
[1] P. Dickinson et al., “Virtual Reality Crowd Simulation: Effects of Agent Density on User
Experience and Behaviour,” Virtual Reality, vol. 23, pp. 19–32, 2019.
https://doi.org/10.1007/s10055-018-0365-0.
[2] J. Pettré et al., “Real-time Navigating Crowds: Scalable Simulation and Rendering,” Computer
Animation and Virtual Worlds, 2006. https://doi.org/10.1002/cav.147.
[3] A. V. Vincent and P. F. Frewen, “Being Where, With Whom, and When It Happens: Spatial,
Interpersonal, and Temporal Presence While Viewing Live Streaming of Collegiate Sports in Virtual
Reality,” Frontiers in Virtual Reality, vol. 4, 2023. https://doi.org/10.3389/frvir.2023.1167051.
[4] P. F. Frewen and A. V. Vincent, “Presence in Time: Watching Live and Recorded Sports in VR
Increases Spatial, Interpersonal, and Temporal Presence,” Frontiers in Virtual Reality, vol. 5, 2024.
https://doi.org/10.3389/frvir.2024.1432888.
[5] B. Ulicny and D. Thalmann, “Towards Interactive Real-Time Crowd Behavior Simulation,”
Computer Graphics Forum, 2003. https://doi.org/10.1111/1467-8659.00634.
[6] W. van Toll et al., “Algorithms for Microscopic Crowd Simulation: Advancements in the 2010s,”
Computer Graphics Forum, vol. 40, no. 2, pp. 731–754, 2021. https://doi.org/10.1111/cgf.142664.
