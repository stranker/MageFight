using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardSelectionButton : MonoBehaviour
{

    public WizardDataScriptable wizardData;

    public Image wizardImage;
    public Text wizardName;

    ColorBlock buttonColors;

    private List<Color> playersColors = new List<Color>();

    private bool isSelected = false;
    public bool isConfirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        buttonColors = GetComponent<Button>().colors;
        wizardImage.sprite = wizardData.artwork;
        wizardName.text = wizardData.wizardName;
    }

    public void Select(Color playerColor)
    {
        if (!playersColors.Contains(playerColor))
        {
            playersColors.Add(playerColor);
            isSelected = true;
            UpdateButtonColor();
        }
    }

    public void Unselect(Color playerColor)
    {
        if (playersColors.Contains(playerColor))
        {
            playersColors.Remove(playerColor);
            isSelected = playersColors.Count > 0;
            UpdateButtonColor();
        }
    }

    private void UpdateButtonColor()
    {
        if (isSelected)
            buttonColors.normalColor = CombineColors(playersColors);
        else
            buttonColors.normalColor = Color.black;
        buttonColors.highlightedColor = buttonColors.normalColor;
        GetComponent<Button>().colors = buttonColors;
    }

    public Color CombineColors(List<Color> colorList)
    {
        Color result = new Color(0, 0, 0, 0);
        foreach (Color c in colorList)
        {
            result += c;
        }
        result /= colorList.Count;
        return result;
    }

    public bool IsConfirmed()
    {
        return isConfirmed;
    }

    public void Confirm()
    {
        if (!isConfirmed)
        {
            isConfirmed = true;
        }
    }
}
