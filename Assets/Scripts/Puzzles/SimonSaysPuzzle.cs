using UnityEngine;


public enum SimonSaysState 
{
    NoInput,
    ShowingSequence,
    WaitingForInput
}
public class SimonSaysPuzzle : Puzzle
{
    public SimonSaysState CurrentState = SimonSaysState.NoInput;

    private string FullCode = "RGYBYR";
    private string TargetCode = "";
    private string CurrentCode = "";
    private int CurrentIndex = 0;

    private float interval = 1.5f;

    private float timer = 0;

    [SerializeField]
    private CodeDisplayer Displayer;

    [SerializeField]
    private MouseInteractableButton RedButton;

    [SerializeField]
    private MouseInteractableButton BlueButton;

    [SerializeField]
    private MouseInteractableButton GreenButton;

    [SerializeField]
    private MouseInteractableButton YellowButton;

    [SerializeField]
    private IndicatorLight[] IndicatorLight;

    private void Update()
    {
        if (PuzzleCompleted)
        {
            return;
        }

        if (CurrentState == SimonSaysState.NoInput) 
        {
            if (timer <= 0)
            {
                IndicatorLight[0].SetLight(FullCode[0], interval / 2);
                timer = interval;
                return;
            }

            timer -= Time.deltaTime;
        }
        else if (CurrentState == SimonSaysState.ShowingSequence) 
        {
            if (CurrentIndex >= TargetCode.Length)
            {
                CurrentState = SimonSaysState.WaitingForInput;
                return;
            }

            if (timer <= 0)
            {
                IndicatorLight[CurrentIndex].SetLight(FullCode[CurrentIndex], interval / 4);
                timer = interval/ 2;
                CurrentIndex++;
                return;
            }

            timer -= Time.deltaTime;
        }

    }

    private void Start()
    {
        TargetCode = $"{FullCode[0]}";

        if (RedButton != null)
        {
            RedButton.SetButtonValue("R");
            RedButton.PuzzleBrain = this;
        }

        if (GreenButton != null)
        {
            GreenButton.SetButtonValue("G");
            GreenButton.PuzzleBrain = this;
        }

        if (BlueButton != null)
        {
            BlueButton.SetButtonValue("B");
            BlueButton.PuzzleBrain = this;
        }

        if (YellowButton != null)
        {
            YellowButton.SetButtonValue("Y");
            YellowButton.PuzzleBrain = this;
        }
    }

    public override void ResetPuzzle()
    {
        base.ResetPuzzle();
    }

    private void Incorrect()
    {
        foreach (IndicatorLight l in IndicatorLight)
        {
            l.SetLight(LightColor.Red, interval / 2);
        }
        CurrentState = SimonSaysState.NoInput;
        TargetCode = $"{FullCode[0]}";
        CurrentCode = "";
    }

    public override void ButtonValue(string value)
    {
        if (PuzzleCompleted) 
        {
            return;
        }

        //Skip if showing a sequence
        if (CurrentState == SimonSaysState.ShowingSequence) 
        {
            return;
        }

        //Check if the next character is correct or not
        if (FullCode[CurrentCode.Length] != value[0]) 
        {
            Incorrect();
            return;
        }

        //Add character to current code
        CurrentCode += value;

        //If Current code hasn't reached the target length, then return (They need to enter more of the sequence)
        if (CurrentCode.Length < TargetCode.Length)
        {
            return;
        }

        //If CurrentCode is now the same length as the full code
        if (CurrentCode.Length == FullCode.Length) 
        {
            foreach (IndicatorLight l in IndicatorLight)
            {
                l.SetLight(LightColor.Green, 10);
            }

            PuzzleCompleted = true;

            return;
        }



        //if(CurrentCode != TargetCode) 
        //{
        //    Incorrect();
        //}
        //else 
        //{
        //Add the next value in
            foreach (IndicatorLight l in IndicatorLight)
            {
                l.SetLight(LightColor.None, 0);
            }

            timer = interval / 2;

            TargetCode += FullCode[TargetCode.Length];
            CurrentState = SimonSaysState.ShowingSequence;
            CurrentIndex = 0;
            CurrentCode = "";
        }
    //}

}
