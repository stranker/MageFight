using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Data", menuName = "Create Spell Data")]
public class SpellDataScriptable : ScriptableObject
{
    public string spellName;
    public AudioEvents.EventsKeys spellKeyInvoke;
    public AudioEvents.EventsKeys spellKeyHit;
    public Sprite spellArtwork;
    public int spellDifficulty;
    public string spellEffect;
}
