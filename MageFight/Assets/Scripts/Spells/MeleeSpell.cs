using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : Spell {

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
        if (collision.tag == "Player" && collision.gameObject != mageOwner)
        {
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
        transform.position = startPos;
        dir = direction;
        if (type == MeleeType.Punch)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            transform.localScale = new Vector2(owner.transform.localScale.x, 1);
        }
        timer = cooldown;
		SetAnimation();
    }
	
	private void SetAnimation()
	{
		switch(type){
			case MeleeType.Punch:
				GetComponent<Animator>().SetTrigger("Punch");
				break;
			case MeleeType.Whip:
				GetComponent<Animator>().SetTrigger("Whip");
				break;
			default:
				break;
		}
	}
}