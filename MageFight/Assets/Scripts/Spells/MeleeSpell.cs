using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : Spell {

    private bool canHit = true;

	public enum MeleeType 
	{
		Punch,
		Whip
	}
	public MeleeType type;

    private void Start()
    {
        typeOfSpeel = SpellType.Melee;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != mageOwner && canHit)
        {
            canHit = false;
            PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
            player.TakeDamage(damage, transform.position);
            CheckHasEffect(player);
            MakeExplosion();
        }
        if (collision.tag == "Ground")
        {
            MakeExplosion();
            Kill();
        }
    }


    public override void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner)
    {
        invoked = true;
        mageOwner = owner;
        transform.parent = mageOwner.transform;
        mageOwner.GetComponent<MovementBehavior>().Immobilize(1f, true);
        transform.position = startPos;
        canHit = true;
        dir = direction;

        if (type == MeleeType.Punch)
        {
            transform.localScale = mageOwner.transform.localScale;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (angle >= 180)
                transform.rotation = Quaternion.AngleAxis(180, Vector2.up);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }

        timer = cooldown;
		SetAnimation();
    }
	
	private void SetAnimation()
	{
        int splitIndex = gameObject.name.IndexOf('(',0);
        GetComponent<Animator>().SetTrigger(gameObject.name.Remove(splitIndex));
	}
}