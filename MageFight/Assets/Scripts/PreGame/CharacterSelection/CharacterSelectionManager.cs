using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum InputType { Joystick = 0, Keyboard, Count }

public class CharacterSelectionManager : MonoBehaviour
{

    private static CharacterSelectionManager instance;
    public static CharacterSelectionManager Instance
    {
        get
        {
            instance = FindObjectOfType<CharacterSelectionManager>();
            if (instance == null)
            {
                GameObject go = new GameObject("CharacterSelectionManager");
                instance = go.AddComponent<CharacterSelectionManager>();
            }
            return instance;

        }
    }

    public Dictionary<int, InputType> players = new Dictionary<int, InputType>();
    public Dictionary<int, string> idPlayerToJoystickName = new Dictionary<int, string>();
    private int currentPlayerId = 1;
    public int maxPlayers;
    public GameObject characterPanel;
    public CharacterSelectionDisplay[] characterSelectionDisplays;
    public CharactersSelected charsSelected;
    public List<Player> playersConfirmed = new List<Player>();

    public GameObject startUI;
    private float canStartTimer = 0;
    private float canStartTime = 1;
    public bool canStart = false;

    // Start is called before the first frame update
    private void Start()
    {
        characterSelectionDisplays = characterPanel.GetComponentsInChildren<CharacterSelectionDisplay>();
        maxPlayers = characterSelectionDisplays.Length;
    }

    private void Update()
    {
        CheckJoystickInput();
        CheckKeyboardInput();
        CheckPlayersConfirmedCount();
    }

    private void CheckPlayersConfirmedCount()
    {
        startUI.SetActive(playersConfirmed.Count > 1);
        if (canStart)
        {
            canStartTimer += Time.deltaTime;
        }
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.JoystickButton10)) && canStartTimer >= canStartTime)
        {
            GoToNextScreen();
        }
    }

    private void GoToNextScreen()
    {
        charsSelected.SetPlayersConfirmed(playersConfirmed);
        SceneManager.LoadScene("PreGameScene");
    }

    private void CheckJoystickInput()
    {
        if (currentPlayerId <= maxPlayers)
        {
            if (Input.GetKey(KeyCode.Joystick1Button0) && !idPlayerToJoystickName.ContainsValue(Input.GetJoystickNames()[0]))
            {
                players.Add(currentPlayerId, InputType.Joystick);
                idPlayerToJoystickName.Add(currentPlayerId, Input.GetJoystickNames()[0]);
                characterSelectionDisplays[currentPlayerId - 1].Initialize(currentPlayerId, players[currentPlayerId], 1);
                currentPlayerId++;
            }
            if (Input.GetKey(KeyCode.Joystick2Button0) && !idPlayerToJoystickName.ContainsValue(Input.GetJoystickNames()[1]))
            {
                players.Add(currentPlayerId, InputType.Joystick);
                idPlayerToJoystickName.Add(currentPlayerId, Input.GetJoystickNames()[1]);
                characterSelectionDisplays[currentPlayerId - 1].Initialize(currentPlayerId, players[currentPlayerId], 2);
                currentPlayerId++;
            }
        }

    }

    private void CheckKeyboardInput()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            if (!players.ContainsValue(InputType.Keyboard) && currentPlayerId <= maxPlayers)
            {
                players.Add(currentPlayerId, InputType.Keyboard);
                characterSelectionDisplays[currentPlayerId - 1].Initialize(currentPlayerId, players[currentPlayerId], 0);
                currentPlayerId++;
            }
        }
    }

    public void AddPlayer(Player player)
    {
        playersConfirmed.Add(player);
        if (playersConfirmed.Count > 1)
        {
            canStart = true;
        }
    }

    internal void RemovePlayer(Player player)
    {
        playersConfirmed.Remove(player);
        if (playersConfirmed.Count < 2)
        {
            canStart = false;
            canStartTimer = 0;
        }
    }
}
