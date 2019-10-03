using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour  
{
    public GameObject mainMenuCanvas;
    public GameObject creditsCanvas;
    public GameObject currentPanel;

    private void Start()
    {
        currentPanel = mainMenuCanvas;
    }

    public void CreditsButtonpressed() {
        creditsCanvas.SetActive(true);
        currentPanel = creditsCanvas;
    }
    public void ExitButtonPressed() {
        Application.Quit();
    }
    public void BackButtonPressed() {
        currentPanel.SetActive(false);
        currentPanel = mainMenuCanvas;
    }

    public void PlayButtonPressed()
    {
        LoaderManager.Get().LoadScene("CharacterSelectionScene");
    }

}
