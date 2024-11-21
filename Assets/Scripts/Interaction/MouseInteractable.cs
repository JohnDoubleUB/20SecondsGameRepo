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

    //TODO: Not super needed, but if it could have an optional bool to indicate if sounds should play when this triggers that'd be cool!
    protected virtual void OnInteract(bool value) 
    {

    }
}
