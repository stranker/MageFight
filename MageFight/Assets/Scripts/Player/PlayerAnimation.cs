using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public PlayerMovement movement;
    public AttackBehavior attack;
    public SpellStateManager ssm;
    public ParticleSystem cookiesParticles;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        anim.SetBool("Fly", movement.flying);
        anim.SetFloat("VelocityX", Mathf.Abs(movement.velocity.x));
        anim.SetFloat("VelocityY", movement.velocity.y);
        anim.SetBool("InvokeSpell", attack.invoking);
        anim.SetBool("OnAir", !movement.onFloor);
        anim.SetBool("Shrinked", ssm.isShrinked);
    }

    public void ThrowPunch()
    {
        anim.SetTrigger("ThrowPunch");
    }

    public void ThrowWhip()
    {
        anim.SetTrigger("ThrowWhip");
    }

    public void PlayRangeSpellAnim()
    {
        anim.SetTrigger("RangeSpell");
    }

    public void ReceiveHit()
    {
        anim.SetTrigger("ReceiveHit");
    }

    public void PlayMeleeAnim(MeleeSpell.MeleeType type)
    {
        switch (type)
        {
            case MeleeSpell.MeleeType.Punch:
                ThrowPunch();
                break;
            case MeleeSpell.MeleeType.Whip:
                ThrowWhip();
                break;
            default:
                break;
        }
    }

    public void SpawnSpell()
    {
        attack.SpawnSpell();
    }

    public void SetMageMotion(int i)
    {
        movement.SetMotion(i);
    }

    public void PlaySpellAnim(Spell spell)
    {
        switch (spell.typeOfSpeel)
        {
            case Spell.SpellType.Melee:
                MeleeSpell melee = (MeleeSpell)spell;
                PlayMeleeAnim(melee.type);
                break;
            case Spell.SpellType.Range:
                PlayRangeSpellAnim();
                break;
            case Spell.SpellType.Utility:
                break;
            default:
                break;
        }
    }

    public void Shrink()
    {
        anim.SetTrigger("Shrink");
    }

    public void MunchCookie()
    {
        cookiesParticles.Play();
    }

    public void WinState()
    {
        anim.SetTrigger("Win");
    }

    public void ResetAnimations()
    {
        anim.SetTrigger("ResetState");
    }
}
