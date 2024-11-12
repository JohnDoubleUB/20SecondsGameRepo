using UnityEngine;

[System.Serializable]
public class SwitchAndLight 
{
    public IndicatorLight Light;
    public MouseInteractableSwitch Switch;
}

public class SwitchPuzzle : Puzzle
{
    [SerializeField]
    private CodeDisplayer Displayer;

    [SerializeField]
    private SwitchAndLight[] SwitchAndLights;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < SwitchAndLights.Length; i++) 
        {
            SwitchAndLight switchAndLight = SwitchAndLights[i];
            switchAndLight.Switch.Index = i;
            switchAndLight.Switch.PuzzleBrain = this;

            switchAndLight.Switch.SetSwitchValue(false, Random.value > 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SwitchValue(bool value, int index)
    {
        SwitchAndLight switchAndLight = SwitchAndLights[index];
        switchAndLight.Light.SetLight(value ? LightColor.Green : LightColor.None, 0);
    }
}
