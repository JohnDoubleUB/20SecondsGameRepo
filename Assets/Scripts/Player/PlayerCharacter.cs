using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter current;
    public Vector2 MoveVector { get; private set; } = Vector2.zero;
    public Vector2 LookVector { get; private set; } = Vector2.zero;
    public float NormalizedWalkVelocity { get; private set; } = 0;

    public PlayerController PlayerController;


    [HideInInspector]
    public Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    private Vector2 Velocity;


    public float CrouchingAmount = 4f;
    public float Speed = 4f;
    public float CrouchingSpeed = 2f;
    public float JumpSpeed = 8.0f;
    public float Gravity = 20.0f;
    public Transform CameraTransform;
    public float LookXLimit = 90.0f;

    private CharacterController characterController;

    private float currentSpeed;

    public bool IsGrounded { get { return characterController.isGrounded; } }
    public bool IsJumping { get { return isJumping; } }
    public bool IsCrouching { get { return isCrouching; } }
    private bool isJumping;
    private bool isCrouching;
    private bool groundedLastFrame;

    public Vector2 GetCharacterRotation()
    {
        return rotation;
    }

    public void SetCharacterPosition(Vector3 position, Vector2 rotation)
    {
        characterController.enabled = false;
        transform.position = position;
        characterController.enabled = true;
        this.rotation = rotation;
    }

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
        
    }

    private void Reset()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentSpeed = isCrouching ? CrouchingSpeed : Speed;
    }

    private float CalculateNormalizedWalkVelocity(Vector3 currentVelocity)
    {
        return Mathf.Clamp(Mathf.Sqrt(currentVelocity.x * currentVelocity.x + currentVelocity.z * currentVelocity.z), 0, currentSpeed)
            .Remap(0, currentSpeed, 0, 1);
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveDirection = GetMovementDirection();

        if (characterController.isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
            }

            if (PlayerController.ControllerInput.Player.Jump.triggered)
            {
                moveDirection.y = JumpSpeed;
                isJumping = true;
            }
            else
            {
                moveDirection.y = -1000 * Time.deltaTime;
            }
        }
        else
        {
            if (groundedLastFrame) moveDirection.y = Mathf.Max(moveDirection.y, 0);
            moveDirection.y -= Gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        Vector2 wantedVelocity = LookVector * PlayerController.LookSpeed;

        Velocity = wantedVelocity;

        rotation += Velocity;
        rotation.x = Mathf.Clamp(rotation.x, -LookXLimit, LookXLimit);
        CameraTransform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, rotation.y);



        groundedLastFrame = characterController.isGrounded;

        NormalizedWalkVelocity = CalculateNormalizedWalkVelocity(characterController.velocity);
    }

    private float GetAppliedSpeed(bool isGrounded)
    {
        return isGrounded ? currentSpeed : currentSpeed * 0.8f;
    }

    private Vector3 GetMovementDirection()
    {
        float appliedSpeed = GetAppliedSpeed(characterController.isGrounded);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = appliedSpeed * MoveVector.y;
        float curSpeedY = appliedSpeed * MoveVector.x;
        Vector3 newMoveDirection = (forward * curSpeedX) + (right * curSpeedY);
        newMoveDirection.y = Mathf.Min(moveDirection.y, characterController.velocity.y);

        return newMoveDirection;
    }

    private void OnMovementPerformed(InputAction.CallbackContext callbackContext)
    {
        MoveVector = callbackContext.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext callbackContext)
    {
        MoveVector = callbackContext.ReadValue<Vector2>();
    }

    private void OnLookPerformed(InputAction.CallbackContext callbackContext)
    {
        Vector2 v = callbackContext.ReadValue<Vector2>();
        LookVector = new Vector2(v.y, v.x);
    }

    private void OnLookCanceled(InputAction.CallbackContext callbackContext)
    {
        Vector2 v = callbackContext.ReadValue<Vector2>();
        LookVector = new Vector2(v.y, v.x);
    }

    public void InitializeBind(PlayerController playerController)
    {
        PlayerController = playerController;

        PlayerController.ControllerInput.Player.Move.performed += OnMovementPerformed;
        PlayerController.ControllerInput.Player.Move.canceled += OnMovementCanceled;

        PlayerController.ControllerInput.Player.Look.performed += OnLookPerformed;
        PlayerController.ControllerInput.Player.Look.canceled += OnLookCanceled;

        //PlayerController.ControllerInput.Player.Movement.performed += OnMovementPerformed;
        //PlayerController.ControllerInput.Player.Movement.canceled += OnMovementCanceled;

        //PlayerController.ControllerInput.Player.Look.performed += OnLookPerformed;
        //PlayerController.ControllerInput.Player.Look.canceled += OnLookCanceled;

        //PlayerController.ControllerInput.Player.ReloadLeft.performed += OnReloadLeftPerformed;
        //PlayerController.ControllerInput.Player.ReloadRight.performed += OnReloadRightPerformed;

        //PlayerController.ControllerInput.Player.Fire.performed += OnFireRightPerformed;
        //PlayerController.ControllerInput.Player.Fire.canceled += OnCancelRight;

        //PlayerController.ControllerInput.Player.AltFire.performed += OnFireLeftPerformed;
        //PlayerController.ControllerInput.Player.AltFire.canceled += OnCancelLeft;

        //PlayerController.ControllerInput.Player.Interact.performed += OnInteractPerformed;

        //PlayerController.ControllerInput.Player.SwitchItems.performed += OnSwitchItemsPerformed;
        //PlayerController.ControllerInput.Player.DropSelected.performed += OnDropSelectedItemPerformed;

        //PlayerController.ControllerInput.Player.ToggleEquipment.performed += OnToggleEquipmentPerformed;
        //PlayerController.ControllerInput.Player.DropSelectedEquipment.performed += OnDropSelectedEquipmentPerformed;

        //PlayerController.ControllerInput.Player.Back.performed += OnBackPerformed;

        //Keyboard.current.onTextInput += OnTextInput;

        ////Interactor binds
        //Interactor.OnInteractableChanged += OnInteractableChanged;
    }
}
