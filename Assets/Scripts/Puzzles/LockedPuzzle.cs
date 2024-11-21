using UnityEngine;

public class LockedPuzzle : Puzzle
{
    [SerializeField]
    private MouseInteractableButton Button;

    [SerializeField]
    private MouseInteractablePanel Panel;

    [SerializeField]
    private float LockedMaxRotation = 3f;

    [SerializeField]
    private float UnlockedMaxRotation = 180f;

    private bool Unlocked = false;

    [SerializeField]
    private MouseInteractableLock InteractableLock;

    [SerializeField]
    private int KeyIndex = 0;

    //TODO: Test that this logic works, also add Button reference in inspector
    public override void ResetPuzzle(bool force = false)
    {
        if (Unlocked) 
        {
            return;
        }

        if (Panel != null)
        {
            Panel.MaxRotation = LockedMaxRotation;
        }

        if(InteractableLock != null) 
        {
            InteractableLock.ResetLock();
        }
    }

    private void Start()
    {
        if (Panel != null)
        {
            Panel.OnPanelMovementChange += OnPanelMovementChange;
        }

        if (Button != null)
        {
            Button.PuzzleBrain = this;
            Button.SetButtonValue("B");
        }

        if (InteractableLock != null) 
        {
            InteractableLock.PuzzleBrain = this;
            InteractableLock.SetLockValue("L");
        }
    }

    //
    private void OnPanelMovementChange()
    {
        if (PuzzleCompleted) 
        {
            return;
        }

        if (InteractableLock == null) 
        {
            return;
        }

        InteractableLock.ShakeAnimation();
    }

    private void OnDestroy()
    {
        if(Panel != null) 
        {
            Panel.OnPanelMovementChange -= OnPanelMovementChange;
        }
    }

    private bool TryGetKeyFromPlayer() 
    {
        return PlayerController.current.TryAndRemoveItemWithIndex(KeyIndex);
    }

    public override void ButtonValue(string value)
    {
        if (PuzzleCompleted)
        {
            return;
        }

        if (value == "B") 
        {
            SetPuzzleCompleted(true);
        }

        if (Unlocked)
        {
            return;
        }

        if (!TryGetKeyFromPlayer()) 
        {
            if (InteractableLock == null)
            {
                return;
            }

            InteractableLock.ShakeAnimation();
            return;
        }

        if (InteractableLock != null)
        {
            InteractableLock.UnlockAnimation(true);
        }

        Unlocked = true;
        //SetPuzzleCompleted(true);

        Panel.MaxRotation = UnlockedMaxRotation;
    }
}
