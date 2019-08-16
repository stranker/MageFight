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
    }


    public override void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner)
    {
        invoked = true;
        mageOwner = owner;
        transform.parent = mageOwner.transform;
        transform.position = startPos;
        canHit = true;
        dir = direction;
        timer = cooldown;
        transform.localScale = new Vector2(dir.x, 1);
        SetAnimation();
    }
	
	private void SetAnimation()
	{
        int splitIndex = gameObject.name.IndexOf('(',0);
        try {
            GetComponent<Animator>().SetTrigger(gameObject.name.Remove(splitIndex));
        } catch {
            splitIndex = "Summoner Slap".IndexOf('(',0);
            GetComponent<Animator>().SetTrigger("Summoner Slap".Remove(splitIndex));
        }
	}
}