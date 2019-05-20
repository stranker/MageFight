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
	[SerializeField] private GameObject powerGrid;
	[Header("Settings")]
	[SerializeField][Range(1,9)] private int powersPerRound;
    private List<PlayerBehavior> players;
	private List<turn> turns = new List<turn>();
	private List<PowerButtonScript> buttons = new List<PowerButtonScript>();
	private int turnCounter;

	private void Awake(){
		for(int i = 0; i < spells.Count; i++){
			GameObject go = Instantiate(powerButtonPrefab) as GameObject;
			go.GetComponent<PowerButtonScript>().SetSpell(spells[i]);
			//go.transform.SetParent(powerGrid.transform);
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
		SetupButtons();
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

	private void SetupButtons(){
		List<PowerButtonScript> pickedButtons = new List<PowerButtonScript>();

		foreach(PowerButtonScript button in buttons){
			if(button.IsAvailable()){ pickedButtons.Add(button);}
			button.gameObject.transform.SetParent(null);
		}

		while (pickedButtons.Count > powersPerRound){
			PowerButtonScript but = pickedButtons[Random.Range(0, pickedButtons.Count -1)];
			pickedButtons.Remove(but);
		}

		foreach(PowerButtonScript button in pickedButtons){
			button.gameObject.transform.SetParent(powerGrid.transform);
		}
	}
}
