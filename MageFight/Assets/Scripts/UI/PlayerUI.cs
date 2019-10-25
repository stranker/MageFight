using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Text healthText;
    public Text hpText;
    public Text staminaText;
    public Text fsText;
    public Image playerHead;
    public GameObject playerTarget;
    private PlayerMovement movement;
    private WizardBehavior player;
    private SpellsManager playerSpells;
    private int playerHealth = 0;
    private float playerStamina = 0;
    public Gradient healthColorRamp;
    public Gradient staminaColorRamp;
    public Animator anim;
    public int playerId = 0;

    // Use this for initialization
    void Start () {
        playerTarget = GameManager.Instance.GetPlayerById(playerId).gameObject;
        player = playerTarget.GetComponent<WizardBehavior>();
        movement = playerTarget.GetComponent<PlayerMovement>();
        playerSpells = playerTarget.GetComponent<SpellsManager>();
        playerHead.sprite = player.charData.artwork;
        UpdateText();
	}

    private void UpdateText()
    {
        UpdateHealthText();
        UpdateStaminaText();
    }

    private void UpdateStaminaText()
    {
        if (playerStamina != movement.flyStamina)
        {
            playerStamina = movement.flyStamina;
            staminaText.text = playerStamina < 100 ? playerStamina.ToString(".0") : playerStamina.ToString();
            staminaText.color = staminaColorRamp.Evaluate(playerStamina * 0.01f);
            fsText.color = staminaColorRamp.Evaluate(playerStamina * 0.01f);
        }
    }

    private void UpdateHealthText()
    {
        if (playerHealth != player.health)
        {
            playerHealth = player.health;
            healthText.text = playerHealth.ToString();
            healthText.color = healthColorRamp.Evaluate(playerHealth * 0.01f);
            hpText.color = healthColorRamp.Evaluate(playerHealth * 0.01f);
            anim.SetTrigger("Damaged");
        }
    }

    // Update is called once per frame
    void Update () {
        UpdateText();
	}

    public void SetTarget(GameObject wizard)
    {
        playerTarget = wizard;
    }
}
