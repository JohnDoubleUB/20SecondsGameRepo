using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KeypadPuzzle : Puzzle
{
    public bool CodeIsValid = false;

    private string Password = "";

    public string SuccessMessage = "Access Granted!";
    public string DeniedMessage = "Code Incorrect!";

    public float MessageDisplayTime = 0.5f;

    public char PasswordGapCharacter = '_';

    private string CurrentCode = string.Empty;

    private string[] buttons = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "OK", "0", "X" };

    public MouseInteractableButton[] KeypadButtons;

    [SerializeField]
    private UnityEvent m_OnPuzzleSuccess = new UnityEvent();

    [SerializeField]
    private UnityEvent m_OnPuzzleReset = new UnityEvent();


    private float timer = 0;

    protected new void Awake()
    {
        base.Awake();
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


    protected override void OnReset()
    {
        SetNewCurrentCode("");
        m_OnPuzzleReset?.Invoke();
    }

    protected override void OnFullReset()
    {
        SetNewCurrentCode("");
        m_OnPuzzleReset?.Invoke();
    }

    private void Update()
    {
        if(timer > 0) 
        {
            timer -= Time.deltaTime;

            if (timer <= 0) 
            {
                if(Displayer.CurrentText == DeniedMessage) 
                {
                    SetNewCurrentCode(CurrentCode);
                }
            }
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
        if(CurrentCode == Password && CodeIsValid) 
        {
            Displayer.SetText(SuccessMessage);
            SetPuzzleCompleted(true);
            m_OnPuzzleSuccess?.Invoke();
        }
        else
        {
            timer = MessageDisplayTime;
            CurrentCode = "";
            Displayer.SetText(DeniedMessage);
        }
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
