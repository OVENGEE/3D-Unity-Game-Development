using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class QuitApplication : MonoBehaviour
{
    [SerializeField] Cubemap initialSkyboxCubemap;

    private void Awake()
    {
        if (initialSkyboxCubemap == null)
        {
            Debug.LogError("cubemap not assigned in inspector");
            return;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Stop Play Mode in the editor
#else
        RenderSettings.skybox.SetTexture("_Tex1", initialSkyboxCubemap);
        RenderSettings.skybox.SetTexture("_Tex2", initialSkyboxCubemap);
        RenderSettings.skybox.SetFloat("_Blend", 0f);
        Application.Quit(); // Quit the built application
#endif
    }
     
    public void CloseQuitPanel()
    {
        gameObject.SetActive(false);
    }
}
