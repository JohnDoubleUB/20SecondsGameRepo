using UnityEngine;

public class PuzzleArea : MonoBehaviour
{
    public Transform PuzzleCameraTransform;

    public bool EnableCenteredCode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController.current.SetPuzzle(this);
        
        if (EnableCenteredCode) 
        {
            UIElementManager.current.ToggleCodeDisplayCenter(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.current.ClearPuzzle(this);

        if (EnableCenteredCode)
        {
            UIElementManager.current.ToggleCodeDisplayCenter(false);
        }
    }
}
