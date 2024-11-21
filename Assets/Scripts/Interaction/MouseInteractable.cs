using UnityEngine;

public class MouseInteractable : MonoBehaviour
{
    public bool ControlUntilRelease = false;
    public bool Held { get; private set; }

    public void Interact(bool value) 
    {
        if (value != Held) 
        {
            Held = value;
            OnInteract(value);
        }
    }

    public void ResetInteractable() 
    {
        Interact(false);
        OnResetInteractable();
    }

    protected virtual void OnResetInteractable() 
    {

    }

    //TODO: May be a sound or two that isn't effected not sure, probably buttons (triggerByReset)
    protected virtual void OnInteract(bool value, bool triggeredByReset = false) 
    {

    }
}
