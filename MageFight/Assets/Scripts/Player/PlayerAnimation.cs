using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public PlayerMovement movement;
    public AttackBehavior attack;
    public SpellStateManager ssm;
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

    public void ThrowSpell()
    {
        anim.SetTrigger("ThrowSpell");
    }
}
