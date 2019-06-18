using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellStateManager : MonoBehaviour
{

    private float freezeTimer;
    private float burnTimer;
    private float burnAttackTimer;
    private float burnAttackTime = 0.5f;
    public bool isFreezed;
    public bool isBurned;
    private MovementBehavior movement;
    private AttackBehavior attack;
    private PlayerBehavior player;

    private void Start()
    {
        movement = GetComponent<MovementBehavior>();
        attack = GetComponent<AttackBehavior>();
        player = GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFreezed)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                isFreezed = !isFreezed;
                movement.Immobilize(true);
                attack.SetCanAttack(true);
            }
        }
        if (isBurned)
        {
            burnAttackTimer += Time.deltaTime;
            if (burnAttackTimer >= burnAttackTime)
            {
                player.TakeDamage(1,new Vector2(UnityEngine.Random.Range(-1,1), UnityEngine.Random.Range(-1, 1)));
                burnAttackTimer = 0;
            }
            burnTimer -= Time.deltaTime;
            if (burnTimer <= 0)
            {
                isBurned = !isBurned;
            }
        }
    }

    public void Freeze(float freezeTime)
    {
        isFreezed = true;
        freezeTimer = freezeTime;
        movement.Immobilize(false);
        attack.SetCanAttack(false);
    }

    public void Burn(float burnTime)
    {
        isBurned = true;
        burnTimer = burnTime;
    }

}