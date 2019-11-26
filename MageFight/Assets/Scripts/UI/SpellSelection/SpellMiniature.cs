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

    public void CheckRecycle()
    {
        anim.SetBool("Recycle", onRecycle);
        onRecycle = !onRecycle;
    }

    public void SetSpell(Spell spell)
    {
        gameObject.SetActive(true);
        spellArtwork.sprite = spell.spellData.spellArtwork;
        onRecycle = true;
    }

    public void Reset()
    {
        onRecycle = false;
        spellArtwork.sprite = null;
    }
}
