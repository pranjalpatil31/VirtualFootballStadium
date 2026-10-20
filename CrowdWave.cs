using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the crowd sections rise and fall in a wave that travels around the stadium.
/// StadiumGenerator adds this component automatically and registers every crowd section.
/// Call Trigger() from code, or use the buttons/Space key in StadiumInteractive.
/// </summary>
public class CrowdWave : MonoBehaviour
{
    public float speed = 6f;        // how fast the wave travels
    public float height = 1.1f;     // how high fans jump (meters)
    public float frequency = 3f;    // number of waves around the stadium

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
