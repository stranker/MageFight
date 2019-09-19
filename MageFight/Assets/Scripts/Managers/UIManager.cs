using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager instance;
    public static UIManager Get() { return instance; }
    private void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
    public delegate void UIManagerActions(UIManager manager);
    public UIManagerActions OnLeaderboardShown;
    public GameObject playerUI;
    public UILeaderboard leaderboard;
    public GameObject PostGameUI;
    public Text playerLabel;
    public Text spellDifficulty;
    private float timer = 0;
    public float leaderboardTime = 3f;
    private bool showLeaderboard = false;
    private bool leaderboardActive = false;
    public PickParticles pickParticles;
    public GameObject countdownPanel;
    private bool onCountdown = false;
    public Text countdownText;
    public Text getReadyText;
    public GameObject PlayerPresentationCanvas;

    public Text nameText;
    public Text dmgText;
    public Text typeText;
    public Text cdText;
    public Text ctText;
    public Text effText;


    public void Fade(float amount)
    {
        var sprites = playerUI.GetComponentsInChildren<Image>();
        foreach (Image image in sprites)
        {
            Color actualColor = image.color;
            actualColor.a = amount;
            image.color = actualColor;
        }
    }

    // Update is called once per frame
    void Update () {
        if (showLeaderboard)
        {
            if (!leaderboardActive)
            {
                if (timer < leaderboardTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    leaderboard.gameObject.SetActive(true);
                    leaderboardActive = true;
                    timer = 0;
                }
            }
            else
            {
                if (timer < leaderboardTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    leaderboard.gameObject.SetActive(false);
                    timer = 0;
                    showLeaderboard = false;
                    OnLeaderboardShown(this);
                    leaderboardActive = false;
                }
            }
        }

        if (onCountdown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                onCountdown = false;
                countdownPanel.SetActive(false);
                GameplayManager.Get().SendEvent(GameplayManager.Events.CountdownEnd);
            }
            if (timer<=1.1f)
            {
                getReadyText.enabled = false;
                countdownText.text = "FIGHT!";
            }
            else
            {
                countdownText.text = Mathf.Floor(timer).ToString("0");
            }
        }
    }

    internal void StartCountdown()
    {
        countdownPanel.SetActive(true);
        getReadyText.enabled = true;
        timer = 3.9f;        
        onCountdown = true;
    }

    public void PlayPickParticles(PlayerBehavior playerBehavior)
    {
        if (pickParticles)
        {
            pickParticles.PlayParticles(playerBehavior);
        }
    }

    public void ShowLeaderboard(int firstPlayerScore, int secondPlayerScore)
    {
        showLeaderboard = true;
        leaderboardActive = false;
        leaderboard.SetScores(firstPlayerScore, secondPlayerScore);
    }
    public void ShowPostGame(int winnerName)
    {
        PostGameUI.SetActive(true);
        playerLabel.text = "PLAYER " + winnerName.ToString();
    }
    public void RematchPressed()
    {
        PostGameUI.SetActive(false);
        GameplayManager.Get().SendEvent(GameplayManager.Events.RematchSelected);
    }

    public void SetSpellDescription(Spell spell)
    {
        nameText.text = spell.spellName;
        dmgText.text = spell.damage.ToString();
        cdText.text = spell.cooldown.ToString();
        ctText.text = spell.GetSpellCastTypeString();
        typeText.text = spell.GetSpellTypeString();
        effText.text = spell.GetEffect();
        spellDifficulty.text = spell.GetDifficulty();
        switch (spell.diff)
        {
            case Spell.Difficulty.Easy:
                spellDifficulty.color = Color.green;
                break;
            case Spell.Difficulty.Medium:
                spellDifficulty.color = Color.yellow;
                break;
            case Spell.Difficulty.Hard:
                spellDifficulty.color = new Color32(0xFF, 0x45, 0x00, 0xFF);
                break;
            case Spell.Difficulty.BeyondMagelike:
                spellDifficulty.color = Color.red;
                break;
            default:
                break;
        }
    }
    public void SetPlayerPresentationUI(bool value)
    {
        if(value)
        {
            PlayerPresentationCanvas.SetActive(true);
            PlayerPresentationCanvas.GetComponent<Animator>().SetTrigger("Start");
        }
        else
            PlayerPresentationCanvas.SetActive(false);
    }
}
