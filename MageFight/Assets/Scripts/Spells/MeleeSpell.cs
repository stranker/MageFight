using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell : Spell {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != mageOwner)
        {
            PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
            player.TakeDamage(damage);
            CheckHasEffect(player);
        }
        if (collision.tag == "Ground")
        {
            Kill();
        }
    }


    public override void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner)
    {
        invoked = true;
        mageOwner = owner;
        transform.position = startPos;
        dir = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        timer = cooldown;
        GetComponent<Animator>().SetTrigger("Invoked");
    }
}
