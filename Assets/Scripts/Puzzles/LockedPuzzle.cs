using System;
using UnityEngine;

public class LockedPuzzle : Puzzle
{
    [SerializeField]
    private MouseInteractablePanel Panel;

    [SerializeField]
    private float LockedMaxRotation = 3f;

    [SerializeField]
    private float UnlockedMaxRotation = 180f;

    [SerializeField]
    private MouseInteractableLock InteractableLock;

    [SerializeField]
    private int KeyIndex = 0;

    public override void ResetPuzzle()
    {
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

        SetPuzzleCompleted(true);

        Panel.MaxRotation = UnlockedMaxRotation;
    }
}
