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

	[SerializeField] private int roundCounter;
	public List<WizardBehavior> activeWizardList = new List<WizardBehavior>();
	public List<WizardBehavior> deadWizards = new List<WizardBehavior>();
    [SerializeField] public int roundsToWin;
	[SerializeField] private List<Transform> startingPositions = new List<Transform>();

    private int posIdx = 0;
    private float timeScale;
    public new CameraController camera;
    private float playerDeathTimer;
    public float playerDeathTime;
    private bool playerDead = false;
    private bool checkingPlayerDead = false;
    public LevelBehavior currentMap;
    public GameObject playersParent;
    public Dictionary<int, Player> playerDict = new Dictionary<int, Player>();
    public List<Player> playerList = new List<Player>();
    private WizardBehavior lastWinner = null;

    void Awake () {
        AddPlayers();
        CreateWizards();
        InitializeRound();
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

        if (Input.GetKeyDown(KeyCode.F5))
        {
            activeWizardList[0].TakeDamage(900, Vector2.zero);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            activeWizardList[1].TakeDamage(900, Vector2.zero);
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
        deadWizards.Clear();
        lastWinner = null;
        roundCounter++;
        checkingPlayerDead = false;
        int count = 0;
        foreach (WizardBehavior wizard in activeWizardList)
        {
            wizard.Pause();
            wizard.Reset(startingPositions[count].position);
            count++;
        }
	}

    public void StartRound(){
        foreach (WizardBehavior wizard in activeWizardList)
        {
            wizard.Resume();
        }
    }


    public void PlayerDeath(WizardBehavior wizard)
    {
        playerDead = true;
        deadWizards.Insert(deadWizards.Count,wizard);
    }

    public void CheckPlayersInRound()
    {
        if (!checkingPlayerDead)
        {
            checkingPlayerDead = true;
            WizardBehavior winner = null;
            if (lastWinner == null)
            {
                foreach (WizardBehavior wizard in activeWizardList)
                {
                    if (wizard.isAlive)
                    {
                        winner = wizard;
                        camera.SetTarget(winner.transform);
                    }
                }
                if (winner == null)
                {
                    winner = deadWizards[1];
                }
            }
            lastWinner = winner;
            GameplayManager.Get().SendEvent(GameplayManager.Events.PlayerDead);
        }

    }

	public void EndGame(){
		roundCounter = 0; //Reset round counter
        deadWizards.Clear();
        foreach (Player player in playerList)
        {
            player.Reset();
        }
        var spellsInGame = GameObject.FindGameObjectsWithTag("Spell");
        foreach (GameObject spell in spellsInGame)
        {
            Destroy(spell.gameObject);
        }
    }

	public void EndRound(){
        lastWinner.playerRef.AddWin();
        UIManager.Get().ShowLeaderboard();
        foreach (WizardBehavior wizard in activeWizardList)
        {
            wizard.Pause();
        }
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
