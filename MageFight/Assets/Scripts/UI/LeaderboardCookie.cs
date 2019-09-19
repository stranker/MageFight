using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardCookie : MonoBehaviour
{

    public Sprite empty;
    public Sprite cookie;
    public Image image;
    public Animator anim;

    public void ChangeToCookie()
    {
        if (image.sprite != cookie)
        {
            image.sprite = cookie;
            anim.SetTrigger("Appear");
        }
    }

}
