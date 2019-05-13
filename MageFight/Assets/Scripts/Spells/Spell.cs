using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour {

    public enum CastType 
    {
        OneTap,
        Hold
    }
    [Header("Spell Stats")]
    public float travelVelocity;
    public float castVelocity;
    public int damage;
    public float cooldown;
    public CastType castType;
    public GameObject mageOwner;

    public bool invoked = false;
    protected Vector3 dir;
    public float timer = 0f;

    public abstract void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner);
    public CastType GetCastType() { return castType; }


    private void Update()
    {
        if (invoked)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                invoked = false;
                timer = cooldown;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != mageOwner)
        {
            PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
            player.TakeDamage(damage);
            CheckHasEffect(player);
            Kill();
        }
        if (collision.tag == "Ground")
        {
            Kill();
        }
    }

    public void Kill()
    {
        transform.position = new Vector2(-999, -999);
    }

    private void CheckHasEffect(PlayerBehavior player)
    {
        SpellEffect effect = GetComponentInChildren<SpellEffect>();
        if (effect)
        {
            effect.ApplyEffect(player.gameObject);
        }
    }
}
