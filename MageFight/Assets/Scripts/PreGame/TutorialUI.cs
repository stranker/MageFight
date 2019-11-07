using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUI : MonoBehaviour
{
    public Animator anim;

    public void PlayerEnterPortal()
    {
        anim.SetTrigger("Flash");
    }

    public void AllPlayers()
    {
        anim.SetTrigger("All");
    }

    public void MatchBegin()
    {
        SceneManager.LoadScene("SampleLevel");
    }
}
