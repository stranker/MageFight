﻿using UnityEngine;

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

    [Header("Spell Stats")]
    public SpellDataScriptable spellData;
    public float castVelocity;
    public int damage;
    public float cooldown;
    public CastType castType;
    public GameObject mageOwner;
    public SpellType typeOfSpeel;
    public Vector2 knockbackForce;

    public bool invoked = false;
    public bool canDamage = true;
    protected Vector3 dir;
    public float timer = 0f;

    public abstract void InvokeSpell(Vector3 startPos, Vector3 direction, GameObject owner);
    public CastType GetCastType() { return castType; }
    public SpellType GetSpellType() { return typeOfSpeel; }

    public GameObject particlesExplosion;
    public Color spellColor;

    public float shakeFactor = 0;

    protected void Update()
    {
        if (invoked)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                invoked = false;
                timer = cooldown;
                Kill();
            }
        }
    }

    public void Kill()
    {
        transform.position = new Vector2(Random.Range(-2000,-999), Random.Range(-2000, -999));
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    protected void CheckHasEffect(WizardBehavior player)
    {
        SpellEffect effect = GetComponentInChildren<SpellEffect>();
        if (effect)
        {
            effect.ApplyEffect(player.gameObject);
        }
    }

    public void SpellShake()
    {
        CameraController.Get().SpellShake(shakeFactor);
    }

    protected void MakeExplosion()
    {
        Instantiate(particlesExplosion, transform.position, Quaternion.identity, transform.parent);
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
        return spellData.spellEffect;
    }
}
