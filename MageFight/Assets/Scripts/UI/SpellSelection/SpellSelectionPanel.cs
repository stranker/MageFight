using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSelectionPanel : MonoBehaviour
{

    public List<Spell> spellList = new List<Spell>();
    List<Spell> spellInSelection = new List<Spell>();
    [SerializeField] [Range(1, 4)] private int spellsPerRound;
    int spellCounter;
    public GameObject spellsPanel;
    public GameObject spellPanelPrefab;
    private List<Player> playerList = new List<Player>();
    Player currentPlayerTurn = null;

    int turnCounter;

    float timer;
    public float timeEndPickRound;

    private void Awake()
    {
        CreateSpellsPanel();
        playerList = GameManager.Instance.playerList;
        currentPlayerTurn = playerList[turnCounter];
    }

    private void CreateSpellsPanel()
    {
        
        for (int i = 0; i < spellsPerRound; i++)
        {
            SpellInfoPanel spellInfoPanel = Instantiate(spellPanelPrefab, spellsPanel.transform).GetComponent<SpellInfoPanel>();
            spellInfoPanel.SetSpell(GetNewSpell());
        }
    }

    private void Update()
    {
        if (turnCounter >= playerList.Count)
        {
            timer += Time.deltaTime;
            if (timer > timeEndPickRound)
            {
                EndSpellSelection();
            }
        }
    }

    private void EndSpellSelection()
    {
        GameplayManager.Get().SendEvent(GameplayManager.Events.PowersSelected);
    }

    private void OnGUI()
    {
        CheckPlayerInput();
    }

    private void CheckPlayerInput()
    {
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
        throw new NotImplementedException();
    }

    private void SelectPanelAt(int spellCounter)
    {
        throw new NotImplementedException();
    }

    private Spell GetNewSpell()
    {
        Spell spell = spellList[UnityEngine.Random.Range(0, spellList.Count)];
        do
        {
            spell = spellList[UnityEngine.Random.Range(0, spellList.Count)];
        } while (!spellInSelection.Contains(spell));
        spellInSelection.Add(spell);
        return spell;
    }
}
