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

    public float maxDamage;
    public float maxCooldown;

    public bool isSelected = false;

    private float timer;

    public Color spellNameInitialColor;

    public AnimationCurve curve;

    [SerializeField] private Animator anim;


    public void SetSpell(Spell spell)
    {
        spellNameInitialColor = spellName.color;
        currentSpell = spell;

        UpdateUI();
    }

    private void UpdateUI()
    {
        spellName.text = currentSpell.spellData.spellName;
        spellArtwork.sprite = currentSpell.spellData.spellArtwork;
        damageBar.fillAmount = 0;
        cooldownBar.fillAmount = 0;
        difficultyBar.fillAmount = 0;
    }

    private void Update()
    {
        if (timer <= 1)
        {
            timer += Time.deltaTime * 0.7f;
            timer = timer > 1 ? 1 : timer;
            damageBar.fillAmount = curve.Evaluate(timer) * CalculateDamage(currentSpell.damage);
            cooldownBar.fillAmount = curve.Evaluate(timer) * CalculateCooldown(currentSpell.cooldown);
            difficultyBar.fillAmount = curve.Evaluate(timer) * currentSpell.spellData.spellDifficulty / 5.0f;
        }
    }

    private float CalculateDamage(float damage)
    {
        return damage / maxDamage;
    }

    private float CalculateCooldown(float cd)
    {
        return cd / maxCooldown;
    }

    public void Selected(bool value)
    {
        isSelected = value;
        anim.SetBool("Selected", value);
        spellName.color = isSelected ? Color.white : spellNameInitialColor;
        transform.localScale = isSelected ? Vector3.one * 1.1f : Vector3.one;
    }

    public void Confirm()
    {
        anim.SetTrigger("Confirmed");
    }

    public void OnEndConfirmedAnim()
    {
        Destroy(this.gameObject);
    }

    public void Disappear()
    {
        anim.SetTrigger("Disappear");
    }
}
