using System;
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

    public int selectionIdx = 0;

    private void Start()
    {
        currentPanel = mainMenuCanvas;
    }

    private void OnGUI()
    {
        if ((Input.GetAxis("KBRD_Axis_Y") + Input.GetAxis("J_Vertical"))> 0.5f)
        {
            selectionIdx++;
            UpdateSelection();
        }
    }

    private void UpdateSelection()
    {
        selectionIdx = Mathf.Clamp(selectionIdx, 0, 1);
        
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
