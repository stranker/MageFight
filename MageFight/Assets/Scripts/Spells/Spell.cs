using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour {

    public enum CastType 
    {
        OneTap,
        Hold,
    }

    public enum SpellType
    {
        Melee,
        Range,
        Utility
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        BeyondMagelike
    }

    [Header("Spell Stats")]
    public string spellName;
    public float castVelocity;
    public int damage;
    public float cooldown;
    public CastType castType;
    public Difficulty diff;
    public GameObject mageOwner;
    public SpellType typeOfSpeel;
    public Vector2 knockbackForce;

    public bool invoked = false;
    protected Vector3 dir;
    public float timer = 0f;

    public abstract void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner);
    public CastType GetCastType() { return castType; }
    public SpellType GetSpellType() { return typeOfSpeel; }

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

    public bool HasSecondAttack()
    {
        return hasSecondAttack;
    }

    public void SecondAttack()
    {
        throw new NotImplementedException();
    }

    public string GetSpellTypeString()
    {
        switch (typeOfSpeel)
        {
            case SpellType.Melee:
                return "Melee";
            case SpellType.Range:
                return "Range";
            case SpellType.Utility:
                return "Utility";
            default:
                return "ERROR";
        }
    }

    public string GetSpellCastTypeString()
    {
        switch (castType)
        {
            case CastType.OneTap:
                return "Instant";
            case CastType.Hold:
                return "Hold";
            default:
                return "ERROR";
        }
    }

    public string GetEffect()
    {
        if (spellName == "Summoner Slap")
        {
            return "Humiliation of your rival";
        }
        if (spellName == "Breeze Boomerang")
        {
            return "After 1 sec It comes back!";
        }
        else
        {
            SpellEffect effect = GetComponent<SpellEffect>();
            if (effect)
                return effect.GetSpellEffect();
            else
                return "-";
        }

    }

    public string GetDifficulty()
    {
        switch (diff)
        {
            case Difficulty.Easy:
                return "Easy";
            case Difficulty.Medium:
                return "Medium";
            case Difficulty.Hard:
                return "Hard";
            case Difficulty.BeyondMagelike:
                return "Beyond Magelike";
            default:
                return "ERROR";
        }
    }
}
