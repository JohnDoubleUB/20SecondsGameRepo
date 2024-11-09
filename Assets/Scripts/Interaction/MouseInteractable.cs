using UnityEngine;

public class MouseInteractable : MonoBehaviour
{
    public bool Held { get; private set; }

    public void Interact(bool value) 
    {
        Held = value;
        OnInteract(value);
    }

    protected virtual void OnInteract(bool value) 
    {

    }
}
