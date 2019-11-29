using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UILoadingScreen : MonoBehaviour
{
    public Text loadingText;
    int loadingVal = 0;

    public void SetVisible(bool show)
    {
        gameObject.SetActive(show);
        loadingVal = Random.Range(61, 78);
        loadingText.text = "Loading " + loadingVal;
    }

    public void Update()
    {
        loadingVal += (int)(LoaderManager.Get().loadingProgress * 100);
        loadingVal = Mathf.Clamp(loadingVal, 0, 100);
        loadingText.text = "Loading " + loadingVal;
    }

    public void OnAnimationFinished()
    {
        Destroy(this.gameObject);
    }

}