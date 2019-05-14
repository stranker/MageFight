using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : MonoBehaviour {

    [Header("Player spells")]
    private const int amountOfSpells = 3;
    public Spell[] inventorySpells = new Spell[amountOfSpells];
    public List<Spell> spells;
    public GameObject clockCooldown;
    private float timer;
    public float clockTime = 1;
    public ParticleSystem spellParticles;
    private Animator anim;

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
        foreach (Spell item in inventorySpells)
        {
            Spell s = Instantiate(item, transform.parent);
            spells.Add(s);
            s.Kill();
        }
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (clockCooldown.activeInHierarchy)
        {
            Vector3 clockScale = new Vector2(transform.localScale.x, 1);
            clockCooldown.transform.localScale = clockScale;
            timer += Time.deltaTime;
            if (timer >= clockTime)
            {
                timer = 0;
                clockCooldown.SetActive(false);
            }
        }
    }

    public void InvokeSpell(int index, Vector3 startPosition, Vector3 direction, GameObject owner)
    {
        if (!spells[index].invoked)
        {
            spells[index].InvokeSpell(startPosition, direction, owner);
            spellParticles.Play();
            anim.SetTrigger("Invoke");
            GetComponent<MovementBehavior>().Knockback();
        }
        else
        {
            clockCooldown.SetActive(true);
            clockCooldown.GetComponentInChildren<TextMesh>().text = spells[index].timer.ToString("0");
        }
    }
    public Spell.CastType GetSpellCastType(int index)
    {
        return inventorySpells[index].GetCastType();
    }
}
