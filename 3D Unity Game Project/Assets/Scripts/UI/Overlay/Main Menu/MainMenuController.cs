using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject confirmationPrompt;
    [SerializeField] private float defaultVolumeValue = 0.5f;

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

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        //Show Prompt 
        StartCoroutine(ConfirmationBox());

        if(PlayerPrefs.HasKey("masterVolume"))
        {
            float volume_Amount = PlayerPrefs.GetFloat("masterVolume");
            Debug.Log($"The game has now an audio of {volume_Amount} %");
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2f);
        confirmationPrompt.SetActive(false);
    }

    public void ResetButton(string settings)
    {
        if (settings == "Audio")
        {
            //Resets all the Audio Settings
            AudioListener.volume = defaultVolumeValue;
            volumeTextValue.text = defaultVolumeValue.ToString("0.0");
            volumeSlider.value = defaultVolumeValue;
            VolumeApply();
        }
    }

    public enum MainSettingType
    {
        Audio,
        Graphics,
        Controls
    }

}

//Code references
// 1)Title: MAIN MENU in UNIty(BEst Menu Tutorial 2024)
//    Author: SpeedTutor
//    URL:https://www.youtube.com/watch?v=Cq_Nnw_LwnI
//    Date accessed:20/10/2025