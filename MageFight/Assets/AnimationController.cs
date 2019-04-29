using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private MovementBehavior movement;
    private AttackBehavior attack;
    private Animator anim;

	// Use this for initialization
	void Start () {
        movement = transform.parent.GetComponent<MovementBehavior>();
        attack = transform.parent.GetComponent<AttackBehavior>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Invoke",attack.onAttackMode);
        anim.SetFloat("Velocity", Mathf.Abs(movement.velocity.x));
	}
}
