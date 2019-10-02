using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum InputType {Joystick = 0, Keyboard, Count }
    public Dictionary<int, InputType> players = new Dictionary<int, InputType>();
    public Dictionary<int, string> joystickList = new Dictionary<int, string>();
    public int currentPlayerId = 0;
    private int joystickIdx = 0;
    public int maxPlayers;
    public CharacterSelectionDisplay[] characterSelectionDisplays;
    public int playersConfirmed = 0;

    public GameObject startUI;
    private float canStartTimer = 0;
    private float canStartTime = 1;
    public bool canStart = false;

    // Start is called before the first frame update
    private void Start()
    {
        characterSelectionDisplays = GetComponentsInChildren<CharacterSelectionDisplay>();
        maxPlayers = characterSelectionDisplays.Length;
    }

    private void Update()
    {
        CheckGamepadInput();
        CheckKeyboardInput();
        CheckPlayersConfirmedCount();
    }

    private void CheckPlayersConfirmedCount()
    {
        startUI.SetActive(playersConfirmed > 1);
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
        print("Go to map selection");
    }

    private void CheckGamepadInput()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            if (currentPlayerId < maxPlayers)
            {
                if (joystickIdx < Input.GetJoystickNames().Length && !joystickList.ContainsValue(Input.GetJoystickNames()[joystickIdx]))
                {
                    players.Add(currentPlayerId, InputType.Joystick);
                    joystickList.Add(currentPlayerId, Input.GetJoystickNames()[joystickIdx]);
                    joystickIdx++;
                    characterSelectionDisplays[currentPlayerId].Initialize(currentPlayerId, players[currentPlayerId]);
                    currentPlayerId++;
                }
            }
        }
    }

    private void CheckKeyboardInput()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            if (!players.ContainsValue(InputType.Keyboard) && currentPlayerId < maxPlayers)
            {
                players.Add(currentPlayerId, InputType.Keyboard);
                characterSelectionDisplays[currentPlayerId].Initialize(currentPlayerId, players[currentPlayerId]);
                currentPlayerId++;
            }
        }
    }

    public void AddPlayerConfirm()
    {
        playersConfirmed++;
        canStart = playersConfirmed > 1;
    }

    public void RemovePlayerConfirm()
    {
        playersConfirmed--;
        canStart = false;
        canStartTimer = 0;
    }
}
