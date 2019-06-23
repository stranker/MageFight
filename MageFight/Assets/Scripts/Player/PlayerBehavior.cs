using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    public int playerName = -1;
    public int health;
    public int maxHealth;
    public bool isAlive = true;
    public Transform headPos;
    public DamagePopUp popText;
    public ParticleSystem deathParticles;
    private int playerID;
    public int winCount;
    private SpellsManager spellsManager;
    private MovementBehavior movement;
    public Color playerColor;
    private SpriteRenderer sr;

    private void Awake(){
        winCount = 0;
        spellsManager = GetComponent<SpellsManager>();
        movement = GetComponent<MovementBehavior>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int val, Vector2 position)
    {
        if (isAlive)
        {
            health -= val;
            GameObject pop = Instantiate(popText.gameObject, headPos.position, Quaternion.identity, transform.parent);
            pop.GetComponent<DamagePopUp>().SetDamage(val);
			GetComponent<MovementBehavior>().Knockback(position);
            StopCoroutine("FlickerEffect");
            StartCoroutine("FlickerEffect");
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
    IEnumerator FlickerEffect()
    {
        for(int i = 0; i < 5; i++)
        {
            if(i % 2 == 0)
            {
                sr.material.SetFloat("_FlashAmount", 0.7f);
            }
            else
            {
                sr.material.SetFloat("_FlashAmount", 0f);
            }
            yield return new WaitForSeconds(0.07f);
        }
        sr.material.SetFloat("_FlashAmount", 0);
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
        movement.flyStamina = movement.flyMaxStamina;
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
        return spellsManager.FullSpellInventory();
    }

    public int GetPlayerName(){
        return playerName;
    }
	
	public void Win(){
		GetComponent<Animator>().SetTrigger("Win");
	}
	
	public void ResetAnimation(){
		GetComponent<Animator>().SetTrigger("Reset");
	}    
}

