using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehavior : MonoBehaviour {

    public MovementBehavior movement;
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
        anim.SetFloat("VelocityY", movement.rigidBody.velocity.y);
        anim.SetBool("OnAir", !movement.onFloor);
    }

    public void PlayMeleeSpellAnim()
    {
        anim.SetTrigger("MeleeSpell");
    }

    public void PlayRangeSpellAnim()
    {
        anim.SetTrigger("RangeSpell");
    }

    public void ReceiveHit()
    {
        anim.SetTrigger("ReceiveHit");
    }

    public void SpawnSpell()
    {
        attack.SpawnSpell();
    }

    public void SetMageMotion(int i)
    {
        movement.SetMotion(i);
    }

    public void PlayStepSound(AudioEvents.EventsKeys key)
    {
        AkSoundEngine.PostEvent(AudioEvents.eventsIDs[key.ToString()], this.gameObject);
    }

    public void PlayFlySound()
    {
        AkSoundEngine.PostEvent(AudioEvents.eventsIDs[AudioEvents.EventsKeys.Player_Flying.ToString()], this.gameObject);
    }
    public void PlaySpellAnim(Spell spell)
    {
        switch (spell.typeOfSpeel)
        {
            case Spell.SpellType.Melee:
                PlayMeleeSpellAnim();
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
