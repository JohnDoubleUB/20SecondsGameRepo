using UnityEngine;

public class SimonSaysPuzzle : Puzzle
{
    private string FullCode = "RGYBYR";

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
    private IndicatorLight IndicatorLight;

    private void Start()
    {
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

    public override void ButtonValue(string value)
    {
        switch (value)
        {
            case "R":
                IndicatorLight.SetLight(LightColor.Red);
                break;
            case "G":
                IndicatorLight.SetLight(LightColor.Green);
                break;
            case "B":
                IndicatorLight.SetLight(LightColor.Blue);
                break;
            case "Y":
                IndicatorLight.SetLight(LightColor.Yellow);
                break;

        }
    }

}
