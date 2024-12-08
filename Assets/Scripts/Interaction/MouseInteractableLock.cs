using UnityEngine;
using UnityEngine.Audio;

public class MouseInteractableLock : MouseInteractable
{
    private string LockValue;
    public Puzzle PuzzleBrain;

    private bool Unlocked = false;

    [SerializeField]
    AudioResource LockedSound;

    [SerializeField]
    AudioResource UnlockedSound;

    [SerializeField]
    AudioSource LockAudioSource;

    [SerializeField]
    Animator LockAnimator;

    protected override void OnResetInteractable()
    {
        LockAnimator.Play("Locked", 0);
        Unlocked = false;
    }

    public void SetLockValue(string value)
    {
        LockValue = value;
    }

    public void ShakeAnimation() 
    {
        if (Unlocked) 
        {
            return;
        }

        if (LockAnimator == null)
        {
            return;
        }

        LockAudioSource.resource = LockedSound;
        LockAudioSource.Play();
        LockAnimator.Play("Shake", 0);

        if (LockAudioSource != null)
        {
            LockAudioSource.Play();
        }
    }

    public void UnlockAnimation(bool value)
    {
        if (LockAnimator == null)
        {
            return;
        }

        LockAudioSource.resource = UnlockedSound;
        LockAudioSource.Play();

        Unlocked = value;
        LockAnimator.Play(value ? "Unlock" : "Locked");

        if (LockAudioSource != null)
        {
            LockAudioSource.Play();
        }
    }

    protected override void OnInteract(bool value, bool triggeredByReset = false)
    {
        if (!value) 
        {
            return;
        }

        if(PuzzleBrain != null) 
        {
            PuzzleBrain.ButtonValue(LockValue);
        }
    }

}
