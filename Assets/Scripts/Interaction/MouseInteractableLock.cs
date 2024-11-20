using UnityEngine;
public class MouseInteractableLock : MouseInteractable
{
    private string LockValue;
    public Puzzle PuzzleBrain;
    

    [SerializeField]
    AudioSource LockAudioSource;

    [SerializeField]
    Animator LockAnimator;

    public void ResetLock()
    {
        LockAnimator.SetBool("Unlock", false);
    }

    public void SetLockValue(string value)
    {
        LockValue = value;
    }

    public void ShakeAnimation() 
    {
        if (LockAnimator == null)
        {
            return;
        }

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

        LockAnimator.SetBool("Unlock", value);

        if (LockAudioSource != null)
        {
            LockAudioSource.Play();
        }
    }

    protected override void OnInteract(bool value)
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
