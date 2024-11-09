using UnityEngine;

public class PuzzleArea : MonoBehaviour
{
    public Transform PuzzleCameraTransform;


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
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.current.ClearPuzzle(this);
    }
}
