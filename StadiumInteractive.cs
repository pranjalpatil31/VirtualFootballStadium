using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Adds the interactive features to the generated stadium:
/// click the pitch to kick a ball, score goals, crowd wave, camera views,
/// follow-ball camera, day/night, seat colour. UI is drawn with OnGUI, so no Canvas is needed.
///
/// SETUP: add this to the same "Stadium" object that has StadiumGenerator, then press Play.
/// Needs Project Settings -> Player -> Active Input Handling = "Both" (or "Input Manager (Old)").
/// </summary>
public class StadiumInteractive : MonoBehaviour
{
    public StadiumGenerator stadium;
    public Camera cam;
    public float ballRadius = 0.45f;

    Transform ball;
    Vector2 vel;                       // ball velocity on the pitch (x, z)
    int score;
    float resetTimer;

    CrowdWave wave;
    Light sun;
    Light[] lights;
    Material seatMat;
    int colorIdx;

    bool night, follow, auto = true;
    float th = 0.6f, ph = 0.95f, rad = 190f;   // orbit angles + distance
    bool hasGoal; Vector3 goal;                // camera tween target (th, ph, rad)
    Vector3 tgt;

    bool dragging; Vector2 downPos, lastMouse; float downTime;
    GUIStyle btn, lbl;

    static readonly Color[] seatColors =
    {
        new Color(.75f, .10f, .10f), new Color(.10f, .25f, .80f), new Color(.10f, .60f, .25f),
        new Color(.95f, .75f, .10f), new Color(.50f, .20f, .70f)
    };

    IEnumerator Start()
    {
        if (!stadium) stadium = FindFirstObjectByType<StadiumGenerator>();
        if (!cam) cam = Camera.main;
        yield return null;   // wait one frame so the generator has finished building

        wave = stadium.GetComponent<CrowdWave>();
        lights = stadium.GetComponentsInChildren<Light>();
        foreach (var l in FindObjectsByType<Light>(FindObjectsSortMode.None))
            if (l.type == LightType.Directional) { sun = l; break; }
        foreach (var r in stadium.GetComponentsInChildren<Renderer>())
            if (r.sharedMaterial != null && r.sharedMaterial.name == "SeatsA") { seatMat = r.sharedMaterial; break; }

        var b = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(b.GetComponent<Collider>());
        b.name = "Ball";
        ball = b.transform;
        ball.SetParent(stadium.transform, false);
        ball.localScale = Vector3.one * ballRadius * 2f;
        ResetBall();
        tgt = stadium.transform.position;
    }

    void Update()
    {
        if (ball == null) return;
        float dt = Time.deltaTime;

        HandleInput();
        StepBall(dt);

        if (Input.GetKeyDown(KeyCode.Space) && wave) wave.Trigger();
        if (Input.GetKeyDown(KeyCode.R)) ResetBall();

        // camera
        if (auto) th += dt * 0.12f;
        if (hasGoal)
        {
            float k = 1f - Mathf.Exp(-6f * dt);
            th = Mathf.Lerp(th, goal.x, k);
            ph = Mathf.Lerp(ph, goal.y, k);
            rad = Mathf.Lerp(rad, goal.z, k);
            if (Mathf.Abs(goal.x - th) + Mathf.Abs(goal.y - ph) + Mathf.Abs(goal.z - rad) < 0.02f) hasGoal = false;
        }
        Vector3 target = follow ? ball.position : stadium.transform.position;
        tgt = Vector3.Lerp(tgt, target, 1f - Mathf.Exp(-6f * dt));
        cam.transform.position = tgt + new Vector3(
            rad * Mathf.Sin(ph) * Mathf.Sin(th),
            rad * Mathf.Cos(ph),
            rad * Mathf.Sin(ph) * Mathf.Cos(th));
        cam.transform.LookAt(tgt);
    }

    // ------------------------------------------------------------ input
    void HandleInput()
    {
        bool overUI = Input.mousePosition.y < 60f;   // bottom button bar

        if (Input.GetMouseButtonDown(0) && !overUI)
        {
            dragging = true;
            downPos = lastMouse = Input.mousePosition;
            downTime = Time.time;
            auto = false; hasGoal = false;
        }
        if (dragging && Input.GetMouseButton(0))
        {
            Vector2 m = Input.mousePosition, d = m - lastMouse;
            th -= d.x * 0.006f;
            ph = Mathf.Clamp(ph + d.y * 0.006f, 0.1f, 1.5f);
            lastMouse = m;
        }
        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;
            // a short click without moving = kick
            if (((Vector2)Input.mousePosition - downPos).magnitude < 6f && Time.time - downTime < 0.4f) Kick();
        }

        float s = Input.mouseScrollDelta.y;
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

        Vector3 hit = stadium.transform.InverseTransformPoint(ray.GetPoint(dist));
        Vector2 d = new Vector2(hit.x - ball.localPosition.x, hit.z - ball.localPosition.z);
        float len = Mathf.Max(d.magnitude, 0.01f);
        vel = d / len * Mathf.Min(48f, 8f + len * 1.2f);
    }

    // ------------------------------------------------------------ ball + goals
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
        vel *= Mathf.Pow(0.45f, dt);   // rolling friction

        float hl = stadium.pitchLength / 2f, hw = stadium.pitchWidth / 2f;

        // goal: crosses the goal line between the posts
        if (Mathf.Abs(p.x) > hl + 0.4f && Mathf.Abs(p.x) < hl + 4f && Mathf.Abs(p.z) < 3.66f)
        {
            score++;
            if (wave) wave.Trigger();
            resetTimer = 1.8f;
            vel *= 0.2f;
            ball.localPosition = p;
            return;
        }

        // bounce off the edge of the run-off area
        float mx = hl + stadium.runoff - 0.5f, mz = hw + stadium.runoff - 0.5f;
        if (Mathf.Abs(p.x) > mx) { p.x = Mathf.Sign(p.x) * mx; vel.x *= -0.6f; }
        if (Mathf.Abs(p.z) > mz) { p.z = Mathf.Sign(p.z) * mz; vel.y *= -0.6f; }

        ball.localPosition = p;
    }

    // ------------------------------------------------------------ camera / scene helpers
    void GoTo(float a, float b, float r)
    {
        float d = Mathf.DeltaAngle(th * Mathf.Rad2Deg, a * Mathf.Rad2Deg) * Mathf.Deg2Rad;  // shortest way round
        goal = new Vector3(th + d, b, r);
        hasGoal = true; auto = false;
    }

    void SetNight(bool n)
    {
        night = n;
        if (sun) sun.intensity = n ? 0.1f : 1f;
        foreach (var l in lights)
            if (l.type == LightType.Spot) l.intensity = n ? stadium.lightIntensity : 0f;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = n ? new Color(.08f, .10f, .20f) : new Color(.60f, .65f, .75f);
        cam.clearFlags = n ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
        cam.backgroundColor = new Color(.04f, .07f, .18f);
    }

    // ------------------------------------------------------------ UI (no Canvas needed)
    void OnGUI()
    {
        if (ball == null) return;
        if (btn == null)
        {
            btn = new GUIStyle(GUI.skin.button) { fontSize = 16 };
            lbl = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
        }

        GUI.Label(new Rect(15, 10, 700, 25), "Click the pitch to kick  |  Drag = orbit  |  Scroll = zoom  |  Space = wave  |  R = reset ball");
        GUI.Label(new Rect(Screen.width - 220, 10, 210, 32), "Goals: " + score, lbl);

        GUILayout.BeginArea(new Rect(10, Screen.height - 50, Screen.width - 20, 44));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Broadcast", btn, GUILayout.Height(40))) { follow = false; GoTo(0f, 1.2f, 150f); }
        if (GUILayout.Button("Top view", btn, GUILayout.Height(40)))  { follow = false; GoTo(0f, 0.12f, 170f); }
        if (GUILayout.Button("Inside", btn, GUILayout.Height(40)))    { follow = false; GoTo(0.4f, 1.42f, 45f); }
        if (GUILayout.Button(follow ? "Follow ball: ON" : "Follow ball", btn, GUILayout.Height(40)))
        {
            follow = !follow;
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
        GUILayout.EndArea();
    }
}
