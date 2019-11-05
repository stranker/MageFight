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
        wizard.GetComponent<WizardBehavior>().Pause();
        wizard.GetComponent<InputManager>().SetInput(player.inputType, player.playerId, player.joistickId);
        activeWizardList.Add(wizard.GetComponent<WizardBehavior>());
        player.AddWizard(wizard);
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
        roundCounter++;
        int count = 0;
        foreach (WizardBehavior wizard in activeWizardList)
        {
            wizard.Pause();
            wizard.Reset(startingPositions[count].position);
            count++;
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
        foreach (WizardBehavior wizard in activeWizardList)
        {
            if (wizard.isAlive)
            {
                camera.SetTarget(wizard.transform);
                wizard.playerRef.winRounds += 1;
                GameplayManager.Get().SendEvent(GameplayManager.Events.PlayerDead);
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
        camera.Reset();
        InitializeRound();
        CheckIfAPlayerIsWinner();
    }

    private void CheckIfAPlayerIsWinner()
    {
        bool winner = false;
        foreach (Player player in playerList)
        {
            if (player.winRounds >= roundsToWin)
            {
                UIManager.Get().ShowPostGame(player.playerId);
                winner = true;
                break;
            }
        }
        if (winner)
            GameplayManager.Get().SendEvent(GameplayManager.Events.LeaderboardShownWinner);
        else
            GameplayManager.Get().SendEvent(GameplayManager.Events.LeaderboardShownNoWinner);
    }

    private void OnDestroy()
    {
        UIManager.Get().OnLeaderboardShown -= OnLeaderboardShown;
    }

    public Player GetPlayerById(int playerId)
    {
        Player playerFound = null;
        foreach (Player player in playerList)
        {
            if (player.playerId == playerId)
            {
                playerFound = player;
                break;
            }
        }
        return playerFound;
    }

}
