using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New CharacterSelectionData", menuName = "CharacterSelection")]
public class CharacterSelection : ScriptableObject
{

    public string characterName;
    public string description;

    public Sprite artwork;

}
