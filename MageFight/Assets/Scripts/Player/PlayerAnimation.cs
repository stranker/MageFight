using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    //public AttackBehavior attack;
    public PlayerMovement movement;
    public AttackBehavior attack;
    //public SpellsManager sm;
    //private Rigidbody2D rd;
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
        //if (attack.isHolding)
        //{
        //    anim.SetBool("InvokeSpell", attack.invoking);
        //    anim.SetBool("HoldingSpell", attack.isHolding);
        //}

    }
}
