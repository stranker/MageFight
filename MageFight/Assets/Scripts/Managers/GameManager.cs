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
	[SerializeField] public int roundsToWin;
	[SerializeField] private List<Transform> startingPositions = new List<Transform>();

    private int posIdx = 0;
    private float timeScale;
    public new CameraController camera;
    private float playerDeathTimer;
    public float playerDeathTime;
    public bool playerDead = false;
    public LevelBehavior currentMap;
    public GameObject playersParent;
    public Dictionary<int, Player> playerDict = new Dictionary<int, Player>();
    public List<Player> playerList = new List<Player>();

    void Awake () {
        AddPlayers();
        CreateWizards();
		roundCounter = 0;
	}

    public int GetCurrentRound()
    {
        return roundCounter;
    }

    private void AddPlayers()
    {
        foreach (Player player in CharactersSelected.Instance.playersConfirmed)
        {
            playerDict[player.playerId] = player;
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
        wizard = Instantiate(player.wizardData.wizardPrefab, startingPositions[posIdx].position, Quaternion.identity, playersParent.transform);
        wizard.GetComponent<WizardBehavior>().Initialize(player);
        wizard.GetComponent<WizardBehavior>().Pause();
        wizard.GetComponent<InputManager>().SetInput(player.inputType, player.playerId, player.joistickId);
        activeWizardList.Add(wizard.GetComponent<WizardBehavior>());
        player.AddWizard(wizard);
        posIdx++;
    }

    private void Start()
    {
        GameplayManager.Get().SendEvent(GameplayManager.Events.StartGame);
        timeScale = Time.timeScale;
        UIManager.Get().Initialize();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("StartButton"))
        {
            GameplayManager.Get().SendEvent(GameplayManager.Events.PauseGameplay);
        }
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
                wizard.Pause();
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

    public void CheckEndRound()
    {
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
    }

    public Player GetPlayerById(int playerId)
    {
        return playerDict[playerId];
    }

    public void SetPause(bool value)
    {
        if(value)
            Time.timeScale = 0;
        else
            Time.timeScale = timeScale;
    }
}
