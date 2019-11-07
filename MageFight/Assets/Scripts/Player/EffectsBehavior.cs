using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsBehavior : MonoBehaviour
{
    public SpriteRenderer playerIndicator;
    public Text playerId;
    public Animator anim;

    [SerializeField]private WizardBehavior wizard;


    private void Start()
    {
        playerIndicator.color = wizard.playerRef.playerColor;
        playerId.text = "P" + wizard.playerRef.playerId.ToString();
        playerId.color = wizard.playerRef.playerColor;
    }

    public void OnReceiveHit()
    {
        anim.SetTrigger("OnReceiveHit");
    }
}
