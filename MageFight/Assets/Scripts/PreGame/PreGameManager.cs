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
        CreateWizards();
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
