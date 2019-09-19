using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardCookie : MonoBehaviour
{

    public Sprite empty;
    public Sprite cookie;
    public Image image;

    public void ChangeToCookie()
    {
        image.sprite = cookie;
    }

}
