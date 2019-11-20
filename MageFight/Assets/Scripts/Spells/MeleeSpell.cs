using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : Spell {

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
        if (collision.tag == "Player" && collision.gameObject != mageOwner && canDamage)
        {
            WizardBehavior player = collision.GetComponent<WizardBehavior>();
            player.TakeDamage(damage, transform.position);
            player.GetComponent<MovementBehavior>().KnockOut(new Vector2(knockDir.x * dir.x, knockDir.y), knockbackForce);
            CheckHasEffect(player);
            MakeExplosion();
            canDamage = false;
            SpellShake();
        }
    }

    public override void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner)
    {
        invoked = true;
        mageOwner = owner;
        transform.parent = mageOwner.transform;
        transform.position = startPos;
        canDamage = true;
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
            dir.x = mageOwner.GetComponent<MovementBehavior>().currentDirection;
        }
    }

    private void SetAnimation()
	{
        GetComponent<Animator>().SetTrigger(spellData.spellName);
    }
}