using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : Spell {

    private bool canHit = true;
    public Vector2 knockDir;

    public enum MeleeType 
	{
		Punch,
		Whip
	}
	public MeleeType type;

    public MeleeType GetMeleeType() { return type; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != mageOwner && canHit)
        {
            canHit = false;
            PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
            player.TakeDamage(damage, transform.position);
            player.GetComponent<PlayerMovement>().KnockOut(knockDir, knockbackForce);
            CheckHasEffect(player);
            MakeExplosion();
        }
    }

    public override void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner)
    {
        invoked = true;
        mageOwner = owner;
        transform.parent = mageOwner.transform;
        transform.position = startPos;
        canHit = true;
        dir = direction;
        CheckAimDirection();
        timer = cooldown;
        transform.localScale = new Vector2(dir.x, 1);
        SetAnimation();
    }

    private void CheckAimDirection()
    {
        if (dir.x == 0)
        {
            dir.x = mageOwner.GetComponent<PlayerMovement>().currentDirection;
        }
    }

    private void SetAnimation()
	{
        GetComponent<Animator>().SetTrigger(spellName);
    }
}