using UnityEngine;

//The brain of each puzzle
public class Puzzle : MonoBehaviour

{

    public bool PuzzleCompleted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ButtonValue(string value)
    {

    }

    public virtual void SwitchValue(bool value, int index) 
    {

    }

    public virtual void ResetPuzzle()
    {

    }
}
