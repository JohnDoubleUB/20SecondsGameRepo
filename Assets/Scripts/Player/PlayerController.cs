using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public enum EControllerState
{
    CharacterControl,
    PuzzleControl,
    None
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;

    public delegate void ItemsHeldUpdate(PickedUpItem item, bool pickupOrDrop);
    public event ItemsHeldUpdate OnItemsHeldUpdate;

    public PlayerCharacter PlayerCharacter;

    public Transform bind;

    public CustomInput ControllerInput;

    public PuzzleArea CurrentPuzzleArea;

    public MouseInteractor PositionInteractor;

    public PickupInteractor PickupInteractor;

    public float LookSpeed = 0.2f;

    public List<PickedUpItem> HeldItems = new List<PickedUpItem>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public EControllerState State { get; private set; } = EControllerState.CharacterControl;

    //Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    public bool PuzzleAreaActive
    {
        get
        {
            return ValidPuzzleArea();
        }
    }

    public bool TryAndRemoveItemWithIndex(int indexToFind)
    {
        for (int i = 0; i < HeldItems.Count; i++) 
        {
            if (HeldItems[i].Index == indexToFind) 
            {
                OnItemsHeldUpdate?.Invoke(HeldItems[i], false);
                HeldItems.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }
    
    void Start()
    {
        ControllerInput = InputManager.current.Input;
        InitializeBind();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetPuzzle(PuzzleArea puzzleArea)
    {
        if(CurrentPuzzleArea != null)
        {
            return;
        }

        CurrentPuzzleArea = puzzleArea;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClearPuzzle(PuzzleArea puzzleArea)
    {
        if (puzzleArea == CurrentPuzzleArea)
        {
            CurrentPuzzleArea = null;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PositionInteractor.SetButton(false);
    }

    public void ClearPuzzle() 
    {
        CurrentPuzzleArea = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PositionInteractor.SetButton(false);
    }

    // Update is called once per frame
    void Update()
    {
    }


    private bool ValidPuzzleArea()
    {
        if (CurrentPuzzleArea == null)
        {
            return false;
        }

        float dot = Vector3.Dot(transform.forward, (CurrentPuzzleArea.transform.position - transform.position).normalized);

        return dot > 0.7f;
    }

    private void LateUpdate()
    {
        if (bind == null) 
        {
            return;
        }

        bool validPuzzleArea = ValidPuzzleArea();

        if (PickupInteractor != null && PickupInteractor.Enabled == validPuzzleArea) 
        {
            PickupInteractor.EnablePickupInteraction(!validPuzzleArea);
        }

        if (validPuzzleArea)
        {
            transform.SetPositionAndRotation(CurrentPuzzleArea.PuzzleCameraTransform.position, CurrentPuzzleArea.PuzzleCameraTransform.rotation);
            return;
        }

        transform.SetPositionAndRotation(bind.position, bind.rotation);
    }
    private void InitializeBind() 
    {
        if (PlayerCharacter == null)
        {
            Debug.Log("Error, there doesn't appear to be a player character attached to this!");
            return;
        }

        ControllerInput.Player.Attack.performed += OnAttackPerformed;
        ControllerInput.Player.Attack.canceled += OnAttackCanceled;

        PlayerCharacter.InitializeBind(this);

        bind = PlayerCharacter.CameraTransform;
    }

    private void UninitializeBind() 
    {
        ControllerInput.Player.Attack.performed -= OnAttackPerformed;
        ControllerInput.Player.Attack.canceled -= OnAttackCanceled;
    }

    private void OnAttackPerformed(CallbackContext obj)
    {
        if(!ValidPuzzleArea())
        {
            if(PickupInteractor != null && PickupInteractor.TryGetPickup(out PickedUpItem pickedUpItem))
            {
                HeldItems.Add(pickedUpItem);
                OnItemsHeldUpdate?.Invoke(pickedUpItem, true);
            }

            return;
        }

        PositionInteractor.SetButton(true);
    }

    private void OnAttackCanceled(CallbackContext obj)
    {
        if (!ValidPuzzleArea())
        {
            return;
        }

        PositionInteractor.SetButton(false);
    }

    private void OnDestroy()
    {
        UninitializeBind();
    }
}
