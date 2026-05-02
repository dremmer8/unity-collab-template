using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Place on a GameObject in your bootstrap / splash scene. On Start, loads the configured game scene.
/// Pair with <see cref="SceneChangeManager.RestartGame"/> so restarts reset through this scene first.
/// </summary>
public class SceneBootstrapAutoLoader : MonoBehaviour
{
    [Tooltip("Scene opened after this bootstrap loads (must be in Build Settings).")]
    [SerializeField] string m_GameSceneName = "SampleScene";

    [SerializeField] LoadSceneMode m_Mode = LoadSceneMode.Single;

    [SerializeField] bool m_LoadOnStart = true;

    void Start()
    {
        if (!m_LoadOnStart)
            return;

        LoadGameScene();
    }

    /// <summary> Loads <see cref="m_GameSceneName"/> immediately (same rules as Start auto-load). </summary>
    public void LoadGameScene()
    {
        if (string.IsNullOrWhiteSpace(m_GameSceneName))
        {
            Debug.LogError("[SceneBootstrapAutoLoader] Game scene name is empty.", this);
            return;
        }

        var name = m_GameSceneName.Trim();
        if (!Application.CanStreamedLevelBeLoaded(name))
        {
            Debug.LogError($"[SceneBootstrapAutoLoader] Scene '{name}' is not in Build Settings.", this);
            return;
        }

        SceneManager.LoadScene(name, m_Mode);
    }
}
