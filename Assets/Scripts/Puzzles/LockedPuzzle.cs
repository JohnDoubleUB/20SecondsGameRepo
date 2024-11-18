using UnityEngine;

public class LockedPuzzle : Puzzle
{
    [SerializeField]
    private MouseInteractablePanel Panel;

    [SerializeField]
    private float LockedMaxRotation = 5f;

    [SerializeField]
    private float UnlockedMaxRotation = 180f;


}
