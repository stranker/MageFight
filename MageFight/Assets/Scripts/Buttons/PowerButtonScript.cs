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
		im.sprite = spell.GetComponent<SpriteRenderer>().sprite;
		im.color = spell.GetComponent<SpriteRenderer>().color;
	}
	public void Reset(){
		GetComponent<Button>().interactable = true;
	}
	public bool IsAvailable(){
		return GetComponent<Button>().interactable;
	}
}
