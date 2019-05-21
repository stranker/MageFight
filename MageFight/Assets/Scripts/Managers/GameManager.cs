using UnityEngine;
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
    private float timer;
    private float frezeeDeathTime = 1f;
    private bool playerDeath = false;
	
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
    private void Start()
    {
        UIManager.Get().OnLeaderboardShown += OnLeaderboardShown;
    }

    private void Update()
    {
        if (playerDeath)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= frezeeDeathTime)
            {
                Time.timeScale = 1;
                timer = 0;
                playerDeath = false;
            }
        }
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

		Debug.Log("Begin round: " + (roundCounter +1));
	}

	public void PlayerDeath(){ //checks if round should end
        playerDeath = true;
        timer = 0;
        Time.timeScale = 0.2f;
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

	public void EndGame(){
		roundCounter = 0; //Reset round counter
		for(int i = 0; i < players.Count; i++){
			players[i].winCount = 0;
		}
		PowerPickingManager.Instance.Reset();
        PowerPickingManager.Instance.Begin();
    }

	private void EndRound(){
        UIManager.Get().ShowLeaderboard(players[0].winCount, players[1].winCount);
    }
    private void OnLeaderboardShown(UIManager manager)
    {
        //runs at end of round, to update round wins and check if there's a winner
        //Stop gameplay
        roundCounter++;
        int winner = -1; //Has to have a default value to not cause compiler errors
        for(int i = 0; i < players.Count; i++)
        {
            players[i].Pause();
            Debug.Log("Player " + (i + 1) + " has won " + players[i].winCount + " rounds.");
            if(players[i].winCount >= roundsToWin) { winner = i; }
        }
        if(winner > -1)
        {
            UIManager.Get().ShowPostGame(winner); //if a winner is found, the game ends            
        }
        else
        {
            PowerPickingManager.Instance.Begin();
        }
    }
    private void OnDestroy()
    {
        UIManager.Get().OnLeaderboardShown -= OnLeaderboardShown;
    }
}
