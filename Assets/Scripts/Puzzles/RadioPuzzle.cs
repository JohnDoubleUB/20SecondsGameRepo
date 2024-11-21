using UnityEngine;

public class RadioPuzzle : Puzzle
{
    [SerializeField]
    private IndicatorLight CompleteLight;

    [SerializeField]
    private MouseInteractablePanel Panel;

    [SerializeField]
    private IndicatorLight OnLight;

    [SerializeField]
    private MouseInteractableSwitch OnSwitch;

    [SerializeField]
    private MouseInteractableDial Dial;

    [SerializeField]
    private RadioDisplay RadioDisplay;

    [SerializeField]
    private float RequiredPrecision = 0.01f;

    [SerializeField]
    private float MinimumDistanceFromCenter = 0.2f;

    [SerializeField]
    private float CalibrationTimeModifier = 0.5f;

    private float CalibrationTimer = 0;

    [SerializeField]
    private AudioSource NoiseAudioSource;

    [SerializeField]
    private AudioSource TargetAudioSource;

    private float randomValue;

    private bool PuzzleOn = false;

    private bool InTune = false;

    public void SetPuzzle(RadioPuzzleData radioData) 
    {
        if (radioData.Value2)
        {
            randomValue = radioData.Value1.Remap(0, 1, 0.5f + MinimumDistanceFromCenter, 1f);
        }
        else
        {
            randomValue = radioData.Value1.Remap(0, 1, 0.5f - MinimumDistanceFromCenter, 0f);
        }
    }

    protected override void OnReset()
    {
        SetPuzzleOn(false);
        OnSwitch.SetSwitchValue(false);
        Dial.ResetInteractable();
        Panel.ResetInteractable();
        InTune = false;
    }

    protected override void OnFullReset()
    {
        SetPuzzleOn(false);
        OnSwitch.SetSwitchValue(false);
        Dial.ResetInteractable();
        Panel.ResetInteractable();
        InTune = false;
    }

    private void Start()
    {
        OnSwitch.PuzzleBrain = this;
    }

    private void Update()
    {
        float dialRemappedValue = Dial.CumulativeRotationX.Remap(Dial.MinMaxCumulativeRotation.x, Dial.MinMaxCumulativeRotation.y, 0, 1);

        RadioDisplay.SetNeedlePosition(dialRemappedValue);

        if (!PuzzleOn)
        {
            return;
        }

        float distanceFromTarget = Mathf.Abs(dialRemappedValue - randomValue);

        NoiseAudioSource.volume = distanceFromTarget;
        TargetAudioSource.volume = distanceFromTarget.Remap(0, MinimumDistanceFromCenter, 1, 0);

        if (PuzzleCompleted) 
        {
            return;
        }

        if (distanceFromTarget < RequiredPrecision)
        {
            if (InTune == false) 
            {
                InTune = true;
                CompleteLight.SetLight(LightColor.Yellow, 0);
            }
        }
        else 
        {
            if (InTune == true) 
            {
                InTune = false;
                CompleteLight.SetLight(LightColor.None, 0);
            }
        }


        if(InTune) 
        {
            if(CalibrationTimer < 1f) 
            {
                CalibrationTimer += Time.deltaTime * CalibrationTimeModifier;
                Displayer.SetText($"{Mathf.CeilToInt(CalibrationTimer * 100)}%");
            }
            else 
            {
                SetPuzzleCompleted(true);
                SetCompleteLight();
            }
        }
        else
        {
            Displayer.SetText("ERROR");
        }
    }

    public override void SwitchValue(bool value, int index)
    {
        SetPuzzleOn(!PuzzleOn);
    }

    private void SetPuzzleOn(bool value) 
    {
        PuzzleOn = value;
        OnLight.SetLight(PuzzleOn ? LightColor.Green : LightColor.None, 0);
        SetCompleteLight();

        NoiseAudioSource.mute = !PuzzleOn;
        TargetAudioSource.mute = !PuzzleOn;

        CalibrationTimer = 0f;

        SetDisplay();
    }

    private void SetDisplay() 
    {
        if(PuzzleOn) 
        {
            Displayer.SetText(PuzzleCompleted ? PuzzleCode : "ERROR");
        }
        else 
        {
            Displayer.SetText("");
        }
        
    }

    private void SetCompleteLight() 
    {
        CompleteLight.SetLight(PuzzleOn && InTune ? PuzzleCompleted ? LightColor.Green : LightColor.Yellow : LightColor.None, 0);
    }
}
