﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellSelectionPanel : MonoBehaviour
{
    public List<Spell> spellList = new List<Spell>();
    List<Spell> spellInSelection = new List<Spell>();
    [SerializeField] [Range(1, 4)] private int spellsPerRound;
    public int spellCounter;
    public GameObject spellsPanel;
    public GameObject spellPanelPrefab;
    private List<Player> playerList = new List<Player>();
    private List<SpellInfoPanel> spellsInfoPanelList = new List<SpellInfoPanel>();
    Player currentPlayerTurn = null;
    SpellInfoPanel currentSpellInfoPanel = null;
    public Text wizardName;
    public Text playerName;

    public int turnCounter = 0;

    public bool endSpellSelection = false;
    float timer;
    public float timeEndSpellSelection;

    public WizardDataScriptable wizardDataTest;

    private bool isActive = false;

    public bool debugMode = false;

    private void Start()
    {
        if (debugMode)
        {
            Player testPlayer = new Player(1, wizardDataTest, InputType.Keyboard, 0, Color.red);
            Player testPlayer2 = new Player(2, wizardDataTest, InputType.Joystick, 1, Color.red);
            playerList.Add(testPlayer);
            playerList.Add(testPlayer2);
        }
        else
        {
            playerList = GameManager.Instance.playerList;
        }
        PrepareNewTurn();
    }

    private void OnEnable()
    {
        CreateSpellsPanel();
        isActive = true;
        PrepareNewTurn();
    }

    private void PrepareNewTurn()
    {
        playerList = playerList.OrderBy(player => player.winRounds).ToList();
        turnCounter = 0;
        if (playerList.Count>0)
            currentPlayerTurn = playerList[turnCounter];
        UpdateUI();
    }

    private void OnDisable()
    {
        isActive = false;
    }

    private void UpdateUI()
    {
        if (currentPlayerTurn != null)
        {
            wizardName.text = currentPlayerTurn.charData.wizardName;
            wizardName.color = currentPlayerTurn.playerColor;
            playerName.text = "Player " + currentPlayerTurn.playerId.ToString();
            playerName.color = currentPlayerTurn.playerColor;
        }

    }

    private void CreateSpellsPanel()
    {
        ClearPrevData();
        for (int i = 0; i < spellsPerRound; i++)
        {
            SpellInfoPanel spellInfoPanel = Instantiate(spellPanelPrefab, spellsPanel.transform).GetComponent<SpellInfoPanel>();
            spellInfoPanel.SetSpell(GetNewSpell());
            spellsInfoPanelList.Add(spellInfoPanel);
        }
        SelectPanelAt(0);
    }

    private void ClearPrevData()
    {
        if (spellsInfoPanelList.Count > 0)
        {
            foreach (SpellInfoPanel spellInfo in spellsInfoPanelList)
            {
                Destroy(spellInfo.gameObject);
            }
        }
        spellsInfoPanelList.Clear();
    }

    private void Update()
    {
        if (isActive)
        {
            if (endSpellSelection)
            {
                timer += Time.deltaTime;
                if (timer > timeEndSpellSelection)
                    EndSpellSelection();
            }
            else
                CheckPlayerInput();
        }
    }

    private void EndSpellSelection()
    {
        GameplayManager.Get().SendEvent(GameplayManager.Events.SpellsSelected);
        endSpellSelection = false;
        isActive = false;
        gameObject.SetActive(false);
    }

    private void CheckPlayerInput()
    {
        if (currentPlayerTurn == null)
            return;
        switch (currentPlayerTurn.inputType)
        {
            case InputType.Joystick:
                CheckJoystickInput();
                break;
            case InputType.Keyboard:
                CheckKeyboardInput();
                break;
            case InputType.Count:
                break;
            default:
                break;
        }
    }

    private void CheckJoystickInput()
    {
        if (Input.GetAxis("J" + currentPlayerTurn.joistickId.ToString() + "_Axis_X") > 0.5f)
        {
            spellCounter++;
            SelectPanelAt(spellCounter);
        }
        else if (Input.GetAxis("J" + currentPlayerTurn.joistickId.ToString() + "_Axis_X") < -0.5f)
        {
            spellCounter--;
            SelectPanelAt(spellCounter);
        }
        if (Input.GetKey("joystick " + currentPlayerTurn.joistickId.ToString() + " button 9"))
        {
            OnSpellConfirm();
        }
    }

    private void CheckKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            spellCounter++;
            SelectPanelAt(spellCounter);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spellCounter--;
            SelectPanelAt(spellCounter);
        }
        if (Input.GetKey(KeyCode.Return))
        {
            OnSpellConfirm();
        }
    }

    private void OnSpellConfirm()
    {
        currentSpellInfoPanel.Confirm();
        spellsInfoPanelList.Remove(currentSpellInfoPanel);
        currentPlayerTurn.AddSpell(currentSpellInfoPanel.currentSpell);
        turnCounter++;
        if (turnCounter >= playerList.Count)
            endSpellSelection = true;
        else
            currentPlayerTurn = playerList[turnCounter];
        SelectPanelAt(0);
        UpdateUI();
    }

    private void SelectPanelAt(int idx)
    {
        if (currentSpellInfoPanel)
            currentSpellInfoPanel.Selected(false);
        spellCounter = Mathf.Clamp(idx, 0, spellsInfoPanelList.Count - 1);
        currentSpellInfoPanel = spellsInfoPanelList[spellCounter];
        currentSpellInfoPanel.Selected(true);
    }

    private Spell GetNewSpell()
    {
        int count = 0;
        Spell spell = spellList[UnityEngine.Random.Range(0, spellList.Count)];
        while (spellInSelection.Contains(spell) && spellInSelection.Count != 0)
        {
            spell = spellList[UnityEngine.Random.Range(0, spellList.Count)];
            count++;
            if (count >= spellList.Count)
            {
                spellInSelection.Clear();
            }
        }
        spellInSelection.Add(spell);
        return spell;
    }
}