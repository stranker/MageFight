using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellMiniature : MonoBehaviour
{
    [SerializeField] public Image spellArtwork;
    [SerializeField] private Animator anim;

    public void SetSpellArtwork(Sprite spr)
    {
        spellArtwork.sprite = spr;
    }

    public void Appear()
    {
        anim.SetTrigger("Appear");

    }
}
