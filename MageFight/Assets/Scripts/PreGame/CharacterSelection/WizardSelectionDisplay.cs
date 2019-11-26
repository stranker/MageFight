using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player
{
    public string playerName;
    public int playerId;
    public WizardDataScriptable wizardData;
    public InputType inputType;
    public int joistickId;
    public int winRounds = 0;
    public Color playerColor;
    public List<Spell> spellList = new List<Spell>();
    public GameObject wizardRef;
    private int spellReplaceIdx = 0;

    public Player(int playerId, WizardDataScriptable wizardData, InputType inputType, int joistickId, Color playerColor)
    {
        this.playerId = playerId;
        this.wizardData = wizardData;
        this.inputType = inputType;
        this.joistickId = joistickId;
        this.playerColor = playerColor;
    }

    public void AddSpell(Spell spell)
    {
        if (spellList.Count < 3)
        {
            spellList.Add(spell);
        }
        else
        {
            spellList.RemoveAt(spellReplaceIdx);
            spellList.Insert(spellReplaceIdx, spell);
            spellReplaceIdx++;
        }
    }

    public void Reset()
    {
        winRounds = 0;
        spellList.Clear();
        spellReplaceIdx = 0;
        if (wizardRef)
            wizardRef.GetComponent<SpellsManager>().Reset();
    }

    public void AddWizard(GameObject wizard)
    {
        wizardRef = wizard;
    }
}


public class WizardSelectionDisplay : MonoBehaviour
{
    public bool isActive = false;

    public Text wizardName;
    public Text inputIdText;
    public Text playerText;
    public Image wizardArtwork;
    public Image inputImage;

    public int playerId;
    public ushort joystickId;

    public InputType inputType;
    public Sprite[] inputImageList;

    public GameObject initPanel;
    public GameObject playerPanel;

    public bool playerConfirmed = false;

    private float canConfirmTimer;
    public float canConfirmTime = 0.7f;
    private bool canConfirm = false;

    Player player = null;

    public int wizardIdx = 0;
    public WizardSelectionButton currentWizard;

    public Image keyboardImage;

    public Color playerColor;

    public Animator anim;


    private void Update()
    {
        keyboardImage.gameObject.SetActive(!WizardSelectionManager.Instance.players.ContainsValue(InputType.Keyboard));
        if (!canConfirm && playerPanel.activeInHierarchy)
        {
            if (canConfirmTimer > 0)
            {
                canConfirmTimer -= Time.deltaTime;
            }
            else
            {
                canConfirm = true;
                canConfirmTimer = 0;
            }
        }
        CheckInput();
    }


    private void UpdateUI()
    {
        wizardName.text = currentWizard.wizardData.wizardName;
        wizardArtwork.sprite = currentWizard.wizardData.artwork;
        inputImage.sprite = inputImageList[(ushort)inputType];
    }

    public void Initialize(int currentPlayerId, InputType inputType, ushort inputId)
    {
        isActive = true;
        playerId = currentPlayerId;
        this.inputType = inputType;
        anim.SetTrigger("PlayerEnter");
        canConfirmTimer = canConfirmTime;
        joystickId = inputId;
        if (joystickId != 0)
        {
            inputIdText.gameObject.SetActive(true);
            inputIdText.text = joystickId.ToString();
        }
        SelectWizardAt(wizardIdx);
    }

    public void SelectWizardAt(int idx)
    {
        if (currentWizard)
            currentWizard.Unselect(playerColor);
        wizardIdx = Mathf.Clamp(idx, 0, WizardSelectionManager.Instance.wizardsSelectionButtons.Length - 1);
        currentWizard = WizardSelectionManager.Instance.wizardsSelectionButtons[wizardIdx];
        currentWizard.Select(playerColor);
        UpdateUI();
    }

    private void CheckInput()
    {
        if (!isActive)
            return;
        switch (inputType)
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

        if (!playerConfirmed)
        {
            if (Input.GetAxis("J" + joystickId.ToString() + "_Axis_X") > 0.5f)
            {
                wizardIdx++;
                SelectWizardAt(wizardIdx);
            }
            else if (Input.GetAxis("J" + joystickId.ToString() + "_Axis_X") < -0.5f)
            {
                wizardIdx--;
                SelectWizardAt(wizardIdx);
            }
            if (Input.GetKey("joystick " + joystickId.ToString() + " button 0") && canConfirm)
            {
                OnPlayerConfirm();
            }
        }
    }


    private void CheckKeyboardInput()
    {
        if (!playerConfirmed)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                wizardIdx++;
                SelectWizardAt(wizardIdx);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                wizardIdx--;
                SelectWizardAt(wizardIdx);
            }
            if (Input.GetKey(KeyCode.Return) && canConfirm)
            {
                OnPlayerConfirm();
            }
        }
    }

    private void OnPlayerConfirm()
    {
        if (!currentWizard.IsConfirmed())
        {
            playerConfirmed = true;
            player = new Player(playerId, currentWizard.wizardData, inputType, joystickId, playerId == 1 ? Color.red : Color.blue);
            WizardSelectionManager.Instance.AddPlayer(player);
            anim.SetTrigger("Ready");

        }
        currentWizard.Confirm();
    }
}
