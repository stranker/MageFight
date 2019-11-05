using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBehavior : MonoBehaviour {

    public int playerName = -1;
    public int health;
    public int maxHealth;
    public bool isAlive = true;
    public ParticleSystem deathParticles;
    private int playerID;
    public SpellsManager spellsManager;
    public PlayerAnimation pAnim;
    public Color playerColor;
    public PlayerOffScreenIndicator pIndicator;
    public AttackBehavior attack;
    public PlayerMovement movement;
    public WizardDataScriptable charData;



    public Player playerRef;

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
                movement.SetActive(false);
                attack.SetActive(false);
                SetSpritesVisibles(false);
                deathParticles.Play();
                pAnim.ResetAnimations();
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

    public void Initialize(Player player)
    {
        playerRef = player;
        playerName = player.playerId;
        charData = player.charData;
        name = charData.wizardName;
        switch (playerName)
        {
            case 1:
                playerColor = Color.red;
                break;
            case 2:
                playerColor = Color.blue;
                break;
            default:
                break;
        }
    }

    public void Reset(Vector3 position){
        health = maxHealth;
        isAlive = true;
        SetSpritesVisibles(true);
        transform.position = position;
    }

    public void Pause(){
        attack.SetActive(false);
        movement.SetActive(false);
    }

    public void Resume(){
        if(isAlive){
            attack.SetActive(true);
            movement.SetActive(true);
            spellsManager.AddSpells(playerRef.spellList);
        }
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

    private void OnBecameInvisible()
    {
        if (pIndicator)
        {
            pIndicator.SetActivated(true);
        }
    }

    private void OnBecameVisible()
    {
        if (pIndicator)
        {
            pIndicator.SetActivated(false);
        }
    }

    public void SetOffScreenIndicator(PlayerOffScreenIndicator indicator)
    {
        pIndicator = indicator;
    }

}