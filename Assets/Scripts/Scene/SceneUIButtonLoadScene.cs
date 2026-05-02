using UnityEngine;

/// <summary>
/// Add next to a UI <c>Button</c>: set <see cref="sceneName"/>, then OnClick → this object → <see cref="LoadAssignedScene"/>.
/// Use when you need a different scene per button (the main manager only exposes Start/Restart without a string argument).
/// </summary>
public class SceneUIButtonLoadScene : MonoBehaviour
{
    [Tooltip("Must match a scene in File → Build Settings (name without .unity).")]
    [SerializeField] string sceneName;

    public void LoadAssignedScene()
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("[SceneUIButtonLoadScene] No scene name set.", this);
            return;
        }

        if (SceneChangeManager.Instance != null)
            SceneChangeManager.Instance.LoadScene(sceneName.Trim());
        else
            Debug.LogError("[SceneUIButtonLoadScene] No SceneChangeManager in the scene.", this);
    }
}
