using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBehavior : MonoBehaviour
{
    public EffectsBehavior effects;
    public GameObject playerName;
    public GameObject body;

    public void ReceiveHit()
    {
        StopCoroutine("FlickerEffect");
        StartCoroutine("FlickerEffect");
        effects.OnReceiveHit();
    }

    private void SetFlashAmountSprites(float amount)
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material.SetFloat("_FlashAmount", amount);
        }
    }

    IEnumerator FlickerEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i % 2 == 0)
            {
                SetFlashAmountSprites(0.7f);
            }
            else
            {
                SetFlashAmountSprites(0);
            }
            yield return new WaitForSeconds(0.07f);
        }
        SetFlashAmountSprites(0);
    }

    public void SetPlayerDead(bool v)
    {
        body.SetActive(!v);
        playerName.SetActive(!v);
        SetFlashAmountSprites(0);
    }
}
