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
    public new CameraController camera;
	
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
	}
    private void Start()
    {
        UIManager.Get().OnLeaderboardShown += OnLeaderboardShown;
		PowerPickingManager.Instance.SetPlayerList(players);
        GameplayManager.Get().SendEvent(GameplayManager.Events.StartGame);
    }

    public void InitializeRound(){
		for(int i = 0; i < players.Count; i++){
			if(i <= startingPositions.Count -1){
                players[i].Pause();
                players[i].Reset(startingPositions[i].position); 
			}
		}

		Debug.Log("Begin round: " + (roundCounter +1));
	}
    public void StartRound(){
        for(int i = 0; i < players.Count; i++)
        {
            players[i].Resume();
        }
    }

	public void PlayerDeath(){ //checks if round should end
        Time.timeScale = 0f;
        bool isOnePlayerAlive = false;
		int winner = -1; //Has to have a default value to not cause compiler errors
		for(int i = 0; i < players.Count; i++){
			if(players[i].isAlive == true){
				if(!isOnePlayerAlive){
                    isOnePlayerAlive = true;
                    winner = i;
                    camera.SetTarget(players[i].transform);
                    GameplayManager.Get().SendEvent(GameplayManager.Events.PlayerDead);
                } //finds the first 'alive' player
				else {return;} //if more than one player is alive, the round continues
			}
		}
		//if only one player is alive, then that player wins the round
		if(isOnePlayerAlive)
			players[winner].winCount+=1;
	}

	public void EndGame(){
		roundCounter = 0; //Reset round counter
		for(int i = 0; i < players.Count; i++){
			players[i].winCount = 0;
			players[i].ResetAnimation();
		}		
        PowerPickingManager.Instance.Begin();
    }

	public void EndRound(){
        if(players[0].GetPlayerName() > players[1].GetPlayerName()){
            UIManager.Get().ShowLeaderboard(players[1].winCount, players[0].winCount);
        } else{
            UIManager.Get().ShowLeaderboard(players[0].winCount, players[1].winCount);
        }
    }
    private void OnLeaderboardShown(UIManager manager)
    {
        //runs at end of round, to update round wins and check if there's a winner
        //Stop gameplay
        roundCounter++;
        camera.Reset();
        InitializeRound();
        int winner = -1; //Has to have a default value to not cause compiler errors
        for(int i = 0; i < players.Count; i++)
        {
            players[i].Pause();
            Debug.Log("Player " + players[i].GetPlayerName() + " has won " + players[i].winCount + " rounds.");
            if(players[i].winCount >= roundsToWin) { winner = i; }
        }
        if(winner > -1)
        {
            GameplayManager.Get().SendEvent(GameplayManager.Events.LeaderboardShownWinner);
            UIManager.Get().ShowPostGame(players[winner].GetPlayerName()); //if a winner is found, the game ends
			players[winner].Win();
        }
        else
        {
            GameplayManager.Get().SendEvent(GameplayManager.Events.LeaderboardShownNoWinner);            
        }
    }
    private void OnDestroy()
    {
        UIManager.Get().OnLeaderboardShown -= OnLeaderboardShown;
    }
}
