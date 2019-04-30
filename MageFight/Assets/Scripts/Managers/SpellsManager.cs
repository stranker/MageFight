using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : MonoBehaviour {

    [Header("Player spells")]
    private const int amountOfSpells = 3;
    public Spell[] inventorySpells = new Spell[amountOfSpells];
    private Spell[] spells;
    void OnValidate()
    {
        if(inventorySpells.Length != amountOfSpells)
        {
            Debug.LogWarning("THE MAX AMOUNT OF SPELLS IS 3 (TRI)");
            Array.Resize(ref inventorySpells, amountOfSpells);
        }
    }
    // Use this for initialization
    void Start () {
        spells = new Spell[3];
	}
	
	// Update is called once per frame
	void Update () {
    }
    public void InvokeSpell(int index, Vector3 startPosition, Vector3 direction, GameObject owner)
    {
        if(!spells[index])
        {
            spells[index] = Instantiate(inventorySpells[index], startPosition, Quaternion.identity);
            spells[index].InvokeSpell(direction,owner);
        }
    }
    public Spell.CastType GetSpellCastType(int index)
    {
        return inventorySpells[index].GetCastType();
    }
}
