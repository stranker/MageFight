using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour {

    [Header("Spell Manager")]
    public SpellsManager spellManager;
    public PlayerMovement playerMovement;
    public InputManager input;
    public bool canAttack = true;
    public bool isHolding = false;
    public bool invoking;
    public float timerBeforeAttack;
    public Transform handPos;
    public GameObject arrowSprite;
    public ParticleSystem invokeParticles;
    private ParticleSystem.MainModule invokeParticlesMain;
    // Use this for initialization

    void Start () {
        invokeParticlesMain = invokeParticles.GetComponent<ParticleSystem>().main;
    }

    // Update is called once per frame
    void Update () {
        GetInput();
    }

    private void GetInput()
    {
        float arrowAngle = Mathf.Atan2(playerMovement.attackDirection.y, playerMovement.attackDirection.x) * Mathf.Rad2Deg;
        arrowSprite.transform.rotation = Quaternion.Euler(0,0,arrowAngle);
        GetSpellsInputs(input.firstSkillButton, 0);
        GetSpellsInputs(input.secondSkillButton, 1);
        GetSpellsInputs(input.thirdSkillButton, 2);
    }

    private void ThrowSpell(int spellIndex)
    {
        if (isHolding && canAttack)
        {
            isHolding = false;
            spellManager.InvokeSpell(spellIndex, handPos.position, playerMovement.aimDirection.normalized, gameObject);
            invokeParticles.Stop();
        }
    }

    private void InvokeSpell(int spellIndex)
    {
        if (canAttack)
        {
            isHolding = true;
            //invokeParticlesMain.startColor = spellManager.GetSpellColor(spellIndex);
            invokeParticles.Play();
        }
    }
    private void GetSpellsInputs(String input,int spellIndex)
    {
        if(Input.GetButtonDown(input) && spellManager.CanInvokeSpell(spellIndex))
        {
            if (spellManager.GetSpellCastType(spellIndex) == Spell.CastType.OneTap)
            {
                arrowSprite.gameObject.SetActive(false);
                ThrowSpell(spellIndex);
            }
            else if (spellManager.GetSpellCastType(spellIndex) == Spell.CastType.Hold)
            {
                arrowSprite.gameObject.SetActive(true);
                InvokeSpell(spellIndex);
                invoking = true;
            }
        }
        if (Input.GetButtonUp(input) && spellManager.GetSpellCastType(spellIndex) == Spell.CastType.Hold && invoking)
        {
            arrowSprite.gameObject.SetActive(false);
            ThrowSpell(spellIndex);
            invoking = false;
        }
    }

    public void SetCanAttack(bool val)
    {
        canAttack = val;
    }

}
