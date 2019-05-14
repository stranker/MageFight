﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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

	private int roundCounter;
	private List<PlayerBehavior> players = new List<PlayerBehavior>();
	[SerializeField] private int roundsToWin; //How much of a round-win advantage a player needs to be considered winner.
	[SerializeField] private List<Transform> startingPositions = new List<Transform>();
	[SerializeField] private Text player1RoundCounter; //PROTOTYPE CODE
	[SerializeField] private Text player2RoundCounter; //PROTOTYPE CODE
	
	void Awake () {
		//DontDestroyOnLoad(gameObject); //Single scene, might not be needed
		roundCounter = 0;
		PlayerBehavior[] activeplayers = FindObjectsOfType<PlayerBehavior>();
		for(int i = 0; i < activeplayers.Length; i++){ //State all players are alive
			if(activeplayers[i].gameObject.activeInHierarchy){
			players.Add(activeplayers[i]);
			activeplayers[i].isAlive = true;
			activeplayers[i].winCount = 0;
			activeplayers[i].RegisterPlayerID(i);
			activeplayers[i].Pause();
			}
		}
		PowerPickingManager.Instance.SetPlayerList(players);
		PowerPickingManager.Instance.Begin();
	}
	
	public void InitializeRound(){
		for(int i = 0; i < players.Count; i++){
			if(i <= startingPositions.Count -1){
				players[i].Reset(startingPositions[i].position); 
				players[i].Resume();
			} else {
				Debug.LogError("Out of starting positions, Players: " + players.Count + ", Positions: " + startingPositions.Count);
			}
		}

		if(roundCounter == 0){
		player1RoundCounter.text = "0"; //PROTOTYPE CODE
		player2RoundCounter.text = "0"; //PROTOTYPE CODE
		}
		Debug.Log("Begin round: " + (roundCounter +1));
	}

	public void PlayerDeath(){ //checks if round should end
		bool isOnePlayerAlive = false;
		int winner = -1; //Has to have a default value to not cause compiler errors
		for(int i = 0; i < players.Count; i++){
			if(players[i].isAlive == true){
				if(!isOnePlayerAlive){ isOnePlayerAlive = true; winner = i;} //finds the first 'alive' player
				else {return;} //if more than one player is alive, the round continues
			}
		}
		//if only one player is alive, then that player wins the round
		if(isOnePlayerAlive){ //check in case of errors
			players[winner].winCount+=1;
			EndRound();
		} else { 
			Debug.LogError("No players are alive");
		}
	}

	private void EndGame(int winner){
		Debug.Log("Player " + (winner + 1) + " has won the match.");
		roundCounter = 0; //Reset round counter
		for(int i = 0; i < players.Count; i++){
			players[i].winCount = 0;
		}
		PowerPickingManager.Instance.Reset();
	}

	private void EndRound(){ //runs at end of round, to update round wins and check if there's a winner
		//Stop gameplay
		roundCounter++;
		int winner = -1; //Has to have a default value to not cause compiler errors
		for(int i = 0; i < players.Count; i++){
			players[i].Pause();
			Debug.Log("Player " + (i + 1) + " has won " + players[i].winCount + " rounds.");
			if (players[i].winCount >= roundsToWin){winner = i;}
		}
		player1RoundCounter.text = players[0].winCount.ToString(); //PROTOTYPE CODE
		player2RoundCounter.text = players[1].winCount.ToString(); //PROTOTYPE CODE
		if(winner > -1){ EndGame(winner);} //if a winner is found, the game ends
		PowerPickingManager.Instance.Begin();
	}
}