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
    private PickupItem KeyPrefab;

    [SerializeField]
    private Transform[] KeySpawnLocations;

    [SerializeField]
    private int KeyIndex = 0;

    private int SpawnIndex = -1;

    private PickupItem NewInstance;

    private void Update()
    {
        if (NewInstance == null) 
        {
            Debug.Log("Picked up!");
        }
    }

    public int SpawnLocationCount() 
    {
        return KeySpawnLocations.Length;
    }

    public void SetPuzzle(int index) 
    {
        SpawnIndex = index;
        SpawnNewKey();
    }

    private void SpawnNewKey() 
    {
        if (KeyPrefab != null)
        {
            Transform location = KeySpawnLocations[SpawnIndex];
            NewInstance = Instantiate(KeyPrefab, location.position, location.rotation);
        }
    }

    public override void OnReset()
    {
        if (Unlocked) 
        {
            return;
        }

        Panel.ResetInteractable();

        if (NewInstance == null) 
        {
            SpawnNewKey();
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
