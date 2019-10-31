using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Data", menuName = "Create Spell Data")]
public class SpellDataScriptable : ScriptableObject
{
    public string spellName;
    public Sprite spellArtwork;
    public int spellDifficulty;
    public string spellEffect;
}
