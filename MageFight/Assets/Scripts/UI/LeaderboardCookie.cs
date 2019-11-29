using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardCookie : MonoBehaviour
{

    public Sprite empty;
    public Image image;
    public Animator anim;

    public void ChangeToCookie()
    {
        if (image.sprite == empty)
        {
            anim.SetTrigger("Appear");
        }
    }

    public void ChangeToEmpty()
    {
        image.sprite = empty;
    }

}
