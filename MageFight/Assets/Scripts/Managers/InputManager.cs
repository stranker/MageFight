using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public InputType inputType;
    public int playerId;

    public void SetInput(InputType type, int playerId)
    {
        inputType = type;
        this.playerId = playerId;
        UpdateInput();
    }

    private void UpdateInput()
    {
        jumpButton = "P" + playerId.ToString() + "_" + jumpButton;
        flyButton = "P" + playerId.ToString() + "_" + flyButton;
        flyButtonKeyboard = "P" + playerId.ToString() + "_" + flyButtonKeyboard;
        movementAxisX = "P" + playerId.ToString() + "_" + movementAxisX;
        AxisXKeyboard = "P" + playerId.ToString() + "_" + AxisXKeyboard;
        aimAxisY = "P" + playerId.ToString() + "_" + aimAxisY;
        AxisYKeyboard = "P" + playerId.ToString() + "_" + AxisYKeyboard;
        DPadX = "P" + playerId.ToString() + "_" + DPadX;
        DPadY = "P" + playerId.ToString() + "_" + DPadY;
        firstSkillButton = "P" + playerId.ToString() + "_" + firstSkillButton;
        secondSkillButton = "P" + playerId.ToString() + "_" + secondSkillButton;
        thirdSkillButton = "P" + playerId.ToString() + "_" + thirdSkillButton;
        pauseButton = "P" + playerId.ToString() + "_" + pauseButton;
    }

    #region Input
    public string jumpButton; // X?
    public string flyButton; // ?
    public string flyButtonKeyboard;
    public string movementAxisX; // L Stick
    public string AxisXKeyboard; // L Stick
    public string aimAxisY; // L Stick
    public string AxisYKeyboard; // L Stick
    public string DPadX; // D Pad Horizontal
    public string DPadY; // D Pad Vertical
    public string firstSkillButton; // Cuadrado ? 
    public string secondSkillButton; // Circulo ?
    public string thirdSkillButton; // Triangulo ?
    public string pauseButton; // Start
    #endregion
}
