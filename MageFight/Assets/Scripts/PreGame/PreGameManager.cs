using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameManager : MonoBehaviour
{

    public List<Transform> positions = new List<Transform>();
    public int posIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Player player in CharactersSelected.Instance.playersConfirmed)
        {
            CreateWizard(player.playerId, player.charData.wizardPrefab, player.inputType, player.joistickId);
        }
    }

    private void CreateWizard(int playerId, GameObject wizardPrefab, InputType inputType, int joistickId = -1)
    {
        GameObject wizard = new GameObject();
        wizard = Instantiate(wizardPrefab, positions[posIdx].position, Quaternion.identity, transform.parent);
        wizard.GetComponent<WizardBehavior>().playerName = playerId;
        wizard.GetComponent<InputManager>().SetInput(inputType, playerId, joistickId);
        posIdx++;
    }
}
