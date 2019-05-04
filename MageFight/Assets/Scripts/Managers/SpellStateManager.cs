using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellStateManager : MonoBehaviour
{

    private float freezeTimer;
    private float burnTimer;
    public bool isFreezed;
    public bool isBurned;
    private MovementBehavior movement;
    private AttackBehavior attack;

    private void Start()
    {
        movement = GetComponent<MovementBehavior>();
        attack = GetComponent<AttackBehavior>();
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
                movement.SetCanMove(true);
                attack.SetCanAttack(true);
            }
        }
        if (isBurned)
        {
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
        movement.SetCanMove(false);
        attack.SetCanAttack(false);
    }

    public void Burn(float burnTime)
    {
        isBurned = true;
        burnTimer = burnTime;
    }

}