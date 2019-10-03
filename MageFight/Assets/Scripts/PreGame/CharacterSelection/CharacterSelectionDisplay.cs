using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public int playerId;
    public CharacterSelection charData;
    public InputType inputType;
    public int joistickId;

    public Player(int playerId, CharacterSelection charData, InputType inputType, int joistickId)
    {
        this.playerId = playerId;
        this.charData = charData;
        this.inputType = inputType;
        this.joistickId = joistickId;
    }
}


public class CharacterSelectionDisplay : MonoBehaviour
{
    public List<CharacterSelection> characterDataList;
    public int characterIdx = 0;

    private CharacterSelection characterData;

    public Text characterName;
    public Text characterDesc;
    public Text inputIdText;
    public Image characterArtwork;
    public Image leftArrow;
    public Image rightArrow;
    public Image inputImage;

    public int playerId;
    public ushort joystickId;

    public InputType inputType;
    public Sprite[] inputImageList;


    public GameObject initPanel;
    public GameObject playerPanel;

    public bool playerConfirmed = false;

    private float canConfirmTimer;
    private float canConfirmTime = 1.1f;
    private bool canConfirm = false;

    Player player = null;

    // Start is called before the first frame update
    void Start()
    {
        leftArrow.gameObject.SetActive(false);
        SelectCharacterAt(characterIdx);
    }

    private void Update()
    {
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

    }

    private void SelectCharacterAt(int idx)
    {
        characterIdx = Mathf.Clamp(idx, 0, characterDataList.Count - 1);
        characterData = characterDataList[characterIdx];
        UpdateUI();
    }

    private void UpdateUI()
    {
        leftArrow.gameObject.SetActive(characterIdx != 0);
        rightArrow.gameObject.SetActive(characterIdx != characterDataList.Count - 1);
        characterName.text = characterData.characterName;
        characterDesc.text = characterData.description;
        characterArtwork.sprite = characterData.artwork;
        inputImage.sprite = inputImageList[(ushort)inputType];
    }

    public void Initialize(int currentPlayerId, InputType inputType, ushort inputId)
    {
        playerId = currentPlayerId;
        this.inputType = inputType;
        initPanel.SetActive(false);
        playerPanel.SetActive(true);
        canConfirmTimer = canConfirmTime;
        joystickId = inputId;
        if (joystickId != 0)
        {
            inputIdText.gameObject.SetActive(true);
            inputIdText.text = joystickId.ToString();
        }
        UpdateUI();
    }

    private void OnGUI()
    {
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
        if (!playerConfirmed && joystickId != 0)
        {
            if (Input.GetKey("joystick " + joystickId.ToString() + " button 1"))
            {
                characterIdx++;
                SelectCharacterAt(characterIdx);
            }
            if (Input.GetKey("joystick " + joystickId.ToString() + " button 2"))
            {
                characterIdx--;
                SelectCharacterAt(characterIdx);
            }
            if (Input.GetKey("joystick "+ joystickId.ToString() + " button 0") && canConfirm && !playerConfirmed)
            {
                ConfirmPlayer();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.JoystickButton3) && playerConfirmed)
            {
                RemoveConfirmedPlayer();
            }
        }
    }

    private void CheckKeyboardInput()
    {
        if (!playerConfirmed)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                characterIdx++;
                SelectCharacterAt(characterIdx);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                characterIdx--;
                SelectCharacterAt(characterIdx);
            }
            if (Input.GetKeyDown(KeyCode.Return) && canConfirm && !playerConfirmed)
            {
                ConfirmPlayer();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) && playerConfirmed)
            {
                RemoveConfirmedPlayer();
            }
        }
    }

    private void ConfirmPlayer()
    {
        playerConfirmed = true;
        characterName.text = "READY!";
        player = new Player(playerId, characterData, inputType, joystickId);
        CharacterSelectionManager.Instance.AddPlayer(player);
    }

    private void RemoveConfirmedPlayer()
    {
        playerConfirmed = false;
        CharacterSelectionManager.Instance.RemovePlayer(player);
        UpdateUI();
        player = null;
    }

}
