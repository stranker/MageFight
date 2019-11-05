using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Text healthText;
    public Image flyBar;
    public Image playerHead;
    public Image background;
    public GameObject cookiePanel;
    public GameObject wizardTarget;
    private Player player;
    private PlayerMovement movement;
    private WizardBehavior wizard;
    private SpellsManager playerSpells;
    private int playerHealth = 0;
    private float playerStamina = 0;
    public Gradient healthColorRamp;
    public Animator anim;
    public int playerId = 0;
    public int cookies = 0;

    // Use this for initialization
    void Start () {
        player = GameManager.Instance.GetPlayerById(playerId);
        wizardTarget = player.wizardRef;
        wizard = wizardTarget.GetComponent<WizardBehavior>();
        movement = wizardTarget.GetComponent<PlayerMovement>();
        playerSpells = wizardTarget.GetComponent<SpellsManager>();
        playerHead.sprite = wizard.charData.artwork;
        background.color = wizard.playerColor;
        UpdateText();
	}

    private void UpdateText()
    {
        UpdateHealthText();
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        if (playerStamina != movement.flyStamina)
        {
            playerStamina = movement.flyStamina;
            float playerMaxStamina = movement.flyMaxStamina;
            flyBar.fillAmount = playerStamina / playerMaxStamina;
        }
    }

    private void UpdateHealthText()
    {
        if (playerHealth != wizard.health)
        {
            playerHealth = wizard.health;
            healthText.text = playerHealth.ToString() + "hp";
            healthText.color = healthColorRamp.Evaluate(playerHealth * 0.01f);
            anim.SetTrigger("Damaged");
        }
    }

    // Update is called once per frame
    void Update () {
        UpdateText();
        UpdateCookies();
	}

    private void UpdateCookies()
    {
        if (cookies != player.winRounds)
        {
            cookies = player.winRounds;
            var cookiesInPanel = cookiePanel.GetComponentsInChildren<Transform>();
            for (int i = 0; i < cookies; i++)
            {
                cookiesInPanel[i+1].gameObject.SetActive(true);
            }
        }
    }
}
