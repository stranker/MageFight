using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum InputType { Joystick = 0, Keyboard, Count }

public class WizardSelectionManager : MonoBehaviour
{

    private static WizardSelectionManager instance;
    public static WizardSelectionManager Instance
    {
        get
        {
            instance = FindObjectOfType<WizardSelectionManager>();
            if (instance == null)
            {
                GameObject go = new GameObject("CharacterSelectionManager");
                instance = go.AddComponent<WizardSelectionManager>();
            }
            return instance;

        }
    }

    public Dictionary<int, InputType> players = new Dictionary<int, InputType>();
    public Dictionary<int, string> idPlayerToJoystickName = new Dictionary<int, string>();
    private int currentPlayerId = 1;
    public int maxPlayers = 2;
    public WizardSelectionDisplay[] characterSelectionDisplays;
    public CharactersSelected charsSelected;
    public List<Player> playersConfirmed = new List<Player>();
    private bool keyboardAdded = false;
    private float canStartTimer = 0;
    private float canStartTime = 1;
    public bool canStart = false;

    public GameObject wizardsPanel;
    public WizardSelectionButton[] wizardsSelectionButtons; 

    // Start is called before the first frame update
    private void Start()
    {
        wizardsSelectionButtons = wizardsPanel.GetComponentsInChildren<WizardSelectionButton>();
    }

    private void Update()
    {
        CheckPlayersConfirmedCount();
        CheckJoystickInput();
        CheckKeyboardInput();
        if (Input.GetButton("Cancel"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void CheckPlayersConfirmedCount()
    {
        if (canStart)
        {
            canStartTimer += Time.deltaTime;
        }
        if (playersConfirmed.Count >= maxPlayers && canStart)
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
        if (Input.anyKeyDown)
        {
            Event ev = Event.current;
            if (ev.isKey && !players.ContainsValue(InputType.Keyboard) && currentPlayerId <= maxPlayers)
            {
                players.Add(currentPlayerId, InputType.Keyboard);
                characterSelectionDisplays[currentPlayerId - 1].Initialize(currentPlayerId, players[currentPlayerId], 0);
                currentPlayerId++;
                keyboardAdded = true;
            }
        }
    }

    public void AddPlayer(Player player)
    {
        playersConfirmed.Add(player);
        if (playersConfirmed.Count >= maxPlayers)
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
