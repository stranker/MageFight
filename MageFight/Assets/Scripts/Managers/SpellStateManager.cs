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
    private float shrinkTimer;
    public bool isFreezed;
    public bool isBurned;
    public bool isStuned;
    public bool isShrinked;
    private PlayerMovement movement;
    private AttackBehavior attack;
    private WizardBehavior player;
    private PlayerAnimation playerAnim;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<AttackBehavior>();
        player = GetComponent<WizardBehavior>();
        playerAnim = GetComponentInChildren<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        FreezeCheck();
        BurnCheck();
        ShrinkCheck();
        if(isDragged){
            if(leadingObjectOnDrag){
                movement.Drag(leadingObjectOnDrag.position);
                dragTimer -= Time.deltaTime;
                if(dragTimer<= 0){
                    player.TakeDamage(1,Vector2.zero);
                    isDragged = false;
                    attack.SetCanAttack(true);
                    movement.SetCanMove(true);
                }
            } else {
                dragTimer = 0.0f;
                isDragged = false;
                player.TakeDamage(10,Vector2.zero);
                attack.SetCanAttack(true);
                movement.SetCanMove(true);
            }
        }
    }

    private void ShrinkCheck()
    {
        if (isShrinked)
        {
            shrinkTimer += Time.deltaTime;
            if (shrinkTimer > 3)
            {
                isShrinked = false;
                shrinkTimer = 0;
                movement.SetCanFly(!isShrinked);
                attack.SetCanAttack(!isShrinked);
            }
        }
    }

    private void BurnCheck()
    {
        if (isBurned)
        {
            burnAttackTimer += Time.deltaTime;
            if (burnAttackTimer >= burnAttackTime)
            {
                player.TakeDamage(1, new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1)));
                burnAttackTimer = 0;
            }
            burnTimer -= Time.deltaTime;
            if (burnTimer <= 0)
            {
                isBurned = !isBurned;
            }
        }
    }

    private void FreezeCheck()
    {
        if (isFreezed)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                isFreezed = !isFreezed;
                movement.stuned = false;
                //movement.SetCanMove(true);
                attack.SetCanAttack(true);
            }
        }
    }


    public void Freeze(float freezeTime)
    {
        isFreezed = true;
        freezeTimer = freezeTime;
        movement.stuned = true;
        attack.SetCanAttack(false);
    }

    public void Burn(float burnTime)
    {
        isBurned = true;
        burnTimer = burnTime;
    }

    public void Pull(Vector2 targetPosition){
        movement.Pull(targetPosition);
    }

    public void Throw(float force){
        movement.Throw(force);
    }

    private bool isDragged = false;
    private float dragTimer = 0.0f;
    private Transform leadingObjectOnDrag;

    public void Drag(Transform leadingObject, float duration){
        isDragged = true;
        leadingObjectOnDrag = leadingObject;
        dragTimer = duration;
        attack.SetCanAttack(false);
        movement.SetCanMove(false);
    }

    public void Shrink()
    {
        isShrinked = true;
        movement.SetCanFly(false);
        attack.SetCanAttack(false);
        playerAnim.Shrink();
    }

    public void Hammerfall()
    {
        Shrink();
    }
}