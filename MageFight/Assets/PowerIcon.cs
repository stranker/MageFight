using UnityEngine;
using UnityEngine.UI;

public class PowerIcon : MonoBehaviour {

	[SerializeField][Range(1,3)] private int skillOrder;
	private Image img;

	void Awake(){
		img = GetComponent<Image>();
		img.enabled = false;
	}
	public void SetSpell(Spell Spell){
		SpriteRenderer sr = Spell.GetComponent<SpriteRenderer>();
		if(!sr){ sr = Spell.GetComponentInChildren<SpriteRenderer>();}
		img.sprite = sr.sprite;
		img.color = sr.color;
		img.enabled = true;
	}
	public void Reset(){
		img.enabled = false;
	}
	public int GetSkillOrder(){
		return skillOrder;
	}
}
