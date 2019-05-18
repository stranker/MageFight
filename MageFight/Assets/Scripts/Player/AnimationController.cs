using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private MovementBehavior movement;
    private AttackBehavior attack;
    private Animator anim;

    // Use this for initialization
    void Start () {
        movement = GetComponent<MovementBehavior>();
        attack = GetComponent<AttackBehavior>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (movement.onFloor)
        {
            anim.SetFloat("VelocityX", Mathf.Abs(movement.velocity.x));
        }
        else
        {
            if (movement.flying)
            {
                anim.SetBool("Fly", movement.flying);

            }
            else
            {
                anim.SetFloat("VelocityY", movement.rd.velocity.y);
            }
        }
	}

    internal void PlayerSpell(Spell.SpellType typeOfSpeel)
    {
        switch (typeOfSpeel)
        {
            case Spell.SpellType.Melee:
                anim.SetTrigger("Melee");
                break;
            case Spell.SpellType.Range:
                anim.SetTrigger("Range");
                break;
            case Spell.SpellType.Utility:
                anim.SetTrigger("Utility");
                break;
            default:
                break;
        }
    }
}
