using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class QuitApplication : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Stop Play Mode in the editor
#else
        Application.Quit(); // Quit the built application
#endif
    }
     
    public void CloseQuitPanel()
    {
        gameObject.SetActive(false);
    }
}
