﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour {

    [Header("Spell Manager")]
    public SpellsManager spellManager;

    public bool onAttackMode = false;
    private bool canAttack = true;
    private bool isHolding = false;
    private InputManager input;
    private MovementBehavior movement;
    private float timer;
    public float attackModeTime = 0.5f;
    public Vector2 aimDirection;
    public Transform handPos;
    public ParticleSystem invokeParticles;

    // Use this for initialization
    void Start () {
        input = GetComponent<InputManager>();
        movement = GetComponent<MovementBehavior>();
    }

    // Update is called once per frame
    void Update () {
        GetInput();
        if (onAttackMode)
        {
            timer += Time.deltaTime;
            if (timer >= attackModeTime)
            {
                onAttackMode = !onAttackMode;
                timer = 0;
            }
        }
    }

    private void GetInput()
    {
        int upDir = (int)Input.GetAxis(input.aimAxisY);
        int fwDir = Mathf.Abs(Input.GetAxis(input.movementAxisX)) > 0.1f ? (int)transform.localScale.x : 0;
        aimDirection = new Vector2(fwDir == 0 && upDir == 0 ? transform.localScale.x : fwDir, upDir);
        GetSpellsInputs(input.firstSkillButton, 0);
        GetSpellsInputs(input.secondSkillButton, 1);
        GetSpellsInputs(input.thirdSkillButton, 2);
    }

    private void ThrowSpell(int spellIndex)
    {
        if ((isHolding || !onAttackMode) && canAttack)
        {
            isHolding = false;
            onAttackMode = !onAttackMode;
            Vector3 dir = aimDirection.normalized;
            spellManager.InvokeSpell(spellIndex, handPos.position, dir, gameObject);
            invokeParticles.Stop();
            print("Throwing spell");
        } else{ Debug.Log("not throwing "+isHolding + " " + !onAttackMode + " " + canAttack);}
    }

    private void InvokeSpell(int spellIndex)
    {
        if (!onAttackMode && canAttack)
        {
            isHolding = true;
            onAttackMode = !onAttackMode;
            invokeParticles.Play();
            print("Invoking spell");
        }

    }
    private void GetSpellsInputs(String input,int spellIndex)
    {
        if(Input.GetButtonDown(input))
        {
            if (spellManager.GetSpellCastType(spellIndex) == Spell.CastType.OneTap)
            {
                //Debug.Log("tap");
                ThrowSpell(spellIndex);
            }
            else if (spellManager.GetSpellCastType(spellIndex) == Spell.CastType.Hold)
            {
                //Debug.Log("Hold");
                InvokeSpell(spellIndex);
            }
        }
        if (Input.GetButtonUp(input) && spellManager.GetSpellCastType(spellIndex) == Spell.CastType.Hold)
        {
            //Debug.Log("Release");
            ThrowSpell(spellIndex);
        }
    }

    public void SetCanAttack(bool val)
    {
        canAttack = val;
    }

}
