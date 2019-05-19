using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviour
{
    #region singleton
    private static LoaderManager instance;
    public static LoaderManager Get()
    {
        return instance;
    }
    public virtual void Awake()
    {
        if (instance == null)
        {            
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public float loadingProgress;
    public float timeLoading;
    public float minTimeToLoad = 2;
    private Scene currentScene;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("LoadingScreen");
        StartCoroutine(AsynchronousLoad(sceneName));
    }
    public void LoadSceneQuick(string sceneName) {
        SceneManager.LoadScene("LoadingScreen");
        minTimeToLoad = 0.5f;
        StartCoroutine(AsynchronousLoad(sceneName));
    }
    IEnumerator AsynchronousLoad(string scene)
    {        
        loadingProgress = 0;
        timeLoading = 0;
        yield return null;
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;
        while (!ao.isDone)
        {
            timeLoading += Time.deltaTime;
            loadingProgress = ao.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minTimeToLoad;
            // Loading completed
            if (loadingProgress >= 1)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
