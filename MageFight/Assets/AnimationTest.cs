using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour {

    public AttackBehavior attack;
    public MovementBehavior movement;
    public SpellsManager sm;
    private Rigidbody2D rd;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (!movement.onFloor)
        {
            anim.SetFloat("VelocityY", rd.velocity.y);
        }
        else
        {
            anim.SetFloat("VelocityY", 0);
            anim.SetFloat("VelocityX", Mathf.Abs(movement.velocity.x));
        }
    }



}
