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
	public List<WizardBehavior> activeWizardList = new List<WizardBehavior>();
	[SerializeField] public int roundsToWin; //How much of a round-win  a player needs to be considered winner.
	[SerializeField] private List<Transform> startingPositions = new List<Transform>();

    private int posIdx = 0;
    public new CameraController camera;
    private float playerDeathTimer;
    public float playerDeathTime;
    public bool playerDead = false;
    public LevelBehavior currentMap;
    public GameObject playersParent;
    public List<Player> playerList = new List<Player>();

    void Awake () {
        AddPlayers();
        CreateWizards();
		roundCounter = 0;
	}

    private void AddPlayers()
    {
        foreach (Player player in CharactersSelected.Instance.playersConfirmed)
        {
            playerList.Add(player);
        }
    }

    private void CreateWizards()
    {
        foreach (Player player in playerList)
        {
            CreateWizard(player);
        }
    }

    private void CreateWizard(Player player)
    {
        GameObject wizard = new GameObject();
        wizard = Instantiate(player.charData.wizardPrefab, startingPositions[posIdx].position, Quaternion.identity, playersParent.transform);
        wizard.GetComponent<WizardBehavior>().Initialize(player);
        wizard.GetComponent<InputManager>().SetInput(player.inputType, player.playerId, player.joistickId);
        activeWizardList.Add(wizard.GetComponent<WizardBehavior>());
        posIdx++;
    }

    private void Start()
    {
        UIManager.Get().OnLeaderboardShown += OnLeaderboardShown;
        GameplayManager.Get().SendEvent(GameplayManager.Events.StartGame);
    }

    private void Update()
    {
        if (playerDead)
        {
            playerDeathTimer += Time.deltaTime;
            if (playerDeathTimer >= playerDeathTime)
            {
                playerDead = false;
                playerDeathTimer = 0;
                CheckPlayersInRound();
            }
        }
    }


    public void InitializeRound(){
		for(int i = 0; i < activeWizardList.Count; i++){
			if(i <= startingPositions.Count -1){
                activeWizardList[i].Pause();
                activeWizardList[i].Reset(startingPositions[i].position); 
			}
		}
		Debug.Log("Begin round: " + (roundCounter +1));
	}

    public void StartRound(){
        foreach (WizardBehavior wizard in activeWizardList)
        {
            wizard.Resume();
        }
    }

	public void PlayerDeath(){ //checks if round should end
        playerDead = true;
	}

    public void CheckPlayersInRound()
    {
        Time.timeScale = 0f;
        bool isOnePlayerAlive = false;
        int winner = -1; //Has to have a default value to not cause compiler errors
        for (int i = 0; i < activeWizardList.Count; i++)
        {
            if (activeWizardList[i].isAlive == true)
            {
                if (!isOnePlayerAlive)
                {
                    isOnePlayerAlive = true;
                    winner = i;
                    camera.SetTarget(activeWizardList[i].transform);
                    GameplayManager.Get().SendEvent(GameplayManager.Events.PlayerDead);
                } //finds the first 'alive' player
                else { return; } //if more than one player is alive, the round continues
            }
        }
        //if only one player is alive, then that player wins the round
        if (isOnePlayerAlive)
        {
            activeWizardList[winner].winCount += 1;
            foreach (WizardBehavior player in activeWizardList)
            {
                player.Pause();
            }
        }
    }

	public void EndGame(){
		roundCounter = 0; //Reset round counter
        foreach (Player player in playerList)
        {
            player.Reset();
        }
    }

	public void EndRound(){
        UIManager.Get().ShowLeaderboard();
    }
    private void OnLeaderboardShown(UIManager manager)
    {
        //runs at end of round, to update round wins and check if there's a winner
        //Stop gameplay
        roundCounter++;
        camera.Reset();
        InitializeRound();
        int winner = -1; //Has to have a default value to not cause compiler errors
        for(int i = 0; i < activeWizardList.Count; i++)
        {
            activeWizardList[i].Pause();
            Debug.Log("Player " + activeWizardList[i].GetPlayerName() + " has won " + activeWizardList[i].winCount + " rounds.");
            if(activeWizardList[i].winCount >= roundsToWin) { winner = i; }
        }
        if(winner > -1)
        {
            GameplayManager.Get().SendEvent(GameplayManager.Events.LeaderboardShownWinner);
            UIManager.Get().ShowPostGame(activeWizardList[winner].GetPlayerName()); //if a winner is found, the game ends
			activeWizardList[winner].Win();
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

    public GameObject GetPlayerById(int playerId)
    {
        GameObject playerFound = null;
        foreach (WizardBehavior player in activeWizardList)
        {
            if (player.playerName == playerId)
            {
                playerFound = player.gameObject;
                break;
            }
        }
        return playerFound;
    }

}
