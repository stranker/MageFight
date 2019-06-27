using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerButtonScript : MonoBehaviour, IPointerEnterHandler{

	private Spell spell;

	public void PickSpell(){
		GetComponent<Button>().interactable = false;
		PowerPickingManager.Instance.SelectPower(spell);
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

	public Spell GetSpell(){
		return spell;
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Get().SetSpellDescription(spell);
    }

}