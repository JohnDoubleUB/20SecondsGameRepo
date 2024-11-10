using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;

public class KeypadPuzzle : Puzzle
{
    private string Password = "";

    public char PasswordGapCharacter = '_';

    private string BlankPassword = "";

    private string CurrentCode = string.Empty;

    private string[] buttons = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "OK", "0", "X" };

    public MouseInteractableButton[] KeypadButtons;

    public bool PuzzleCompleted = false;

    public CodeDisplayer Displayer;

    public void Awake()
    {
        SetPassword("1234");

        if(KeypadButtons == null) 
        {
            return;
        }

        for (int i = 0; i < KeypadButtons.Length && i < buttons.Length; i++)
        {
            MouseInteractableButton currentButton = KeypadButtons[i];
            currentButton.SetButtonValue(buttons[i]);
            currentButton.PuzzleBrain = this;
        }
    }

    public void SetPassword(string password) 
    {
        Password = password;
        SetNewCurrentCode("");
    }

    public override void ButtonValue(string value)
    {
        if (PuzzleCompleted) 
        {
            return;
        }

        switch (value) 
        {
            case "X":
                Backspace();
                return;

            case "OK":
                Enter();
                return;
        }


        if(CurrentCode.Length < Password.Length) 
        {
            SetNewCurrentCode(CurrentCode + value);
        }
    }

    private void Enter() 
    {

    }

    private void Backspace() 
    {
        if(CurrentCode.Length < 1) 
        {
            return;
        }

        SetNewCurrentCode(CurrentCode.Substring(0, CurrentCode.Length - 1));
    }


    private void SetNewCurrentCode(string currentCode) 
    {
        CurrentCode = currentCode;
        
        if (Displayer != null) 
        {
            var padded = CurrentCode.PadRight(Password.Length, PasswordGapCharacter);
            Displayer.SetText(padded);
        }
    }
}
