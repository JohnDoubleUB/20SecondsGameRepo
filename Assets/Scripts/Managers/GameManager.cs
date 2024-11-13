using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public SessionData SessionData;

    public KeypadPuzzle KeyPad;

    private bool SpawnLocationSet = false;

    public Vector3 SpawnLocation;
    public Vector2 SpawnRotation;

    public bool CompleteReset = true;

    public float timer = 5;
    public float CurrentTime { get; private set; }

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    private void Start()
    {

        SceneManager.LoadScene("Audio", LoadSceneMode.Additive);
        if(!SpawnLocationSet && PlayerCharacter.current != null)
        {
            SpawnLocation = PlayerCharacter.current.transform.position;
            SpawnRotation = PlayerCharacter.current.GetCharacterRotation();
        }

        SessionData = new SessionData(4, 1);

        Debug.Log("Codes! " + string.Join(", ", SessionData.GetAllCodes()));

        string completeCode = SessionData.GetCompleteCode();
        Debug.Log("Complete Code! " + completeCode);

        if(KeyPad != null) 
        {
            KeyPad.SetPassword(completeCode);
        }

        ResetTimer();
    }

    private void Update()
    {
        if (CurrentTime > 0)
        {
            CurrentTime = Mathf.Max(0, CurrentTime - Time.deltaTime);
        }
        else 
        {
            TriggerPlayerDeath();
        }
    }

    public void TriggerPlayerDeath() 
    {
        ResetTimer();

        KeyPad.ResetPuzzle();

        if (!CompleteReset)
        {
            return;
        }

        if (PlayerController.current != null)
        {
            PlayerController.current.ClearPuzzle();
        }

        ResetGame();
    }

    public void ResetTimer()
    {
        CurrentTime = timer;
    }

    public void ResetGame()
    {
        if (PlayerCharacter.current != null)
        {
            PlayerCharacter.current.SetCharacterPosition(SpawnLocation, SpawnRotation);
        }
    }
}
