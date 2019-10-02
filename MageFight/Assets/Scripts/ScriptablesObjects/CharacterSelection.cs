using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New CharacterSelectionData", menuName = "CharacterSelection")]
public class CharacterSelection : ScriptableObject
{
    public enum CharacterType {Pedro, Pablo}
    public string characterName;
    public string description;
    public CharacterType characterType;
    public Sprite artwork;

}
