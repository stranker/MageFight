using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wizard Data", menuName = "Create Wizard Data")]
public class WizardDataScriptable : ScriptableObject
{
    public string wizardName;
    public GameObject wizardPrefab;
    public Sprite artwork;
    public Sprite presentationArtwork;
}
