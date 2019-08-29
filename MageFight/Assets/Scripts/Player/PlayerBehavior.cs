using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    public int playerName = -1;
    public int health;
    public int maxHealth;
    public bool isAlive = true;
    public ParticleSystem deathParticles;
    private int playerID;
    public int winCount;
    public SpellsManager spellsManager;
    public PlayerAnimation pAnim;
    public Color playerColor;

    private void Awake(){
        winCount = 0;
    }

    public void TakeDamage(int val, Vector2 position)
    {
        if (isAlive)
        {
            health -= val;
            StopCoroutine("FlickerEffect");
            StartCoroutine("FlickerEffect");
            spellsManager.CancelMeleeSpells();
            pAnim.ReceiveHit();
            if (health <= 0)
            {
                isAlive = !isAlive;
                GetComponent<AttackBehavior>().enabled = false;
                GetComponent<PlayerMovement>().enabled = false;
                SetSpritesVisibles(false);
                deathParticles.Play();
                GameManager.Instance.PlayerDeath();
            }
        }
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    IEnumerator FlickerEffect()
    {
        for(int i = 0; i < 5; i++)
        {
            if(i % 2 == 0)
            {
                SetFlashAmountSprites(0.7f);
            }
            else
            {
                SetFlashAmountSprites(0);
            }
            yield return new WaitForSeconds(0.07f);
        }
        SetFlashAmountSprites(0);
    }



    private void SetSpritesVisibles(bool v)
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = v;
        }
    }

    private void SetFlashAmountSprites(float amount)
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material.SetFloat("_FlashAmount",amount);
        }
    }

    public void Reset(Vector3 position){
        health = maxHealth;
        isAlive = true;
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
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void Resume(){
        if(isAlive){
            GetComponent<AttackBehavior>().enabled = true;
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = 6;
        }
    }

    public void AddSpell(Spell s){
        spellsManager.AddSpell(s);
    }

    public bool FullSpellInventory(){
        return spellsManager.FullSpellInventory();
    }

    public int GetPlayerName(){
        return playerName;
    }
	
	public void Win(){
        pAnim.WinState();
	}
	
	public void ResetAnimation(){
        pAnim.ResetAnimations();
	}    
}

