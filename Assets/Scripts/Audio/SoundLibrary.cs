using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

/// <summary>
/// Asset that maps string keys to FMOD <see cref="EventReference"/> entries for use with <see cref="SoundManager"/>.
/// </summary>
[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Audio/Sound Library", order = 0)]
public class SoundLibrary : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        [Tooltip("Key used with SoundManager.Play / PlayOneShot (case-insensitive).")]
        public string Key;

        public EventReference Event;
    }

    [SerializeField] List<Entry> m_Entries = new List<Entry>();

    Dictionary<string, EventReference> m_Lookup;
    bool m_Built;

    void OnEnable()
    {
        m_Built = false;
        m_Lookup = null;
    }

    public IReadOnlyList<Entry> Entries => m_Entries;

    public bool TryGet(string key, out EventReference eventReference)
    {
        eventReference = default;
        EnsureLookup();
        return !string.IsNullOrEmpty(key) && m_Lookup.TryGetValue(key.Trim(), out eventReference);
    }

    void EnsureLookup()
    {
        if (m_Built && m_Lookup != null)
            return;

        m_Lookup = new Dictionary<string, EventReference>(StringComparer.OrdinalIgnoreCase);
        foreach (var e in m_Entries)
        {
            if (string.IsNullOrWhiteSpace(e.Key) || e.Event.IsNull)
                continue;

            var k = e.Key.Trim();
            if (m_Lookup.ContainsKey(k))
            {
                Debug.LogWarning($"[SoundLibrary] Duplicate key '{k}' in {name}; last assignment wins.", this);
            }

            m_Lookup[k] = e.Event;
        }

        m_Built = true;
    }
}
