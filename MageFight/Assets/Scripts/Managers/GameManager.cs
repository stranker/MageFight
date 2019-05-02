﻿//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private static GameManager instance;
	public static GameManager Instance{
		get{
			 instance = FindObjectOfType<GameManager>();
            if(instance == null) {
                GameObject go = new GameObject("Game Manager");
                instance = go.AddComponent<GameManager>();
            }
            return instance;

		}
	}

	private int RoundCounter;
	private int playerIDCounter;
	private bool[] PlayerStatus;// true = alive, false = dead
	[SerializeField] bool PlaysOnKeyboard; //Defines wether there are 2 or 4 players
	[SerializeField] int RoundsToWin; //How much of a round-win advantage a player needs to be considered winner.
	private int[] WinCounters;
	[SerializeField] GameObject PowerPickPanel;
	[SerializeField] Transform[] StartingPositions;
	[SerializeField] Text Player1RoundCounter; //PROTOTYPE CODE
	[SerializeField] Text Player2RoundCounter; //PROTOTYPE CODE
	
	void Awake () {
		DontDestroyOnLoad(gameObject); //Single scene, might not be needed
		RoundCounter = 0;
		playerIDCounter = 0;
		int PlayerCount;
		if(PlaysOnKeyboard){ PlayerCount = 2;} else { PlayerCount = 4;} //Determine ammount of players based on input
		PlayerStatus = new bool[PlayerCount];
		WinCounters = new int[PlayerCount];
		for(int i = 0; i < PlayerCount; i++){ //State all players are alive
			PlayerStatus[i] = true;
			WinCounters[i] = 0;
		}
		if(!PowerPickPanel){ PowerPickPanel = FindObjectOfType<Button>().transform.parent.gameObject;} //prototype only code!!!
		PowerPickPanel.SetActive(true);
	}
	public int RegisterPlayerID(){
		int ID = playerIDCounter;
		playerIDCounter++;
		if(playerIDCounter > PlayerStatus.Length){
			Debug.LogError("Error registering player ID, all players slots have been filled.");
			return -1;
		}
		return ID;
	}
	
	public void InitializeRound(){
		for(int i = 0; i < PlayerStatus.Length; i++){ //State all players are alive
			PlayerStatus[i] = true;
		}

		PlayerBehavior[] players = FindObjectsOfType<PlayerBehavior>();
		for(int i = 0; i < PlayerStatus.Length; i++){
			players[i].Reset(StartingPositions[i].position); 
		}
		if(RoundCounter == 0){
		Player1RoundCounter.text = "0"; //PROTOTYPE CODE
		Player2RoundCounter.text = "0"; //PROTOTYPE CODE
		}
		Debug.Log("Begin round: " + (RoundCounter +1));
	}

	public void PlayerDeath(int PlayerID){ //registers player deaths, and checks if round should end
		if(PlayerID >= 0 && PlayerID <= PlayerStatus.Length -1){
			PlayerStatus[PlayerID] = false;
			bool IsOnePlayerAlive = false;
			bool IsAnotherPlayerAlive = false;
			int winner = 0; //Has to have a default value to not cause compiler errors
			for(int i = 0; i < PlayerStatus.Length; i++){
				if(PlayerStatus[i] == true){
					if(!IsOnePlayerAlive){ IsOnePlayerAlive = true; winner = i;} //finds the first 'alive' player
					else {
						IsAnotherPlayerAlive = true; //notes that there is more than one player 'alive'
					}
				}
			}
			if(!IsAnotherPlayerAlive){ //if only one player is alive, then that player wins
				WinCounters[winner]++;
				EndRound();
			}
		} else {
			Debug.LogError("PlayerID " + PlayerID + " is out of PlayerStatus array index.");
		}
	}

	private void EndGame(int winner){
		Debug.Log("Player " + (winner +1) + " has won the match.");
		RoundCounter = 0; //Reset round counter
		for(int i = 0; i < WinCounters.Length; i++){
			WinCounters[i] = 0;
		}
	}

	private void EndRound(){ //runs at end of round, to update round wins and check if there's a winner
		//Stop gameplay
		RoundCounter++;
		int winner = 0; //Has to have a default value to not cause compiler errors
		for(int i = 0; i < WinCounters.Length; i++){
			Debug.Log("Player " + (i + 1) + " has won " + WinCounters[i] + " rounds.");
			if (WinCounters[i] >= RoundsToWin){winner = i +1;}
		}
		Player1RoundCounter.text = WinCounters[0].ToString(); //PROTOTYPE CODE
		Player2RoundCounter.text = WinCounters[1].ToString(); //PROTOTYPE CODE
		if(winner > 0){ EndGame(winner);}
		PowerPickPanel.SetActive(true); //if no winner is found, another round begins
	}
}
