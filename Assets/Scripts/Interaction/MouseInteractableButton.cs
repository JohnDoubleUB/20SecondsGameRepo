using UnityEngine;

public class MouseInteractableButton : MouseInteractable
{
    [SerializeField]
    Animator ButtonAnimator;
    protected override void OnInteract(bool value)
    {
        if (ButtonAnimator != null) 
        {
            ButtonAnimator.SetBool("Press", value);
        }
    }
}
