using UnityEngine;

/// <summary>
/// For Unity UI: add this to the <b>same</b> DontDestroyOnLoad GameObject as your bootstrap
/// <see cref="SceneChangeManager"/> (not on a canvas that lives only in the game or menu scene).
/// Wire buttons to <i>this</i> component — never to a scene-local copy of <see cref="SceneChangeManager"/>
/// that gets removed when duplicates are stripped. Calls always go through <see cref="SceneChangeManager.Instance"/>.
/// </summary>
public class SceneChangeUIButtons : MonoBehaviour
{
    public void UI_StartGame() => SceneChangeManager.Instance?.StartGame();

    public void UI_RestartGame() => SceneChangeManager.Instance?.RestartGame();

    public void UI_ExitGame() => SceneChangeManager.Instance?.ExitGame();
}
