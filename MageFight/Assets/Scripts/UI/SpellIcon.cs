using System;
using UnityEngine;
using UnityEngine.UI;

public class SpellIcon : MonoBehaviour {

	[SerializeField][Range(1,3)] private int skillOrder;
	[SerializeField] private Image spellArtwork;
	[SerializeField] private Image cooldown;
    [SerializeField] private Image button;
    private float timer;
    private float cooldownTime;
    private bool onCooldown = false;
    private Animator anim;
    [SerializeField] private Sprite[] joystickButtonImages;
    [SerializeField] private Sprite[] keyboardButtonImages;
    private InputType playerInput;

    void Awake(){
        anim = GetComponent<Animator>();
	}

	public void SetSpell(Spell spell){
		spellArtwork.sprite = spell.spellData.spellArtwork;
        button.sprite = playerInput == InputType.Joystick? joystickButtonImages[skillOrder - 1] : keyboardButtonImages[skillOrder - 1];
        button.gameObject.SetActive(true);
	}

	public void Reset(){
		spellArtwork.sprite = null;
        button.gameObject.SetActive(false);
    }

    public int GetSkillOrder(){
		return skillOrder;
	}

    public void StartCooldown(float time)
    {
        onCooldown = true;
        timer = time;
        cooldownTime = time;
        anim.SetTrigger("Used");
    }

    private void Update()
    {
        if (onCooldown)
        {
            timer -= Time.deltaTime;
            cooldown.fillAmount = timer / cooldownTime;
            if (timer <= 0)
            {                
                onCooldown = false;
                timer = 0;
                cooldown.fillAmount = 0;
                anim.SetTrigger("isReady");
            }
        }
    }

    public void SetInputType(InputType inputType)
    {
        playerInput = inputType;
    }
}
