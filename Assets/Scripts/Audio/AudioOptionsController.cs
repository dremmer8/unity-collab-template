using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Binds UI sliders to <see cref="AudioOptionSettings"/> and persists changes to PlayerPrefs.
/// Assign the same bus paths you use in FMOD (e.g. group buses for SFX and music).
/// </summary>
[DefaultExecutionOrder(-50)]
public class AudioOptionsController : MonoBehaviour
{
    [Header("FMOD buses (must match your FMOD mixer)")]
    [SerializeField] string m_SoundBusPath = "bus:/SFX";
    [SerializeField] string m_MusicBusPath = "bus:/Music";

    [Header("UI (optional — can use only for bootstrap via script)")]
    [SerializeField] Slider m_SoundVolumeSlider;
    [SerializeField] Slider m_MusicVolumeSlider;

    void Awake()
    {
        AudioOptionSettings.Configure(m_SoundBusPath, m_MusicBusPath);
        AudioOptionSettings.LoadAndApply();
    }

    void OnEnable()
    {
        AudioOptionSettings.SoundVolumeChanged += OnSoundVolumeChanged;
        AudioOptionSettings.MusicVolumeChanged += OnMusicVolumeChanged;

        if (m_SoundVolumeSlider != null)
        {
            m_SoundVolumeSlider.SetValueWithoutNotify(AudioOptionSettings.SoundVolume);
            m_SoundVolumeSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        }

        if (m_MusicVolumeSlider != null)
        {
            m_MusicVolumeSlider.SetValueWithoutNotify(AudioOptionSettings.MusicVolume);
            m_MusicVolumeSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }
    }

    void OnDisable()
    {
        AudioOptionSettings.SoundVolumeChanged -= OnSoundVolumeChanged;
        AudioOptionSettings.MusicVolumeChanged -= OnMusicVolumeChanged;

        if (m_SoundVolumeSlider != null)
            m_SoundVolumeSlider.onValueChanged.RemoveListener(OnSoundSliderChanged);

        if (m_MusicVolumeSlider != null)
            m_MusicVolumeSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
    }

    void OnSoundSliderChanged(float value)
    {
        AudioOptionSettings.SetSoundVolume(value, save: true);
    }

    void OnMusicSliderChanged(float value)
    {
        AudioOptionSettings.SetMusicVolume(value, save: true);
    }

    void OnSoundVolumeChanged()
    {
        if (m_SoundVolumeSlider != null)
            m_SoundVolumeSlider.SetValueWithoutNotify(AudioOptionSettings.SoundVolume);
    }

    void OnMusicVolumeChanged()
    {
        if (m_MusicVolumeSlider != null)
            m_MusicVolumeSlider.SetValueWithoutNotify(AudioOptionSettings.MusicVolume);
    }
}
