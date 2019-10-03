using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wizard { Pedro, Pablo }

[CreateAssetMenu(fileName = "New CharacterSelectionData", menuName = "CharacterSelection")]
public class CharacterSelection : ScriptableObject
{
    public string characterName;
    public string description;
    public Wizard wizardType;
    public Sprite artwork;

}
