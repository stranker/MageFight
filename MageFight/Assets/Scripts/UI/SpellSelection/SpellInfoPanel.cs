using System;
using UnityEngine;
using UnityEngine.UI;

public class SpellInfoPanel : MonoBehaviour
{

    public Spell currentSpell;

    public Text spellName;
    public Image spellArtwork;
    public Image damageBar;
    public Image cooldownBar;
    public Image difficultyBar;

    public int maxDamage;
    public int minDamage;
    public int maxCooldown;
    public int minCooldown;

    public bool isSelected = false;

    public void SetSpell(Spell spell)
    {
        currentSpell = spell;
        UpdateUI();
    }

    private void UpdateUI()
    {
        spellName.text = currentSpell.spellData.spellName;
        spellArtwork.sprite = currentSpell.spellData.spellArtwork;
        damageBar.fillAmount = CalculateDamage(currentSpell.damage);
        cooldownBar.fillAmount = CalculateCooldown(currentSpell.cooldown);
        difficultyBar.fillAmount = CalculateCooldown(currentSpell.spellData.spellDifficulty);
    }

    private float CalculateDamage(float damage)
    {
        return Mathf.InverseLerp(minDamage, maxDamage, damage);
    }

    private float CalculateCooldown(float cd)
    {
        return 1 - Mathf.InverseLerp(minCooldown, maxCooldown, cd);
    }

    public void Selected(bool value)
    {
        isSelected = value;
        spellName.color = isSelected ? Color.white : Color.red;
        transform.localScale = isSelected ? Vector3.one * 1.1f : Vector3.one;
    }

    public void Confirm()
    {
        Destroy(this.gameObject);
    }
}
