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

    [SerializeField]
    private MouseInteractablePanel Panel;
    
    public Dictionary<int, IndicatorLight> connectedLights = new Dictionary<int, IndicatorLight>();

    private SwitchData Data;

    public override void OnReset() 
    {
        ResetSwitches();

        if (Panel != null) 
        {
            Panel.ResetInteractable();
        }
    }

    public override void OnFullReset()
    {
        ResetSwitches();

        if (Panel != null)
        {
            Panel.ResetInteractable();
        }
    }

    public void SetPuzzle(SwitchData switchData) 
    {
        Data = switchData;

        connectedLights.Clear();

        for (int i = 0; i < Data.LinkableSwitches.Count && i < Data.OnSwitchIndexes.Count; i++)
        {
            connectedLights.Add(Data.LinkableSwitches[i], SwitchAndLights[Data.OnSwitchIndexes[i]].Light);
        }

        ResetSwitches();
    }

    private void ResetSwitches() 
    {
        for (int i = 0; i < SwitchAndLights.Length; i++)
        {
            SwitchAndLight switchAndLight = SwitchAndLights[i];
            switchAndLight.Switch.Index = i;
            switchAndLight.Switch.PuzzleBrain = this;

            bool shouldStartOn = Data.OnSwitches[i] == 1;

            switchAndLight.Switch.SetSwitchValue(shouldStartOn, Data.InitialSwitchPositions[i]);
            switchAndLight.Light.SetLight(shouldStartOn ? LightColor.Green : LightColor.None, 0);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SwitchValue(bool value, int index)
    {
        if (PuzzleCompleted) 
        {
            return;
        }

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

        if (AreAllLightsOn()) 
        {
            SetPuzzleCompleted(true);
        }
    }

    private bool AreAllLightsOn() 
    {
        for (int i = 0; i < SwitchAndLights.Length; i++) 
        {
            if (!SwitchAndLights[i].Light.isOn) 
            {
                return false;
            }
        }

        return true;
    }
}
