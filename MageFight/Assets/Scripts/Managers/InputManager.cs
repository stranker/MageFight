using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public InputType inputType;
    public int playerId;
    public int joystickId;

    public bool debugMode = false;
    private bool onDebug = false;

    private void Update()
    {
        if (debugMode && !onDebug)
        {
            onDebug = true;
            SetInput(InputType.Keyboard, 1, -1);
        }
    }

    public void SetInput(InputType type, int playerId, int joystickId = -1)
    {
        inputType = type;
        this.playerId = playerId;
        this.joystickId = joystickId;
        UpdateInput();
    }

    private void UpdateInput()
    {
        string subfix = inputType == InputType.Keyboard ? "KBRD_" : "J" + joystickId.ToString() + "_";
        jumpButton = subfix + "Jump";
        flyButton = subfix + "Fly";
        xAxis = subfix + "Axis_X";
        yAxis = subfix + "Axis_Y";
        firstSkillButton = subfix + "FirstSkill";
        secondSkillButton = subfix + "SecondSkill";
        thirdSkillButton = subfix + "ThirdSkill";
        pauseButton = subfix + "Pause";
    }

    #region Input
    public string jumpButton; // X?
    public string flyButton; // ?
    public string xAxis; // L Stick
    public string yAxis; // L Stick
    public string firstSkillButton; // Cuadrado ? 
    public string secondSkillButton; // Circulo ?
    public string thirdSkillButton; // Triangulo ?
    public string pauseButton; // Start


    public string GetXAxis()
    {        
        return xAxis;
    }

    public string GetYAxis()
    {
        return yAxis;
    }

    public string GetFlyButton()
    {
        return flyButton;
    }
    #endregion
}
