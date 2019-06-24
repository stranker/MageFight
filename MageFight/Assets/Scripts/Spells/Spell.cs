using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour {

    public enum CastType 
    {
        OneTap,
        Hold,
        Error
    }

    public enum SpellType
    {
        Melee,
        Range,
        Utility
    }

    [Header("Spell Stats")]
    public float castVelocity;
    public int damage;
    public float cooldown;
    public CastType castType;
    public GameObject mageOwner;

    public SpellType typeOfSpeel;

    public bool invoked = false;
    protected Vector3 dir;
    public float timer = 0f;

    public abstract void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner);
    public CastType GetCastType() { return castType; }

    public GameObject particlesExplosion;
    public Color spellColor;

    public bool hasSecondAttack = false;

    protected void Update()
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

    public void Kill()
    {
        transform.position = new Vector2(-999, -999);
    }

    protected void CheckHasEffect(PlayerBehavior player)
    {
        SpellEffect effect = GetComponentInChildren<SpellEffect>();
        if (effect)
        {
            effect.ApplyEffect(player.gameObject);
        }
    }

    protected void MakeExplosion()
    {
        Instantiate(particlesExplosion, transform.position, Quaternion.identity, transform.parent);
    }

    internal bool HasSecondAttack()
    {
        return hasSecondAttack;
    }

    internal void SecondAttack()
    {
        throw new NotImplementedException();
    }
}
