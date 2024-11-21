using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class MouseInteractableButton : MouseInteractable
{
    private string ButtonValue;
    public Puzzle PuzzleBrain;
    
    [SerializeField]
    private TextMeshProUGUI ButtonValueText;

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

    public void SetButtonValue(string value) 
    {
        ButtonValue = value;

        if (ButtonValueText != null) 
        {
            ButtonValueText.text = value;
        } 
    }

    protected override void OnInteract(bool value, bool triggeredByReset = false)
    {
        if (ButtonAnimator != null) 
        {
            ButtonAnimator.SetBool("Press", value);
        }

        if (value)
        {
            m_OnInteractDown?.Invoke();

            if(PuzzleBrain != null) 
            {
                PuzzleBrain.ButtonValue(ButtonValue);
            }
        }
        else
        {
            m_OnInteractUp?.Invoke();
           
        }

        if (!triggeredByReset) 
        {
            PlayerButtonSound(value);
        }
    }

    private void PlayerButtonSound(bool down)
    {
        if (ButtonAudio == null)
        {
            return;
        }

        if (down && ButtonDownSound != null)
        {
            ButtonAudio.resource = ButtonDownSound;
            ButtonAudio.Play();
        }
        else if (!down && ButtonUpSound != null)
        {
            ButtonAudio.resource = ButtonUpSound;
            ButtonAudio.Play();
        }
    }


}
