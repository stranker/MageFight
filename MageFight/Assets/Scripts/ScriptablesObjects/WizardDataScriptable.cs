using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterSelectionData", menuName = "CharacterSelection")]
public class WizardDataScriptable : ScriptableObject
{
    public string wizardName;
    public GameObject wizardPrefab;
    public Sprite artwork;

}
