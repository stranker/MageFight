using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSpell : Spell {

    public float travelVelocity;
    protected Rigidbody2D rd;

    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        typeOfSpeel = SpellType.Range;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != mageOwner)
        {
            PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
            player.TakeDamage(damage,transform.position);
            CheckHasEffect(player);
            MakeExplosion();
            Kill();

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
        rd.velocity = travelVelocity * dir;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        timer = cooldown;
    }
}
