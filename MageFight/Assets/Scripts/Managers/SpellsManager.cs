using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : MonoBehaviour {

    [Header("Player spells")]
    private List<Spell> spells = new List<Spell>();
    private const int amountOfSpells = 3;
    public GameObject powerIcons; //set to corresponding player's UI "PowerIcons" object from hierarchy
    private List<PowerIcon> icons = new List<PowerIcon>();
    public ParticleSystem spellParticles;
    public AnimationController anim;


   /* void OnValidate()
    {
        if(inventorySpells.Length != amountOfSpells)
        {
            Debug.LogWarning("THE MAX AMOUNT OF SPELLS IS 3 (TRI)");
            Array.Resize(ref inventorySpells, amountOfSpells);
        }
    }*/
    // Use this for initialization
    void Start () {
        /*foreach (Spell item in inventorySpells)
        {
            Spell s = Instantiate(item, transform.parent);
            spells.Add(s);
            s.Kill();
        }*/
        anim = GetComponent<AnimationController>();
        PowerIcon[] pI = powerIcons.GetComponentsInChildren<PowerIcon>();
        foreach(PowerIcon icon in pI){
            icons.Add(icon);
        }
	}

    public void InvokeSpell(int index, Vector3 startPosition, Vector3 direction, GameObject owner)
    {   if(index < spells.Count){
            if (!spells[index].invoked)
            {
                spells[index].InvokeSpell(startPosition, direction, owner);
                spellParticles.Play();
                GetComponent<MovementBehavior>().Knockback();
                anim.PlayerSpell(spells[index].typeOfSpeel);
                icons[index].StartCooldown(spells[index].cooldown);
            }
        } else {
            Debug.LogWarning("Spell not available or out of index");
        }
    }
    public Spell.CastType GetSpellCastType(int index)
    {
        if(spells.Count > index){
            return spells[index].GetCastType();
        } else{
            Debug.LogWarning("GetSpellCastType index out of list range");
            return Spell.CastType.Error;
        }
    }
    public void AddSpell(Spell s){
        if(spells.Count >= amountOfSpells){
            Debug.LogWarning("Spell capacity reached, unable to add new one");
        } else {
            Spell sp = Instantiate(s, transform.parent);
            sp.Kill();
            spells.Add(sp);
            foreach(PowerIcon icon in icons){
                if(spells.Count == icon.GetSkillOrder()){
                    icon.SetSpell(s);
                }
            }
        }
    }
    public bool FullSpellInventory(){
        return spells.Count == amountOfSpells;
    }

    public void Reset(){
        spells.Clear();
        foreach(PowerIcon icon in icons){
            icon.Reset();
        }
    }
}
