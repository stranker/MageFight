using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBehavior : MonoBehaviour {

    public int health;
    public int maxHealth;
    public bool isAlive = true;
    public ParticleSystem deathParticles;
    public int playerID;
    public SpellsManager spellsManager;
    public AnimationBehavior pAnim;
    public Color playerColor;
    public PlayerOffScreenIndicator pIndicator;
    public AttackBehavior attack;
    public MovementBehavior movement;
    public WizardDataScriptable charData;
    public VisualBehavior visual;
    public Player playerRef;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TakeDamage(5, Vector2.one);
        }
    }

    public void TakeDamage(int val, Vector2 position)
    {
        if (isAlive)
        {
            health -= val;
            spellsManager.CancelMeleeSpells();
            pAnim.ReceiveHit();
            visual.ReceiveHit();
            if (health <= 0)
            {
                isAlive = !isAlive;
                movement.SetActive(false);
                attack.SetActive(false);
                deathParticles.Play();
                visual.SetPlayerDead(true);
                pAnim.ResetAnimations();
                GameManager.Instance.PlayerDeath();
            }
        }
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void Initialize(Player player)
    {
        playerRef = player;
        playerID = player.playerId;
        charData = player.wizardData;
        name = charData.wizardName;
        playerColor = player.playerColor;
    }

    public void Reset(Vector3 position){
        health = maxHealth;
        isAlive = true;
        visual.SetPlayerDead(false);
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