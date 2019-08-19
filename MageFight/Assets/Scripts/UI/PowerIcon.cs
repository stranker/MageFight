using UnityEngine;
using UnityEngine.UI;

public class PowerIcon : MonoBehaviour {

	[SerializeField][Range(1,3)] private int skillOrder;
	private Image powerImage;
    [SerializeField] private Image backgroundSprite;
    private Image circleSprite;
    private float timer;
    private float cooldownTime;
    private bool onCooldown = false;
    private bool onTween = false;
    private Animator anim;

	void Awake(){
		powerImage = GetComponent<Image>();
        circleSprite = transform.parent.GetComponent<Image>();
		powerImage.enabled = false;
        anim = GetComponent<Animator>();
	}
	public void SetSpell(Spell Spell){
		SpriteRenderer sr = Spell.GetComponent<SpriteRenderer>();
		if(!sr){ sr = Spell.GetComponentInChildren<SpriteRenderer>();}
		powerImage.sprite = sr.sprite;
        backgroundSprite.sprite = powerImage.sprite;
		powerImage.color = sr.color;
		powerImage.enabled = true;
        backgroundSprite.enabled = true;
	}
	public void Reset(){
		powerImage.enabled = false;
        backgroundSprite.enabled = false;
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
            powerImage.fillAmount = timer / cooldownTime;
            if (timer >= cooldownTime)
            {                
                onCooldown = false;
                timer = 0;
                powerImage.fillAmount = 1f;
                onTween = true;
                circleSprite.GetComponent<Transform>().localScale = new Vector2(1.5f, 1.5f);
                anim.SetTrigger("isReady");
            }
        }
        if (onTween)
        {            
            timer += Time.deltaTime;
            circleSprite.GetComponent<Transform>().localScale -= new Vector3(Time.deltaTime, Time.deltaTime);
            if (timer >= 0.5f)
            {
                timer = 0;
                onTween = false;
                circleSprite.GetComponent<Transform>().localScale = new Vector2(1, 1);
            }
        }
    }

}
