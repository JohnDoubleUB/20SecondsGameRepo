using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Button;

public class MouseInteractableButton : MouseInteractable
{
    [SerializeField]
    Animator ButtonAnimator;

    [SerializeField]
    private UnityEvent m_OnInteractDown = new UnityEvent();

    [SerializeField]
    private UnityEvent m_OnInteractUp = new UnityEvent();
    protected override void OnInteract(bool value)
    {
        if (ButtonAnimator != null) 
        {
            ButtonAnimator.SetBool("Press", value);
        }

        if (value)
        {
            m_OnInteractDown?.Invoke();
        }
        else
        {
            m_OnInteractUp?.Invoke();
        }
    }
}
