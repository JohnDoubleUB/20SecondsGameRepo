using NUnit.Framework;
using System;
using System.Collections.Generic;
using UIManagerLibrary.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    private Vector3 InitialPosition;

    public static PlayerController current;

    public delegate void SkipButtonPressed();
    public event SkipButtonPressed OnSkipButtonPressed;

    public delegate void ItemsHeldUpdate(PickedUpItem item, bool pickupOrDrop);
    public event ItemsHeldUpdate OnItemsHeldUpdate;

    public delegate void ResetInventory();
    public event ResetInventory OnResetInventory;

    public PlayerCharacter PlayerCharacter;

    public Transform bind;

    protected CustomInput ControllerInput;

    public PuzzleArea CurrentPuzzleArea;

    public MouseInteractor PositionInteractor;

    public PickupInteractor PickupInteractor;

    public float LookSpeed = 0.2f;

    public List<PickedUpItem> HeldItems = new List<PickedUpItem>();

    private bool Paused { get { return GameManager.current.Paused; } }
    private bool InPuzzle;

    public bool PuzzleAreaActive
    {
        get
        {
            return ValidPuzzleArea();
        }
    }

    public void ResetPosition() 
    {
        transform.position = InitialPosition;
    }

    public void DisablePlayerInput(bool value) 
    {
        if (value)
        {
            ControllerInput.Player.Disable();
        }
        else 
        {
            ControllerInput.Player.Enable();
        }
    }

    public void SetInGame(bool value)
    {
        if (value)
        {
            ControllerInput.Player.Enable();
        }
        else
        {
            ControllerInput.Player.Disable();
        }
    }

    public bool JumpTriggered() 
    {
        if(Paused) 
        {
            return false;
        }

        return ControllerInput.Player.Jump.triggered;
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

    private void SetIsInPuzzle(bool value) 
    {
        InPuzzle = value;

        if (!InPuzzle) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ResetProgress()
    {
        OnResetInventory?.Invoke();
        HeldItems.Clear();
    }

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;

        InitialPosition = transform.position;
    }

    void Start()
    {
        ControllerInput = InputManager.current.Input;
        InitializeBind();

        ControllerInput.Player.Disable();

        ControllerInput.PauseMenu.Pause.performed += OnPausePerformed;
        ControllerInput.PauseMenu.Skip.performed += SkipPerformed;
    }

    private void SkipPerformed(CallbackContext obj)
    {
        OnSkipButtonPressed?.Invoke();
    }

    private void OnPausePerformed(CallbackContext obj)
    {
        GameManager.current.PauseToggle();
        PlayerCharacter.Movement(Vector2.zero);
        PlayerCharacter.Look(Vector2.zero);
    }

    public void SetPuzzle(PuzzleArea puzzleArea)
    {
        if (CurrentPuzzleArea != null)
        {
            return;
        }

        CurrentPuzzleArea = puzzleArea;
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

    public void BindToCameraToCharacter()
    {
        bind = PlayerCharacter.CameraTransform;
    }

    public void BindCameraToPoint(Transform transform) 
    {
        bind = transform;
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
        if (!GameManager.current.GameStarted) 
        {
            if (!Cursor.visible) 
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            return;
        }

        if (bind == null || bind.gameObject == GameManager.current.MainMenuCameraPos)
        {
            return;
        }
      
        bool validPuzzleArea = ValidPuzzleArea();

        if (Paused || validPuzzleArea)
        {
            if (!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else 
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

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

        ControllerInput.Player.Move.performed += OnMovement;
        ControllerInput.Player.Move.canceled += OnMovement;

        ControllerInput.Player.Look.performed += OnLook;
        ControllerInput.Player.Look.canceled += OnLook;

        PlayerCharacter.InitializeBind(this);

        //bind = PlayerCharacter.CameraTransform;
    }

    private void OnMovement(InputAction.CallbackContext callbackContext)
    {
        if (PlayerCharacter == null || Paused) 
        {
            PlayerCharacter.Movement(Vector2.zero);
            return;
        }

        PlayerCharacter.Movement(callbackContext.ReadValue<Vector2>());
    }

    private void OnLook(InputAction.CallbackContext callbackContext)
    {
        if (PlayerCharacter == null || Paused)
        {
            PlayerCharacter.Look(Vector2.zero);
            return;
        }

        Vector2 v = callbackContext.ReadValue<Vector2>();
        PlayerCharacter.Look(new Vector2(v.y, v.x));
    }

    private void UninitializeBind() 
    {
        ControllerInput.Player.Attack.performed -= OnAttackPerformed;
        ControllerInput.Player.Attack.canceled -= OnAttackCanceled;

        ControllerInput.PauseMenu.Pause.performed -= OnPausePerformed;
        ControllerInput.PauseMenu.Skip.performed -= SkipPerformed;
        ControllerInput.Player.Move.performed -= OnMovement;
        ControllerInput.Player.Move.canceled -= OnMovement;
        ControllerInput.Player.Look.performed -= OnLook;
        ControllerInput.Player.Look.canceled -= OnLook;

    }

    private void OnAttackPerformed(CallbackContext obj)
    {
        if (Paused) 
        {
            return;
        }

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
        if (Paused)
        {
            return;
        }

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
