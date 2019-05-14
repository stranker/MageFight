﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public PlayerBehavior p1;
    public PlayerBehavior p2;
    public Image health1;
    public Image health2;
    public GameObject playerUI;

    internal void Fade(float v)
    {
        var sr = playerUI.GetComponentsInChildren<Image>();
        foreach (Image sprite in sr)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, v);
        }

    }

    // Update is called once per frame
    void Update () {
        health1.fillAmount = (float)p1.health / (float)p1.maxHealth;
        health2.fillAmount = (float)p2.health / (float)p2.maxHealth;
    }


}