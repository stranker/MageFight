using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterSelectionData", menuName = "Create Spell Data")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public Sprite spellArtwork;
    public int spellDifficulty;
    public string spellEffect;
}
