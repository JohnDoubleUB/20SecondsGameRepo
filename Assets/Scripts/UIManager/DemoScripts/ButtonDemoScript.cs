using UnityEngine;
using UIManagerLibrary.Scripts;

public class ButtonDemoScript : MonoBehaviour
{
    public void ToggleRed()
    {
        UIManager.current.ToggleActiveContexts("Red");
    }

    public void ToggleGreen() 
    {
        UIManager.current.ToggleActiveContexts("green");
    }
    public void ToggleBlue() 
    {
        UIManager.current.ToggleActiveContexts("Blue");
    }

    public void ToggleAll() 
    {
        UIManager.current.ToggleActiveContexts("Red", "Green", "Blue");
    }

    public void ShowAll() 
    {
        UIManager.current.SetActiveContexts(true, "Red", "Green", "Blue");
    }

    public void HideAll()
    {
        UIManager.current.SetActiveContexts(false, "Red", "Green", "Blue");
    }
}
