using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager current;
    public CustomInput Input;

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
        Input = new CustomInput();
    }

    private void OnEnable()
    {
        Input.Enable();
    }
    private void OnDisable()
    {
        Input.Disable();
    }
}