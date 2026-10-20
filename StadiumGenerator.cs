using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Builds a complete football stadium from scratch (no assets needed):
/// striped pitch, markings, goals, 4 stands with crowd, roofs, floodlights.
/// Works in URP, HDRP and Built-in. Units are meters (real pitch size).
///
/// HOW TO USE:
/// 1. Create an empty GameObject, name it "Stadium", position (0,0,0).
/// 2. Add this script to it.
/// 3. Click the 3-dots menu on the component -> "Generate Stadium"
///    (or just press Play, it generates on Start).
/// </summary>
public class StadiumGenerator : MonoBehaviour
{
    [Header("Pitch (real football size)")]
    public float pitchLength = 105f;
    public float pitchWidth = 68f;
    public float runoff = 8f;

    [Header("Stands")]
    public int tiers = 20;
    public float tierDepth = 0.9f;
    public float tierHeight = 0.5f;

    [Header("Crowd")]
    public bool addCrowd = true;
    [Range(0f, 1f)] public float crowdFill = 0.75f;
    public int randomSeed = 7;
    public float sectionSize = 8f;      // crowd is split into sections of this length (for the wave)

    [Header("Floodlights")]
    public float towerHeight = 40f;
    public float lightIntensity = 3f;   // URP/Built-in. For HDRP use a much bigger value (e.g. 500000)

    public bool generateOnStart = true;

    Material grassDark, grassLight, outfield, lineMat, concrete, seatA, seatB, roofMat, metal, lampMat;
    Material[] shirtMats;
    Dictionary<int, List<CombineInstance>[]> crowd;   // crowd section id -> lists per shirt colour
    Mesh cubeMesh;
    float hl, hw;

    void Start()
    {
        if (generateOnStart) Generate();
    }

    [ContextMenu("Generate Stadium")]
    public void Generate()
    {
        // Remove old version
        var old = transform.Find("Stadium_Generated");
        if (old != null)
        {
            if (Application.isPlaying) Destroy(old.gameObject);
            else DestroyImmediate(old.gameObject);
        }

        Random.InitState(randomSeed);
        hl = pitchLength / 2f;
        hw = pitchWidth / 2f;

        var root = new GameObject("Stadium_Generated").transform;
        root.SetParent(transform, false);

        CreateMaterials();

        var tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeMesh = tmp.GetComponent<MeshFilter>().sharedMesh;
        DestroyImmediate(tmp);

        crowd = new Dictionary<int, List<CombineInstance>[]>();

        BuildGround(root);
        BuildPitchMarkings(root);
        BuildGoal(root, 1f);
        BuildGoal(root, -1f);

        float lx = pitchLength + 2f * runoff;   // length of the long stands
        float lz = pitchWidth + 2f * runoff;    // length of the end stands

        BuildStand(root, new Vector3(0, 0, hw + runoff), Quaternion.identity, lx, 0);
        BuildStand(root, new Vector3(0, 0, -(hw + runoff)), Quaternion.Euler(0, 180, 0), lx, 1);
        BuildStand(root, new Vector3(hl + runoff, 0, 0), Quaternion.Euler(0, 90, 0), lz, 2);
        BuildStand(root, new Vector3(-(hl + runoff), 0, 0), Quaternion.Euler(0, -90, 0), lz, 3);

        BuildCrowdMeshes(root);

        float cx = hl + runoff + 8f, cz = hw + runoff + 8f;
        BuildTower(root, new Vector3(cx, 0, cz));
        BuildTower(root, new Vector3(-cx, 0, cz));
        BuildTower(root, new Vector3(cx, 0, -cz));
        BuildTower(root, new Vector3(-cx, 0, -cz));
    }

    // ---------------------------------------------------------------- materials
    Material Mat(string n, Color c, float smooth = 0.1f)
    {
        Shader sh = GraphicsSettings.currentRenderPipeline != null
            ? GraphicsSettings.currentRenderPipeline.defaultShader
            : Shader.Find("Standard");
        var m = new Material(sh) { name = n, color = c };
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smooth);
        return m;
    }

    void CreateMaterials()
    {
        grassDark  = Mat("GrassDark",  new Color(0.12f, 0.45f, 0.14f));
        grassLight = Mat("GrassLight", new Color(0.18f, 0.55f, 0.18f));
        outfield   = Mat("Outfield",   new Color(0.10f, 0.30f, 0.12f));
        lineMat    = Mat("Lines",      Color.white);
        concrete   = Mat("Concrete",   new Color(0.55f, 0.55f, 0.58f));
        seatA      = Mat("SeatsA",     new Color(0.75f, 0.10f, 0.10f));
        seatB      = Mat("SeatsB",     new Color(0.85f, 0.85f, 0.85f));
        roofMat    = Mat("Roof",       new Color(0.25f, 0.27f, 0.30f), 0.5f);
        metal      = Mat("Metal",      new Color(0.35f, 0.35f, 0.38f), 0.6f);

        lampMat = Mat("Lamp", Color.white);
        lampMat.EnableKeyword("_EMISSION");
        if (lampMat.HasProperty("_EmissionColor")) lampMat.SetColor("_EmissionColor", Color.white * 3f);

        Color[] shirts =
        {
            new Color(0.85f, 0.1f, 0.1f), new Color(0.1f, 0.2f, 0.8f), Color.white,
            new Color(0.95f, 0.8f, 0.1f), new Color(0.1f, 0.1f, 0.1f), new Color(0.1f, 0.6f, 0.2f)
        };
        shirtMats = new Material[shirts.Length];
        for (int i = 0; i < shirts.Length; i++) shirtMats[i] = Mat("Shirt" + i, shirts[i], 0.05f);
    }

    // ---------------------------------------------------------------- helpers
    GameObject Box(string n, Transform parent, Vector3 pos, Quaternion rot, Vector3 scale, Material m, bool collider = true)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = n;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localRotation = rot;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = m;
        if (!collider) DestroyImmediate(go.GetComponent<Collider>());
        return go;
    }

    void Line(Transform p, Vector3 a, Vector3 b, float w = 0.12f)
    {
        Vector3 d = b - a;
        if (d.sqrMagnitude < 0.0001f) return;
        Box("Line", p, (a + b) / 2f + Vector3.up * 0.03f, Quaternion.LookRotation(d),
            new Vector3(w, 0.02f, d.magnitude + w), lineMat, false);
    }

    // ---------------------------------------------------------------- ground + pitch
    void BuildGround(Transform p)
    {
        float total = tiers * tierDepth;
        // big base under everything
        Box("Base", p, new Vector3(0, -0.2f, 0), Quaternion.identity,
            new Vector3(pitchLength + 2f * (runoff + total + 30f), 0.4f, pitchWidth + 2f * (runoff + total + 30f)),
            concrete);
        // run-off area
        Box("Outfield", p, new Vector3(0, -0.04f, 0), Quaternion.identity,
            new Vector3(pitchLength + 2f * runoff, 0.08f, pitchWidth + 2f * runoff), outfield, false);
        // pitch
        Box("Pitch", p, new Vector3(0, -0.05f, 0), Quaternion.identity,
            new Vector3(pitchLength, 0.1f, pitchWidth), grassDark);
        // mowing stripes
        int n = 14;
        float sw = pitchLength / n;
        for (int i = 0; i < n; i += 2)
            Box("Stripe", p, new Vector3(-hl + sw * (i + 0.5f), 0.005f, 0), Quaternion.identity,
                new Vector3(sw, 0.01f, pitchWidth), grassLight, false);
    }

    void BuildPitchMarkings(Transform p)
    {
        // outline + halfway line
        Line(p, new Vector3(-hl, 0, -hw), new Vector3(hl, 0, -hw));
        Line(p, new Vector3(-hl, 0, hw), new Vector3(hl, 0, hw));
        Line(p, new Vector3(-hl, 0, -hw), new Vector3(-hl, 0, hw));
        Line(p, new Vector3(hl, 0, -hw), new Vector3(hl, 0, hw));
        Line(p, new Vector3(0, 0, -hw), new Vector3(0, 0, hw));

        // centre circle + spot
        int seg = 48;
        float r = 9.15f;
        for (int i = 0; i < seg; i++)
        {
            float a1 = i * Mathf.PI * 2f / seg, a2 = (i + 1) * Mathf.PI * 2f / seg;
            Line(p, new Vector3(Mathf.Cos(a1) * r, 0, Mathf.Sin(a1) * r),
                    new Vector3(Mathf.Cos(a2) * r, 0, Mathf.Sin(a2) * r));
        }
        Box("CentreSpot", p, new Vector3(0, 0.03f, 0), Quaternion.identity, new Vector3(0.4f, 0.02f, 0.4f), lineMat, false);

        // penalty + goal areas
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
        float x = s * hl;
        Box("PostL", p, new Vector3(x, 1.22f, -3.66f), Quaternion.identity, new Vector3(0.12f, 2.44f, 0.12f), lineMat);
        Box("PostR", p, new Vector3(x, 1.22f, 3.66f), Quaternion.identity, new Vector3(0.12f, 2.44f, 0.12f), lineMat);
        Box("Crossbar", p, new Vector3(x, 2.44f, 0), Quaternion.identity, new Vector3(0.12f, 0.12f, 7.44f), lineMat);
    }

    // ---------------------------------------------------------------- stands
    // Local space of a stand: x = along the stand, z = away from the pitch.
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
        Box("BackWall", p, center + rot * new Vector3(0, wallH / 2f, total + 0.25f), rot,
            new Vector3(length, wallH, 0.5f), concrete);

        float roofD = total * 0.85f;
        Box("Roof", p, center + rot * new Vector3(0, wallH, total + 0.5f - roofD / 2f), rot,
            new Vector3(length, 0.4f, roofD), roofMat, false);
    }

    // Combine fans into a few meshes per section (fast), and register each section for the crowd wave
    void BuildCrowdMeshes(Transform p)
    {
        var wave = GetComponent<CrowdWave>();
        if (wave == null) wave = gameObject.AddComponent<CrowdWave>();
        wave.Clear();

        foreach (var kv in crowd)
        {
            var section = new GameObject("CrowdSection_" + kv.Key);
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

                var go = new GameObject("Crowd_" + g);
                go.transform.SetParent(section.transform, false);
                go.AddComponent<MeshFilter>().sharedMesh = mesh;
                go.AddComponent<MeshRenderer>().sharedMaterial = shirtMats[g];
            }
            wave.Add(section.transform, Mathf.Atan2(first.z, first.x));
        }
    }

    // ---------------------------------------------------------------- floodlights
    void BuildTower(Transform p, Vector3 basePos)
    {
        Box("Pole", p, basePos + Vector3.up * towerHeight / 2f, Quaternion.identity,
            new Vector3(0.8f, towerHeight, 0.8f), metal);

        var head = Box("LampPanel", p, basePos + Vector3.up * towerHeight, Quaternion.identity,
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
