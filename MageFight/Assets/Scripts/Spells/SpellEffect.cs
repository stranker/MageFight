using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect : MonoBehaviour {

    public float duration;
    public enum EffectType
    {
        Burn,
        Freeze
    }
    public EffectType type;
    public GameObject spriteEffect;

    public void ApplyEffect(GameObject enemy)
    {
        if (enemy.tag == "Player")
        {
            SpellStateManager player = enemy.GetComponent<SpellStateManager>();
            SetEffectOnPlayer(player);
            GameObject e = Instantiate(spriteEffect, enemy.transform.position + new Vector3(0,0.5f), Quaternion.identity, enemy.transform);
            e.GetComponent<SpellEffectDestroyer>().SetTimer(duration);
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
            default:
                break;
        }
    }
}
