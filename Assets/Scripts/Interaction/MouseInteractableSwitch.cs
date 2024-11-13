using System;
using UnityEngine;
using UnityEngine.Audio;

public class MouseInteractableSwitch : MouseInteractable
{
    public Puzzle PuzzleBrain;

    [SerializeField]
    AudioResource SwitchSound;

    [SerializeField]
    AudioSource ButtonAudio;

    [SerializeField]
    Animator SwitchAnimator;

    private bool toggle = false;

    public int Index;

    private bool SwitchInversion = false;

    protected override void OnInteract(bool value)
    {
        if (!value) 
        {
            return;
        }

        SetSwitchValue(!toggle);

        if (PuzzleBrain != null)
        {
            PuzzleBrain.SwitchValue(toggle, Index);
        }

        PlayButtonSound();
    }

    public void SetSwitchValue(bool value) 
    {
        toggle = value;

        if (SwitchAnimator != null)
        {
            SwitchAnimator.SetBool("switch", toggle != SwitchInversion);
        }
    }

    public void SetSwitchValue(bool value, bool switchInversion)
    {
        toggle = value;
        SwitchInversion = switchInversion;

        if (SwitchAnimator != null)
        {
            SwitchAnimator.SetBool("switch", toggle != SwitchInversion);
        }
    }

    private void PlayButtonSound()
    {
        if (ButtonAudio == null) 
        {
            return;
        }

        ButtonAudio.resource = SwitchSound;
        ButtonAudio.Play();
    }
}
