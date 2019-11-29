using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Image flyBar;
    public Image healthBar;
    public Image playerHead;
    public Image background;
    public GameObject cookiePanel;
    public GameObject wizardTarget;
    private Player player;
    private MovementBehavior movement;
    private WizardBehavior wizard;
    private int playerHealth = 0;
    private float playerStamina = 0;
    public Animator anim;
    public int playerId = 0;
    public int cookies = 0;
    [SerializeField] private GameObject spellIcons;

    // Use this for initialization
    void Start () {
        healthBar.material = Instantiate(healthBar.material);
        player = GameManager.Instance.GetPlayerById(playerId);
        wizardTarget = player.wizardRef;
        wizard = wizardTarget.GetComponent<WizardBehavior>();
        movement = wizardTarget.GetComponent<MovementBehavior>();
        playerHead.sprite = wizard.charData.artwork;
        background.color = wizard.playerColor;
        playerHealth = wizard.maxHealth;
        playerStamina = movement.flyMaxStamina;
        var spellIconsList = spellIcons.GetComponentsInChildren<SpellIcon>();
        foreach (SpellIcon spellIcon in spellIconsList)
        {
            spellIcon.SetInputType(player.inputType);
        }
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
            float playerMaxHealth = wizard.maxHealth;
            healthBar.fillAmount = playerHealth / playerMaxHealth;
            StopCoroutine("FlickerEffect");
            StartCoroutine("FlickerEffect");
            anim.SetTrigger("Damaged");
        }
    }

    // Update is called once per frame
    void Update () {
        UpdateText();
        UpdateCookies();
        anim.SetFloat("Stamina",movement.flyStamina);
	}

    public void ResetUICookies()
    {
        var cookiesInPanel = cookiePanel.GetComponentsInChildren<Image>();
        for (int i = 0; i < GameManager.Instance.roundsToWin; i++)
        {
            cookiesInPanel[i].color = new Color(0,0,0,0);
        }
    }

    private void UpdateCookies()
    {
        if (cookies != player.winRounds)
        {
            cookies = player.winRounds;
            var cookiesInPanel = cookiePanel.GetComponentsInChildren<Image>();
            for (int i = 0; i < cookies; i++)
            {
                cookiesInPanel[i].color = Color.white;
            }
        }
    }

    IEnumerator FlickerEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i % 2 == 0)
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

    private void SetFlashAmountSprites(float amount)
    {
        healthBar.material.SetFloat("_FlashAmount", amount);
    }
}
