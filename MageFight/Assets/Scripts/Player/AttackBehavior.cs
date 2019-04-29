using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour {

    [Header("Spell Manager")]
    public SpellsManager spellManager;

    public bool onAttackMode = false;

    private InputManager input;
    private MovementBehavior movement;
    public Transform crosshair;
    private float timer;
    public float attackModeTime = 0.5f;

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
            int upDir = (int)Input.GetAxis(input.aimAxisY);
            int fwDir = (int)Input.GetAxis(input.movementAxisX);
            Vector2 aimDirection = new Vector2(fwDir,upDir);
            float crossAngle = Vector2.SignedAngle(Vector2.right, aimDirection);
            crosshair.eulerAngles = new Vector3(crosshair.eulerAngles.x, transform.localScale.x == 1f ? 0 : 180, transform.localScale.x == 1f ? crossAngle : -crossAngle);
        }
    }

    private void GetInput()
    {
        GetSpellsInputs(input.firstSkillButton, 0);
        GetSpellsInputs(input.secondSkillButton, 1);
        GetSpellsInputs(input.thirdSkillButton, 2);
    }

    private void ThrowSpell(int spellIndex)
    {
        if (!onAttackMode)
        {
            onAttackMode = !onAttackMode;
            movement.SetCanMove(true);
            Vector3 dir = crosshair.right * (transform.localScale.x == 1f ? 1 : -1);
            spellManager.InvokeSpell(spellIndex, transform.position, dir);
            print("Throwing spell");
        }
    }

    private void InvokeSpell(int spellIndex)
    {
        if (!onAttackMode)
        {
            onAttackMode = !onAttackMode;
            movement.SetCanMove(false);
            print("Invoking spell");
        }

    }
    private void GetSpellsInputs(String input,int spellIndex)
    {
        if(Input.GetButtonDown(input))
        {
            if(spellManager.GetSpellCastType(spellIndex) == Spell.CastType.OneTap)
            {
                ThrowSpell(spellIndex);
            }
            else if(spellManager.GetSpellCastType(spellIndex) == Spell.CastType.Hold)
            {
                InvokeSpell(spellIndex);
            }
        }
        if (Input.GetButtonUp(input) && spellManager.GetSpellCastType(spellIndex) == Spell.CastType.Hold)
        {
            ThrowSpell(spellIndex);
        }
    }
}
