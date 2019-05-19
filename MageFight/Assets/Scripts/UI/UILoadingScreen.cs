using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UILoadingScreen : MonoBehaviour
{
    public Text loadingText;
    public Text pressText;
    public bool onGame = false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetVisible(bool show)
    {
        gameObject.SetActive(show);
    }

    public void Update()
    {
        int loadingVal = (int)(LoaderManager.Get().loadingProgress * 100);
        loadingText.text = "Loading " + loadingVal;
    }

    public void SetOnGame()
    {
        onGame = true;
        loadingText.gameObject.SetActive(false);
        pressText.gameObject.SetActive(true);
    }

    public void OnAnimationFinished()
    {
        Destroy(this.gameObject);
    }

}