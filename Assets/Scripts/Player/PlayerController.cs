using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter PlayerCharacter;

    public Transform bind;

    public CustomInput ControllerInput;

    public float LookSpeed = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ControllerInput = InputManager.current.Input;
        InitializeBind();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void LateUpdate()
    {
        if (bind == null) 
        {
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
