using UnityEngine;
using UnityEngine.UI;

public class PowerButtonScript : MonoBehaviour {

	private Spell spell;

	public void PickSpell(){
		PowerPickingManager.Instance.SelectPower(spell);
		GetComponent<Button>().interactable = false;
	}
	public void SetSpell(Spell s){
		spell = s;
		Image im = GetComponent<Image>();
		SpriteRenderer sr = spell.GetComponent<SpriteRenderer>();
		if(!sr){ sr = spell.GetComponentInChildren<SpriteRenderer>();}
		im.sprite = sr.sprite;
		im.color = sr.color;
	}
	public void Reset(){
		GetComponent<Button>().interactable = true;
	}
	public bool IsAvailable(){
		return GetComponent<Button>().interactable;
	}
}
