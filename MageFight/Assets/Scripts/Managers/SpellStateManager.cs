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
    public bool isStuned;
    private PlayerMovement movement;
    private AttackBehavior attack;
    private PlayerBehavior player;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<AttackBehavior>();
        player = GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        FreezeCheck();
        BurnCheck();
        //if (isCursed)
        //{
        //    curseTimer -= Time.deltaTime;
        //    if(curseTimer <= 0)
        //    {
        //        player.TakeDamage(curseDamage,Vector2.zero);
        //        curseTimer = 0.0f;
        //        curseDamage = 0;
        //        isCursed = false;
        //    }
        //}
        if(isDragged){
            if(leadingObjectOnDrag){
                //movement.Drag((Vector2)leadingObjectOnDrag.position);
                dragTimer -= Time.deltaTime;
                if(dragTimer<= 0){
                    player.TakeDamage(1,Vector2.zero);
                    isDragged = false;
                    attack.SetCanAttack(true);
                }
            } else {
                dragTimer = 0.0f;
                isDragged = false;
                player.TakeDamage(1,Vector2.zero);
                attack.SetCanAttack(true);
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
                movement.SetCanMove(true);
                attack.SetCanAttack(true);
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

    //private bool isCursed = false;
    //private float curseTimer = 0.0f;
    //private int curseDamage;
    //public void Curse(){
    //    isCursed = true;
    //    curseTimer = UnityEngine.Random.Range(2.0f,5.0f);
    //    int dmgRoll = UnityEngine.Random.Range(0,11); //max range is exclusive for integers
    //    if(dmgRoll == 0) {
    //        curseDamage = 0;
    //    } else if(dmgRoll == 1) {
    //        curseDamage = UnityEngine.Random.Range(3,6);
    //    } else {
    //        curseDamage = UnityEngine.Random.Range(1,3);
    //    }
    //}

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
    }
}