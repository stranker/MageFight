using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : MonoBehaviour {

    [Header("Player spells")]
    public List<Spell> spells = new List<Spell>();
    public const int amountOfSpells = 3;
    public int spellIndexToReplace = 0;
    public GameObject powerIcons; //set to corresponding player's UI "PowerIcons" object from hierarchy
    private List<PowerIcon> icons = new List<PowerIcon>();
    public ParticleSystem spellParticles;
    private ParticleSystem.MainModule spellParticlesMain;

    void Start () {
        if (powerIcons)
        {
            PowerIcon[] pI = powerIcons.GetComponentsInChildren<PowerIcon>();
            foreach (PowerIcon icon in pI)
                icons.Add(icon);
        }

        //spellParticlesMain = spellParticles.GetComponent<ParticleSystem>().main;
    }

    public void ThrowSpell(int index, Vector3 startPosition, Vector3 direction, GameObject owner) {
        if(index < spells.Count){
            if (spells[index] && !spells[index].invoked)
            {
                spells[index].InvokeSpell(startPosition, direction, owner);
                //spellParticlesMain.startColor = spells[index].spellColor;
                //spellParticles.Play();
                icons[index].StartCooldown(spells[index].cooldown);
            }
        }
    }
    public Spell.CastType GetSpellCastType(int index)
    {
        return spells[index].GetCastType();
    }

    public void AddSpell(Spell spell){
        
        if(spells.Count < amountOfSpells){
            Spell sp = Instantiate(spell, transform.parent);
            sp.Kill();
            spells.Add(sp);
            foreach(PowerIcon icon in icons){
                if(icon.GetSkillOrder() == spells.Count){
                    icon.SetSpell(spell);
                }
            }
        } else {
            if(spellIndexToReplace >= amountOfSpells){ spellIndexToReplace = 0;}

            spells[spellIndexToReplace].Kill();
            PowerPickingManager.Instance.RecicleSpell(spells[spellIndexToReplace]);

            Spell sp = Instantiate(spell, transform.parent);
            sp.Kill();
            spells[spellIndexToReplace] = sp;

            foreach(PowerIcon icon in icons){
                if(icon.GetSkillOrder() == spellIndexToReplace + 1 ){
                    icon.SetSpell(spell);
                }
            }
            spellIndexToReplace++;
        }
        
    }

    public bool FullSpellInventory(){
        return spells.Count == amountOfSpells;
    }

    public void Reset(){
        spells.Clear();
        spellIndexToReplace = 0;
        foreach(PowerIcon icon in icons){
            icon.Reset();
        }
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
