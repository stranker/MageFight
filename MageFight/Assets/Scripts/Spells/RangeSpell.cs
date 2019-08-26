using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSpell : Spell {

    public float speed;
    public float acceleration;
    private float accelerationIncrement = 0;
    private Vector2 velocity;
    protected Rigidbody2D rd;

    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        base.Update();
        if (invoked)
            SpellMovement();
        else
            StopSpell();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != mageOwner)
        {
            PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
            PlayerMovement pMovement = player.GetComponent<PlayerMovement>();
            player.TakeDamage(damage,transform.position);
            pMovement.KnockOut(dir, knockbackForce);
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

    public void SpellMovement()
    {
        accelerationIncrement += Time.deltaTime * acceleration;
        velocity = (speed + accelerationIncrement) * dir;
        rd.velocity = velocity;
    }

    public void StopSpell()
    {
        velocity = Vector2.zero;
        rd.velocity = velocity;
        accelerationIncrement = 0;
    }

    public override void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner)
    {
        invoked = true;
        mageOwner = owner;
        transform.position = startPos;
        dir = direction.normalized;
        velocity = dir * speed;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        timer = cooldown;
    }
}
