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
    public Button playButton;
    float timer = 0;
    public float timeToChangeScene = 1f;
    private bool changingToPlay = false;

    private void Start()
    {
        currentPanel = mainMenuCanvas;
    }

    private void Update()
    {
        if (changingToPlay)
        {
            timer += Time.deltaTime;
            if (timer > timeToChangeScene)
            {
                LoaderManager.Get().LoadScene("SampleLevel");
                changingToPlay = false;
                timer = 0;
            }
        }
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
        playButton.GetComponent<AudioSource>().Play();
        changingToPlay = true;
    }

}
