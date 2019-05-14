using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPush : MonoBehaviour {

    private Spell spell;

    private void Start()
    {
        spell = GetComponent<Spell>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && spell.mageOwner != collision.gameObject)
        {
            collision.GetComponent<MovementBehavior>().Knockback();
        }
    }
}
