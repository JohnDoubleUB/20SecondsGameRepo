using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class MouseInteractableLock : MouseInteractable
{
    private string LockValue;
    public Puzzle PuzzleBrain;

    public bool Locked { get; private set; } = true;
    

    [SerializeField]
    AudioSource LockAudioSource;

    [SerializeField]
    Animator LockAnimator;

    public void ResetLock()
    {
        Locked = false;
        LockAnimator.SetBool("Unlock", false);
    }

    public void SetLockValue(string value)
    {
        LockValue = value;
    }

    protected override void OnInteract(bool value)
    {
        if (!value) 
        {
            return;
        }

        if (!Locked) 
        {
            return;
        }

        if (LockAnimator != null)
        {
            LockAnimator.SetBool("Unlock", true);
        }

        if (LockAudioSource != null) 
        {
            LockAudioSource.Play();
        }

        PuzzleBrain.ButtonValue(LockValue);

        Locked = false;
    }

}
