using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : MonoBehaviour {

    [Header("Player spells")]
    public List<Spell> spells = new List<Spell>();
    public const int amountOfSpells = 3;
    private List<SpellIcon> icons = new List<SpellIcon>();
    public ParticleSystem spellParticles;
    private ParticleSystem.MainModule spellParticlesMain;

    void Start () {
        var playersUIs = GameObject.FindGameObjectsWithTag("PlayerUI");
        foreach (var ui in playersUIs)
        {
            if (ui.GetComponent<PlayerUI>().playerId == GetComponent<WizardBehavior>().playerID)
            {
                SpellIcon[] pI = ui.GetComponentsInChildren<SpellIcon>();
                foreach (SpellIcon icon in pI)
                    icons.Add(icon);
                break;
            }
        }
        //spellParticlesMain = spellParticles.GetComponent<ParticleSystem>().main;
    }

    internal void CancelMeleeSpells()
    {
        if (spells.Count > 0)
        {
            foreach (Spell sp in spells)
            {
                if (sp.GetSpellType() == Spell.SpellType.Melee)
                {
                    sp.Kill();
                }
            }
        }
    }

    public void ThrowSpell(int index, Vector3 startPosition, Vector3 direction, GameObject owner) {
        if(index < spells.Count){
            if (spells[index] && !spells[index].invoked)
            {
                if (spells.Count > 0)
                {
                    spells[index].InvokeSpell(startPosition, direction, owner);
                }
                //spellParticlesMain.startColor = spells[index].spellColor;
                //spellParticles.Play();
                if (icons.Count > 0)
                    icons[index].StartCooldown(spells[index].cooldown);
            }
        }
    }

    public Spell GetSpellByIdx(int idx)
    {
        return spells[idx];
    }

    public void AddSpell(Spell spell){
        
        if(spells.Count < amountOfSpells){
            Spell sp = Instantiate(spell, transform.parent);
            sp.Kill();
            spells.Add(sp);
            foreach(SpellIcon icon in icons){
                if(icon.GetSkillOrder() == spells.Count){
                    icon.SetSpell(spell);
                }
            }
        }
    }

    public bool FullSpellInventory(){
        return spells.Count == amountOfSpells;
    }

    public Spell.CastType GetSpellCastType(int spellIndex)
    {
        return spells[spellIndex].GetCastType();
    }

    public void Reset(){
        spells.Clear();
        foreach(SpellIcon icon in icons){
            icon.Reset();
        }
    }

    public void AddSpells(List<Spell> spellList)
    {
        ClearSpells();
        foreach (Spell spell in spellList)
        {
            AddSpell(spell);
        }
    }

    private void ClearSpells()
    {
        foreach (Spell spell in spells)
        {
            Destroy(spell.gameObject);
        }
        spells.Clear();
    }

    public Color GetSpellColor(int index)
    {
        return spells[index].spellColor;
    }

    public bool CanInvokeSpell(int spellIndex)
    {
        if(spellIndex < spells.Count)
            return !spells[spellIndex].invoked;
        else
            return false;
    }
}
