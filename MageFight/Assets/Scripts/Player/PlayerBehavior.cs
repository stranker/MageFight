using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {


    public int health;
    public int maxHealth;
    public bool isAlive = true;
    public Transform headPos;
    public DamagePopUp popText;
    public ParticleSystem deathParticles;
    private int playerID;
    public int winCount;
    private SpellsManager spellsManager;
    private void Start(){
        winCount = 0;
        spellsManager = GetComponent<SpellsManager>();
    }

    public void TakeDamage(int val)
    {
        if (isAlive)
        {
            health -= val;
            GameObject pop = Instantiate(popText.gameObject, headPos.position, Quaternion.identity, transform.parent);
            pop.GetComponent<DamagePopUp>().SetDamage(val);
            if (health <= 0)
            {
                isAlive = !isAlive;
                GetComponent<AttackBehavior>().enabled = false;
                GetComponent<MovementBehavior>().enabled = false;
                SetSpritesVisibles(false);
                deathParticles.Play();
                GameManager.Instance.PlayerDeath();
            }
        }
    }

    private void SetSpritesVisibles(bool v)
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = v;
        }
    }

    public void Reset(Vector3 position){
        health = maxHealth;
        isAlive = true;
        GetComponent<AttackBehavior>().enabled = true;
        GetComponent<MovementBehavior>().enabled = true;
        SetSpritesVisibles(true);
        transform.position = position;
    }

    public void RegisterPlayerID(int ID){
        playerID = ID;
        Debug.Log(gameObject + " ID: " + playerID);
    }
    public int GetID(){
        return playerID;
    }
    public void Pause(){
        GetComponent<AttackBehavior>().enabled = false;
        GetComponent<MovementBehavior>().enabled = false;
    }
    public void Resume(){
        if(isAlive){
            GetComponent<AttackBehavior>().enabled = true;
            GetComponent<MovementBehavior>().enabled = true;
        }
    }

    public void AddSpell(Spell s){
        spellsManager.AddSpell(s);
    }

    public bool fullSpellInventory(){
        return spellsManager.fullSpellInventory();
    }
}
