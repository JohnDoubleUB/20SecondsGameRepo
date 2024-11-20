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
    private SwitchAndLight[] SwitchAndLights;

    
    public Dictionary<int, IndicatorLight> connectedLights = new Dictionary<int, IndicatorLight>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Determine if it is 2 or 3 
        int enabledSwitchCount = 2;
        int randomIndex;

        int[] onSwitches = new int[SwitchAndLights.Length];

        List<int> linkableSwitches = new List<int>();
        List<int> onSwitchIndexes = new List<int>();

        for (int i = 0; i < onSwitches.Length; i++) 
        {
            linkableSwitches.Add(i);
        }

        connectedLights.Clear();

        for (int i = 0; i < enabledSwitchCount;) 
        {
            randomIndex = Random.Range(0, onSwitches.Length);

            if (onSwitches[randomIndex] < 1) 
            {
                onSwitches[randomIndex] = 1;
                linkableSwitches.Remove(randomIndex);
                onSwitchIndexes.Add(randomIndex);
                if (randomIndex+1 < onSwitches.Length) 
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

        for (int i = 0; i < linkableSwitches.Count && i < onSwitchIndexes.Count ; i++) 
        {
            connectedLights.Add(linkableSwitches[i], SwitchAndLights[onSwitchIndexes[i]].Light);
        }

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
        
        if(connectedLights.TryGetValue(index, out IndicatorLight light)) 
        {
            if (light == null) 
            {
                return;
            }

            light.SetLight(!light.isOn ? LightColor.Green : LightColor.None, 0);
        }
    }
}
