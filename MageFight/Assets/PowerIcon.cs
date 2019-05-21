using UnityEngine;
using UnityEngine.UI;

public class PowerIcon : MonoBehaviour {

	[SerializeField][Range(1,3)] private int skillOrder;
	private Image img;
    [SerializeField] private Image background;
    private float timer;
    private float cooldownTime;
    private bool onCooldown = false;

	void Awake(){
		img = GetComponent<Image>();
		img.enabled = false;
	}
	public void SetSpell(Spell Spell){
		SpriteRenderer sr = Spell.GetComponent<SpriteRenderer>();
		if(!sr){ sr = Spell.GetComponentInChildren<SpriteRenderer>();}
		img.sprite = sr.sprite;
        background.sprite = img.sprite;
		img.color = sr.color;
		img.enabled = true;
        background.enabled = true;
	}
	public void Reset(){
		img.enabled = false;
        background.enabled = false;
	}
	public int GetSkillOrder(){
		return skillOrder;
	}

    public void StartCooldown(float time)
    {
        onCooldown = true;
        timer = 0;
        cooldownTime = time;
    }

    private void Update()
    {
        if (onCooldown)
        {
            timer += Time.deltaTime;
            img.fillAmount = timer / cooldownTime;
            if (timer >= cooldownTime)
            {
                onCooldown = false;
                timer = 0;
                img.fillAmount = 1f;
            }
        }
    }

}
