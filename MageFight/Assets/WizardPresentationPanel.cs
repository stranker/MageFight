using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardPresentationPanel : MonoBehaviour
{

    [SerializeField] private int playerId;
    [SerializeField] private Image wizardArtwork;
    [SerializeField] private Text wizardName;
    [SerializeField] private Text playerTextId;
    [SerializeField] private Player player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.GetPlayerById(playerId);
        wizardArtwork.sprite = player.wizardData.presentationArtwork;
        wizardName.text = player.wizardData.wizardName;
        playerTextId.text = "J" + player.playerId.ToString();
    }
}
