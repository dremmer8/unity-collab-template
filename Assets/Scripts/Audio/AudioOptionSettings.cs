using System;
using UnityEngine;
using FMODUnity;

/// <summary>
/// PlayerPrefs-backed sound/music levels and FMOD bus application.
/// Configure bus paths via <see cref="Configure"/> (typically from <see cref="AudioOptionsController"/>).
/// </summary>
public static class AudioOptionSettings
{
    public const string PrefSoundVolume = "AudioOptions.SoundVolume";
    public const string PrefMusicVolume = "AudioOptions.MusicVolume";

    static string s_SoundBusPath = "bus:/SFX";
    static string s_MusicBusPath = "bus:/Music";

    public static float SoundVolume { get; private set; } = 1f;
    public static float MusicVolume { get; private set; } = 1f;

    public static event Action SoundVolumeChanged;
    public static event Action MusicVolumeChanged;

    public static void Configure(string soundBusPath, string musicBusPath)
    {
        s_SoundBusPath = string.IsNullOrWhiteSpace(soundBusPath) ? "bus:/SFX" : soundBusPath.Trim();
        s_MusicBusPath = string.IsNullOrWhiteSpace(musicBusPath) ? "bus:/Music" : musicBusPath.Trim();
    }

    /// <summary> Loads stored values from PlayerPrefs (defaults to 1 if unset). </summary>
    public static void Load()
    {
        SoundVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(PrefSoundVolume, 1f));
        MusicVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(PrefMusicVolume, 1f));
    }

    public static void Save()
    {
        PlayerPrefs.SetFloat(PrefSoundVolume, SoundVolume);
        PlayerPrefs.SetFloat(PrefMusicVolume, MusicVolume);
        PlayerPrefs.Save();
    }

    /// <summary> Loads from PlayerPrefs and applies volumes to FMOD buses. </summary>
    public static void LoadAndApply()
    {
        Load();
        ApplyFmodVolumes();
    }

    public static void SetSoundVolume(float linear01, bool save)
    {
        SoundVolume = Mathf.Clamp01(linear01);
        ApplySoundBus();
        if (save)
        {
            PlayerPrefs.SetFloat(PrefSoundVolume, SoundVolume);
            PlayerPrefs.Save();
        }
        SoundVolumeChanged?.Invoke();
    }

    public static void SetMusicVolume(float linear01, bool save)
    {
        MusicVolume = Mathf.Clamp01(linear01);
        ApplyMusicBus();
        if (save)
        {
            PlayerPrefs.SetFloat(PrefMusicVolume, MusicVolume);
            PlayerPrefs.Save();
        }
        MusicVolumeChanged?.Invoke();
    }

    public static void ApplyFmodVolumes()
    {
        ApplySoundBus();
        ApplyMusicBus();
    }

    static void ApplySoundBus()
    {
        TrySetBusVolume(s_SoundBusPath, SoundVolume);
    }

    static void ApplyMusicBus()
    {
        TrySetBusVolume(s_MusicBusPath, MusicVolume);
    }

    static void TrySetBusVolume(string path, float linearVolume)
    {
        if (string.IsNullOrEmpty(path))
            return;

        try
        {
            var studio = RuntimeManager.StudioSystem;
            if (!studio.isValid())
                return;

            if (studio.getBus(path, out var bus) != FMOD.RESULT.OK || !bus.isValid())
            {
                RuntimeUtils.DebugLogWarning($"[AudioOptionSettings] FMOD bus not found: '{path}'");
                return;
            }

            bus.setVolume(Mathf.Clamp01(linearVolume));
        }
        catch (SystemNotInitializedException e)
        {
            RuntimeUtils.DebugLogWarning($"[AudioOptionSettings] FMOD not initialized; cannot set bus '{path}': {e.Message}");
        }
    }
}
