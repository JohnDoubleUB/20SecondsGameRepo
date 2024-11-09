using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using static UnityEngine.UI.Button;

public class MouseInteractableButton : MouseInteractable
{
    [SerializeField]
    Animator ButtonAnimator;

    [SerializeField]
    AudioSource ButtonAudio;

    [SerializeField]
    AudioResource ButtonDownSound;

    [SerializeField]
    AudioResource ButtonUpSound;

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
            PlayerButtonSound(true);
        }
        else
        {
            m_OnInteractUp?.Invoke();
            PlayerButtonSound(false);
        }
    }

    private void PlayerButtonSound(bool down)
    {
        if (ButtonAudio == null) 
        {
            return;
        }

        ButtonAudio.resource = down ? ButtonDownSound : ButtonUpSound;
        ButtonAudio.Play();
    }


}
