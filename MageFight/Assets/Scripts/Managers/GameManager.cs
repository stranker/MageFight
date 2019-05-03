//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
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

	private int roundCounter;
	private int playerIDCounter;
	private bool[] playerStatus;// true = alive, false = dead
	[SerializeField] bool playsOnKeyboard; //Defines wether there are 2 or 4 players
	[SerializeField] int roundsToWin; //How much of a round-win advantage a player needs to be considered winner.
	private int[] winCounters;
	[SerializeField] GameObject powerPickPanel;
	[SerializeField] Transform[] startingPositions;
	[SerializeField] Text player1RoundCounter; //PROTOTYPE CODE
	[SerializeField] Text player2RoundCounter; //PROTOTYPE CODE
	
	void Awake () {
		DontDestroyOnLoad(gameObject); //Single scene, might not be needed
		roundCounter = 0;
		playerIDCounter = 0;
		int playerCount;
		if(playsOnKeyboard){ playerCount = 2;} else { playerCount = 4;} //Determine ammount of players based on input
		playerStatus = new bool[playerCount];
		winCounters = new int[playerCount];
		for(int i = 0; i < playerCount; i++){ //State all players are alive
			playerStatus[i] = true;
			winCounters[i] = 0;
		}
		if(!powerPickPanel){ powerPickPanel = FindObjectOfType<Button>().transform.parent.gameObject;} //prototype only code!!!
		powerPickPanel.SetActive(true);
	}
	public int RegisterPlayerID(){
		int ID = playerIDCounter;
		playerIDCounter++;
		if(playerIDCounter > playerStatus.Length){
			Debug.LogError("Error registering player ID, all players slots have been filled.");
			return -1;
		}
		return ID;
	}
	
	public void InitializeRound(){
		for(int i = 0; i < playerStatus.Length; i++){ //State all players are alive
			playerStatus[i] = true;
		}

		PlayerBehavior[] players = FindObjectsOfType<PlayerBehavior>();
		for(int i = 0; i < playerStatus.Length; i++){
			players[i].Reset(startingPositions[i].position); 
		}
		if(roundCounter == 0){
		player1RoundCounter.text = "0"; //PROTOTYPE CODE
		player2RoundCounter.text = "0"; //PROTOTYPE CODE
		}
		Debug.Log("Begin round: " + (roundCounter +1));
	}

	public void PlayerDeath(int playerID){ //registers player deaths, and checks if round should end
		if(playerID >= 0 && playerID <= playerStatus.Length -1){
			playerStatus[playerID] = false;
			bool isOnePlayerAlive = false;
			int winner = 0; //Has to have a default value to not cause compiler errors
			for(int i = 0; i < playerStatus.Length; i++){
				if(playerStatus[i] == true){
					if(!isOnePlayerAlive){ isOnePlayerAlive = true; winner = i;} //finds the first 'alive' player
					else {return;} //if more than one player is alive, the round continues
				}
			}
			//if only one player is alive, then that player wins the round
			if(isOnePlayerAlive){ //check in case of errors
				winCounters[winner]++;
				EndRound();
			} else { Debug.LogError("No players are alive, ID of last death: " + playerID);}
		} else {
			Debug.LogError("PlayerID " + playerID + " is out of PlayerStatus array index.");
		}
	}

	private void EndGame(int winner){
		Debug.Log("Player " + (winner +1) + " has won the match.");
		roundCounter = 0; //Reset round counter
		for(int i = 0; i < winCounters.Length; i++){
			winCounters[i] = 0;
		}
	}

	private void EndRound(){ //runs at end of round, to update round wins and check if there's a winner
		//Stop gameplay
		roundCounter++;
		int winner = 0; //Has to have a default value to not cause compiler errors
		for(int i = 0; i < winCounters.Length; i++){
			Debug.Log("Player " + (i + 1) + " has won " + winCounters[i] + " rounds.");
			if (winCounters[i] >= roundsToWin){winner = i +1;}
		}
		player1RoundCounter.text = winCounters[0].ToString(); //PROTOTYPE CODE
		player2RoundCounter.text = winCounters[1].ToString(); //PROTOTYPE CODE
		if(winner > 0){ EndGame(winner);}
		powerPickPanel.SetActive(true); //if no winner is found, another round begins
	}
}
