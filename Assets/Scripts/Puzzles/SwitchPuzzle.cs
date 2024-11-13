using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private IndicatorLight[] ShuffledLights;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Determine if it is 2 or 3 
        int enabledSwitchCount = 2;//Random.value > 0.7f ? 2 : 3;

        int linkedSwitchCount = 3;

        int[] onSwitches = new int[SwitchAndLights.Length];

        for (int i = 0; i < enabledSwitchCount;) 
        {
            int randomIndex = Random.Range(0, onSwitches.Length);

            if (onSwitches[randomIndex] < 1) 
            {

                onSwitches[randomIndex] = 1;
                if(randomIndex+1 < onSwitches.Length) 
                {
                    onSwitches[randomIndex+1] = 2;
                }

                if (randomIndex - 1 > 0) 
                {
                    onSwitches[randomIndex - 1] = 2;
                }
                i++;
            }
        }

        ShuffledLights = SwitchAndLights.Select(x => x.Light).ToArray();
        ShuffledLights.Shuffle();

        for (int i = 0; i < SwitchAndLights.Length; i++) 
        {
            SwitchAndLight switchAndLight = SwitchAndLights[i];
            switchAndLight.Switch.Index = i;
            switchAndLight.Switch.PuzzleBrain = this;

            bool shouldStartOn = onSwitches[i] == 1;

            switchAndLight.Switch.SetSwitchValue(shouldStartOn, Random.value > 0.5f);
            switchAndLight.Light.SetLight(shouldStartOn ? LightColor.Green : LightColor.None, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SwitchValue(bool value, int index)
    {
        SwitchAndLight switchAndLight = SwitchAndLights[index];
        switchAndLight.Light.SetLight(!switchAndLight.Light.isOn ? LightColor.Green : LightColor.None, 0);
        
        IndicatorLight shuffledLight = ShuffledLights[index];
        if (shuffledLight != switchAndLight.Light) 
        {
            shuffledLight.SetLight(!shuffledLight.isOn ? LightColor.Green : LightColor.None, 0);
        }
    }
}
