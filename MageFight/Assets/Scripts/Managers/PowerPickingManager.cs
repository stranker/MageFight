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
		public int   playerID;
		public int   PlayerName;
		public int   wins;
        public Color playerColor;
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
			buttons.Add(go.GetComponent<PowerButtonScript>());
		}
	}
	
	public void SetPlayerList(List<PlayerBehavior> playerList){
		players = playerList;
	}
	public void Begin(){
		turnCounter = 0;
		if(ShouldContinue()){
			UpdateTurns();
            SetColorAndTextRound(turnCounter);
            powerPickingPanel.SetActive(true);
			SetupButtons();
		} else {
			End();
		}
	}

	private void End(){
		powerPickingPanel.SetActive(false);
		GameManager.Instance.InitializeRound();
	}

	private void UpdateTurns(){
		turns.Clear();
		for(int i = 0; i < players.Count; i++){
			turn t = new turn();
			t.playerID    = players[i].GetID();
			t.PlayerName  = players[i].GetPlayerName();
			t.wins        = players[i].winCount;
            t.playerColor = players[i].playerColor;
			turns.Add(t);
		}
		if(turns.TrueForAll(s => s.wins == 0)){
			turns.Sort((s1,s2) => s1.PlayerName.CompareTo(s2.PlayerName));
		}else{
			turns.Sort((s1,s2) => s1.wins.CompareTo(s2.wins));
		}
	}

	public void SelectPower(Spell s){
		players[turns[turnCounter].playerID].AddSpell(s);
		turnCounter++;
		if(turnCounter >= players.Count){
			End();
			return;
		} else {
            SetColorAndTextRound(turnCounter);
        }
        if(!ShouldContinue()){ End();}

	}
	public void Reset(){
		foreach(PowerButtonScript button in buttons){
			button.Reset();
		}
		SpellsManager[] spellsManagers = FindObjectsOfType<SpellsManager>();
		
		foreach(SpellsManager sm in spellsManagers){
			sm.Reset();
		} 
	}

	private void SetupButtons(){
		List<PowerButtonScript> roundButtons = new List<PowerButtonScript>();

		foreach(PowerButtonScript button in buttons){
			if(button.IsAvailable()){ roundButtons.Add(button);}
			button.gameObject.transform.SetParent(null);
		}

		int difference = roundButtons.Count - powersPerRound;
		for(int i = 0; i < difference; i++ ){
			PowerButtonScript but = roundButtons[Random.Range(0, roundButtons.Count -1)];
			roundButtons.Remove(but);
		}

		foreach(PowerButtonScript button in roundButtons){
			button.gameObject.transform.SetParent(powerGrid.transform);
		}
	}

	private bool ShouldContinue(){
		bool spellsAvailable = false;
		foreach(PowerButtonScript button in buttons){
			if(button.IsAvailable()){
				spellsAvailable = true;
			}
		}
		return spellsAvailable;
	}

	public void RecicleSpell(Spell spell){
		PowerButtonScript[] buttonsInMatrix = powerGrid.GetComponentsInChildren<PowerButtonScript>();
		foreach(PowerButtonScript button in buttonsInMatrix){
			if(!button.IsAvailable()){
				button.SetSpell(spell);
				button.Reset();
				return;
			}
		}
	}
    public void SetColorAndTextRound(int turnIndex)
    {
        pickTurnText.color = turns[turnIndex].playerColor;
        pickTurnText.text = "Player " + turns[turnIndex].PlayerName.ToString() + ", pick your power.";
    }
}
