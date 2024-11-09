using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;

    public PlayerCharacter PlayerCharacter;

    public Transform bind;

    public CustomInput ControllerInput;

    public PuzzleArea CurrentPuzzleArea;

    public float LookSpeed = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool PuzzleAreaActive
    {
        get
        {
            return ValidPuzzleArea();
        }
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
    }

    public void SetPuzzle(PuzzleArea puzzleArea)
    {
        if(CurrentPuzzleArea != null)
        {
            return;
        }

        CurrentPuzzleArea = puzzleArea;
    }

    public void ClearPuzzle(PuzzleArea puzzleArea)
    {
        if(puzzleArea == CurrentPuzzleArea)
        {
            CurrentPuzzleArea = null;
        }
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


        if (ValidPuzzleArea())
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

        PlayerCharacter.InitializeBind(this);

        bind = PlayerCharacter.CameraTransform;
    }
}
