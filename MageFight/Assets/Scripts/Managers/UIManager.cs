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
    public PlayerBehavior p1;
    public PlayerBehavior p2;
    public Image health1;
    public Image health2;
    public Image stamina1;
    public Image stamina2;
    public GameObject playerUI;
    public UILeaderboard leaderboard;
    public GameObject PostGameUI;
    public Sprite[] playerLabelSprites;
    public Image playerLabel;
    private float timer = 0;
    public float leaderboardTime = 3f;
    private bool showLeaderboard = false;

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
        if (p1)
        {
            health1.fillAmount = (float)p1.health / (float)p1.maxHealth;
            stamina1.fillAmount = (float)p1.GetComponent<MovementBehavior>().flyStamina / (float)p1.GetComponent<MovementBehavior>().flyMaxStamina;
        }
        if (p2)
        {
            health2.fillAmount = (float)p2.health / (float)p2.maxHealth;
            stamina2.fillAmount = (float)p2.GetComponent<MovementBehavior>().flyStamina / (float)p2.GetComponent<MovementBehavior>().flyMaxStamina;
        }


        if(showLeaderboard)
        {
            if(timer < leaderboardTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                leaderboard.gameObject.SetActive(false);
                timer = 0;
                showLeaderboard = false;
                OnLeaderboardShown(this);
            }
        }
    }
    public void ShowLeaderboard(int firstPlayerScore, int secondPlayerScore)
    {
        leaderboard.gameObject.SetActive(true);
        leaderboard.ShowLeaderboard(firstPlayerScore, secondPlayerScore);
        showLeaderboard = true;
    }
    public void ShowPostGame(int winner)
    {
        PostGameUI.SetActive(true);
        playerLabel.sprite = playerLabelSprites[winner];
    }
    public void RematchPressed()
    {
        PostGameUI.SetActive(false);
        GameManager.Instance.EndGame();
    }
}
