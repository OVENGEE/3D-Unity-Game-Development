using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Level To Load")]
    public string _newGameLevel;
    private string levelToLoad;

    [SerializeField] private GameObject noSavedGameDialog; 

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
                // Stop play mode in the Unity Editor
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // Quit the built application
                Application.Quit();
        #endif
    }

}

//Code references
// 1)Title: MAIN MENU in UNIty(BEst Menu Tutorial 2024)
//    Author: SpeedTutor
//    URL:https://www.youtube.com/watch?v=Cq_Nnw_LwnI
//    Date accessed:20/10/2025