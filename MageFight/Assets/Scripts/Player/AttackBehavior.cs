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
    public bool aiming = false;
    public bool invoking;
    public float timeAfterAttack;
    private float timerAfterAttack;
    public Transform handPos;
    public GameObject arrowSprite;
    public ParticleSystem invokeParticles;
    public PlayerAnimation anim;
    private ParticleSystem.MainModule invokeParticlesMain;
    public int spellIndex = -1;
    public Vector2 spellDir = Vector2.zero;
    // Use this for initialization

    void Start () {
        invokeParticlesMain = invokeParticles.GetComponent<ParticleSystem>().main;
    }

    // Update is called once per frame
    void Update () {
        GetInput();
        CanAttackCheck();
        UpdateSpellDir();
        UpdateArrow();
    }

    private void UpdateArrow()
    {
        float arrowAngle = Mathf.Atan2(spellDir.y, spellDir.x) * Mathf.Rad2Deg;
        arrowSprite.transform.rotation = Quaternion.Euler(0, 0, arrowAngle);
    }

    private void CanAttackCheck()
    {
        if (!canAttack)
        {
            timerAfterAttack += Time.deltaTime;
            if (timerAfterAttack > timeAfterAttack)
            {
                timerAfterAttack = 0;
                canAttack = !canAttack;
            }
        }
    }

    private void GetInput()
    {

        GetSpellsInputs(input.firstSkillButton, 0);
        GetSpellsInputs(input.secondSkillButton, 1);
        GetSpellsInputs(input.thirdSkillButton, 2);
    }

    private void ThrowSpell(int _spellIndex)
    {
        if (canAttack)
        {
            isHolding = false;
            spellIndex = _spellIndex;
            spellDir = playerMovement.aimDirection;
            anim.PlaySpellAnim(spellManager.GetSpellByIdx(spellIndex));
            invokeParticles.Stop();
            canAttack = !canAttack;
        }
    }

    public void SpawnSpell()
    {
        spellManager.ThrowSpell(spellIndex, handPos.position, spellDir, gameObject);
        spellDir = Vector2.zero;
    }

    private void InvokeSpell(int spellIndex)
    {
        if (canAttack)
        {
            invokeParticles.Play();
            isHolding = true;
            //invokeParticlesMain.startColor = spellManager.GetSpellColor(spellIndex);
        }
    }

    private void UpdateSpellDir()
    {
        if (isHolding)
        {
            aiming = playerMovement.aimDirection != Vector2.zero;
            if (aiming)
                spellDir = playerMovement.aimDirection;
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
