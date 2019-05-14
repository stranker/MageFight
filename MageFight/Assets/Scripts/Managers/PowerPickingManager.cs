using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerPickingManager : MonoBehaviour {
	private static PowerPickingManager instance;
	public static PowerPickingManager Instance{
		get{
			instance = FindObjectOfType<PowerPickingManager>();
            if(instance == null) {
                GameObject go = new GameObject("Power Picking Manager");
                instance = go.AddComponent<PowerPickingManager>();
            }
            return instance;

		}
	}

	struct turn{
		public int player;
		public int wins;
	}

	[Header("Spells")]
	[SerializeField] private List<Spell> spells = new List<Spell>();
	[Header("UI")]
	[SerializeField] private GameObject powerPickingPanel;
	[SerializeField] private Text pickTurnText;
	[SerializeField] private GameObject powerButtonPrefab;
	private List<PlayerBehavior> players;
	private List<turn> turns = new List<turn>();
	private List<PowerButtonScript> buttons = new List<PowerButtonScript>();
	private int turnCounter;

	private void Start(){
		for(int i = 0; i < spells.Count; i++){
			GameObject go = Instantiate(powerButtonPrefab) as GameObject;
			Vector3 pos = powerPickingPanel.transform.position;
            if (i <= 4){
                if (i % 2 == 0){
                    pos.x += 30 + 30 * i;
                } else {
                    pos.x -= 30 + 30 * (i -1);
                }
                pos.y += 30;
            } else {
                if (i % 2 == 0) {
                    pos.x += 30 + 30 * (i - 6);
                } else {
                    pos.x -= 30 + 30 * (i - 5);
                }
                pos.y -= 30;
            }
			go.transform.position = pos;
			go.transform.SetParent(powerPickingPanel.transform);
			go.GetComponent<PowerButtonScript>().SetSpell(spells[i]);
			buttons.Add(go.GetComponent<PowerButtonScript>());
		}
	}
	
	public void SetPlayerList(List<PlayerBehavior> playerList){
		players = playerList;
	}
	public void Begin(){
		turnCounter = 0;
		UpdateTurns();
		pickTurnText.text = "Player " + (turns[turnCounter].player + 1).ToString() + ", pick your power.";
		powerPickingPanel.SetActive(true);
	}

	private void End(){
		powerPickingPanel.SetActive(false);
		GameManager.Instance.InitializeRound();
	}

	private void UpdateTurns(){
		turns.Clear();
		for(int i = 0; i < players.Count; i++){
			turn t = new turn();
			t.player = players[i].GetID();
			t.wins = players[i].winCount;
			turns.Add(t);
		}
		if(!turns.TrueForAll(s => s.wins == 0)){
			turns.Sort((s1,s2) => s1.wins.CompareTo(s2.wins));
		}
	}

	public void SelectPower(Spell s){
		players[turns[turnCounter].player].AddSpell(s);
		turnCounter++;
		if(turnCounter >= players.Count){
			End();
			return;
		} else {
			pickTurnText.text = "Player " + (turns[turnCounter].player + 1).ToString() + ", pick your power.";
		}
		bool spellsAvailable = false;
		foreach(PowerButtonScript button in buttons){
			if(button.IsAvailable()){
				spellsAvailable = true;
			}
		}
		bool playerInventoryFull = true;
		foreach(PlayerBehavior player in players){
			if(!player.fullSpellInventory()){
				playerInventoryFull = false;
			}
		}
		if(!spellsAvailable || playerInventoryFull){ End();}

	}
	public void Reset(){
		foreach(PowerButtonScript button in buttons){
			button.Reset();
		}
	}
}
