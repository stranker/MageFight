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
        cooldownBar.fillAmount = CalculateCooldown(currentSpell.damage);
        difficultyBar.fillAmount = CalculateCooldown(currentSpell.spellData.spellDifficulty);
    }

    private float CalculateDamage(int damage)
    {
        return 1 - Mathf.InverseLerp(minDamage, maxDamage, damage);
    }

    private float CalculateCooldown(int damage)
    {
        return 1 - Mathf.InverseLerp(minCooldown, maxCooldown, damage);
    }
}
