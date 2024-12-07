using UnityEngine;

//The brain of each puzzle
public class Puzzle : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem SuccessParticleSystem;

    [SerializeField]
    protected AudioSource SuccessPlayer;

    [SerializeField]
    protected CodeDisplayer Displayer;

    [SerializeField]
    protected bool ShouldShowCodepart = true;

    [SerializeField]
    private bool CompletingShouldFinishGame = false;

    protected string PuzzleCode = null;
    public int PuzzleIndex { get; private set; } = -1;
    public bool PuzzleCompleted { get; private set; } = false;

    public void SetPuzzleCompleted(bool value) 
    {
        PuzzleCompleted = value;

        if (PuzzleCompleted)
        {
            if (!ShouldShowCodepart)
            {
                SuccessPlayer.Play();

                if (SuccessParticleSystem != null) 
                {
                    SuccessParticleSystem.Play();
                }

                if (CompletingShouldFinishGame) 
                {
                    GameManager.current.CompleteGame();
                }

                return;
            }

            if (PuzzleCode == null && GameManager.current.TryGetNextUnusedCode(out string code))
            {
                PuzzleCode = code;

                if (SuccessPlayer != null) 
                {
                    SuccessPlayer.Play();
                }

                if (SuccessParticleSystem != null) 
                {
                    SuccessParticleSystem.Play();
                }

                Displayer.SetText(code);
            }
        }
        else 
        {
            PuzzleCode = null;
            Displayer.SetText("");
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

    public void ResetPuzzle()
    {
        if (PuzzleCompleted) 
        {
            return;
        }

        OnReset();
    }

    protected virtual void OnReset() 
    {

    }

    public virtual void FullReset() 
    {
        if (SuccessPlayer != null) 
        {
            SuccessPlayer.Stop();
        }

        PuzzleCode = null;
        Displayer.SetText("");
        PuzzleCompleted = false;
        OnFullReset();
    }

    protected virtual void OnFullReset() 
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
