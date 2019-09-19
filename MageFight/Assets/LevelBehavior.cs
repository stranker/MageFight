using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehavior : MonoBehaviour
{

    public Transform platforms;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.currentMap = this;
    }

    public void GenerateLevel()
    {
        var papitas = platforms.GetComponentsInChildren<FallingPlatform>();
        foreach (FallingPlatform papita in papitas)
        {
            papita.TweenScale();
        }
    }

}
