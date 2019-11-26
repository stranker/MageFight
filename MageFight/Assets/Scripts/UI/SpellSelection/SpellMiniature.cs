using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellMiniature : MonoBehaviour
{
    [SerializeField] public Image spellArtwork;
    [SerializeField] private Animator anim;
    [SerializeField] private bool onRecycle = false;

    public void Appear()
    {
        anim.SetTrigger("Appear");
    }

    public void SelectForRecycle()
    {
        onRecycle = !onRecycle;
        anim.SetBool("Recycle", onRecycle);
    }

    public void SetSpell(Spell spell)
    {
        gameObject.SetActive(true);
        spellArtwork.sprite = spell.spellData.spellArtwork;
    }
}
