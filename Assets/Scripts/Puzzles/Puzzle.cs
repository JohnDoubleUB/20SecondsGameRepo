using UnityEngine;

//The brain of each puzzle
public class Puzzle : MonoBehaviour
{
    [SerializeField]
    protected CodeDisplayer Displayer;

    private string PuzzleCode = null;
    public int PuzzleIndex { get; private set; } = -1;
    public bool PuzzleCompleted { get; private set; } = false;

    public void SetPuzzleCompleted(bool value) 
    {
        PuzzleCompleted = value;

        if (PuzzleCompleted) 
        {
            if (PuzzleCode == null && GameManager.current.SessionData.TryGetNextUnusedCode(out string code)) 
            {
                PuzzleCode = code;
                Displayer.SetText(code);
            }
        }
    }
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

    public virtual void ButtonValue(int value) 
    {

    }

    public virtual void ResetPuzzle(bool force = false)
    {
        PuzzleCode = null;
    }

    public virtual void FullReset() 
    {

    }

    public void SetIndex(int index)
    {
        if (PuzzleIndex == -1) 
        {
            PuzzleIndex = index;
        }
    }
}
