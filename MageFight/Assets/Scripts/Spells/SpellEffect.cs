using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect : MonoBehaviour {

    public float duration;
    public enum EffectType
    {
        Burn,
        Freeze,
        Bubble,
        Pull,
        KnockBack,
        Throw,
        Stun,
        Hammer
    }
    public EffectType type;
    public GameObject spriteEffect;

    public void ApplyEffect(GameObject enemy)
    {
        if (enemy.tag == "Player")
        {
            SpellStateManager player = enemy.GetComponent<SpellStateManager>();
            SetEffectOnPlayer(player);
            if(spriteEffect)
            {
                GameObject e = Instantiate(spriteEffect, enemy.transform.position + new Vector3(0, 0.5f), Quaternion.identity, enemy.transform);
                e.GetComponent<SpellEffectDestroyer>().SetTimer(duration);
            }
        }
    }

    private void SetEffectOnPlayer(SpellStateManager player)
    {
        switch (type)
        {
            case EffectType.Burn:
                player.Burn(duration);
                break;
            case EffectType.Freeze:
                player.Freeze(duration);
                break;
            case EffectType.Pull:
                Vector2 pullDirection = (Vector2)GetComponent<Spell>().mageOwner.transform.position;
                player.Pull(pullDirection);
                break;
            case EffectType.Throw:
                player.Throw(30f);
                break;
            case EffectType.Stun:
                player.Freeze(duration);
                break;
            case EffectType.Bubble:
                GameObject go = Instantiate(new GameObject("Bubble"));
                Bubble b = go.AddComponent<Bubble>();
                b.trapInBubble(player.gameObject.transform.position, duration);
                player.Drag(b.getBubble(), duration);
                break;
            case EffectType.Hammer:
                player.Hammerfall();
                break;
            default:
                break;
        }
    }

    public string GetSpellEffect()
    {
        switch (type)
        {
            case EffectType.Burn:
                return "The target will receive 1 damage per second while on fire";
            case EffectType.Freeze:
                return "The target will freeze and will be immobile";
            case EffectType.Bubble:
                return "The target will be trapped in a bubble";
            case EffectType.KnockBack:
                return "The target will be pushed away from the caster";
            case EffectType.Stun:
                return "The target will be stunned";
            case EffectType.Hammer:
                return "The target will be shrinked";
            case EffectType.Pull:
                return "The target will be pulled towards the caster";
            case EffectType.Throw:
                return "The target will be thrown into the air";
            default:
                return "-";
        }
    }
}
