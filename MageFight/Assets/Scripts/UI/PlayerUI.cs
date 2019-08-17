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
    public GameObject playerTarget;
    private PlayerMovement movement;
    private PlayerBehavior player;
    private int playerHealth = 0;
    private float playerStamina = 0;
    public Gradient healthColorRamp;
    public Gradient staminaColorRamp;
    public Animator anim;

    // Use this for initialization
    void Start () {
        player = playerTarget.GetComponent<PlayerBehavior>();
        movement = playerTarget.GetComponent<PlayerMovement>();
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
}
