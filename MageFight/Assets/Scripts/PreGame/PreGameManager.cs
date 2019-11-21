using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameManager : MonoBehaviour
{

    public List<Transform> positions = new List<Transform>();
    public int posIdx = 0;
    public bool debugMode = false;

    public WizardDataScriptable wizardDebugData1;
    public WizardDataScriptable wizardDebugData2;

    private List<Player> playerDebugList = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {
        if (debugMode)
        {
            CreateDebugPlayers();
        }
        CreateWizards();
    }

    public void CreateDebugPlayers()
    {
        Player p1 = new Player(1, wizardDebugData1, InputType.Keyboard, -1, Color.red);
        Player p2 = new Player(2, wizardDebugData2, InputType.Joystick, 1, Color.blue);
        playerDebugList.Add(p1);
        playerDebugList.Add(p2);
        CharactersSelected chars = GameObject.Find("CharactersSelected").GetComponent<CharactersSelected>();
        chars.SetPlayersConfirmed(playerDebugList);
    }

    private void CreateWizards()
    {
        foreach (Player player in CharactersSelected.Instance.playersConfirmed)
        {
            CreateWizard(player);
        }
    }

    private void CreateWizard(Player player)
    {
        GameObject wizard = new GameObject();
        wizard = Instantiate(player.wizardData.wizardPrefab, positions[posIdx].position, Quaternion.identity, transform);
        wizard.GetComponent<WizardBehavior>().Initialize(player);
        wizard.GetComponent<InputManager>().SetInput(player.inputType, player.playerId, player.joistickId);
        player.AddWizard(wizard);
        posIdx++;
    }
}
