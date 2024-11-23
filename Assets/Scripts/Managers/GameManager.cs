using UIManagerLibrary.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public SessionData SessionData;

    public KeypadPuzzle KeyPad;

    public SimonSaysPuzzle SimonSays;
    public RadioPuzzle Radio;
    public WirePuzzle Wires;
    public SwitchPuzzle Switch;
    public LockedPuzzle Locked;

    public Transform MainMenuCameraPos;

    [SerializeField]
    private bool GameStarted = false;

    private bool SpawnLocationSet = false;

    public Vector3 SpawnLocation;
    public Vector2 SpawnRotation;

    public bool CompleteReset = true;
    public bool LoadEnvironmentAudio = true;

    public float timer = 5;
    public float CurrentTime { get; private set; }

    private void Awake()
    {
        if (current != null) Debug.LogWarning("Oops! it looks like there might already be a " + GetType().Name + " in this scene!");
        current = this;
    }

    private void Start()
    {
        if (LoadEnvironmentAudio)
        {
            SceneManager.LoadScene("Audio", LoadSceneMode.Additive);
        }

        if (!SpawnLocationSet && PlayerCharacter.current != null)
        {
            SpawnLocation = PlayerCharacter.current.transform.position;
            SpawnRotation = PlayerCharacter.current.GetCharacterRotation();
        }

        PlayerController.current.BindCameraToPoint(MainMenuCameraPos);

        UIManager.current.SetActiveContexts(true, true, "Menu");
        //StartGame();
    }

    public void StartGame() 
    {
        GameStarted = true;

        SessionData = new SessionData(5, 1)
        {
            LockedPuzzleSpawnLocationCount = Locked.SpawnLocationCount()
        };

        SessionData.Initialize();

        Debug.Log("Codes! " + string.Join(", ", SessionData.GetAllCodes()));

        string completeCode = SessionData.GetCompleteCode();
        Debug.Log("Complete Code! " + completeCode);

        if (KeyPad != null)
        {
            KeyPad.SetPassword(completeCode);
        }

        ResetTimer();

        InitializePuzzles();

        ResetPlayerProgress();


        ResetPlayerToStart();

        UIManager.current.SetActiveContexts(false, true, "Menu");
        UIManager.current.SetActiveContexts(true, "Game");

        PlayerController.current.SetInGame(true);
        PlayerController.current.BindToCameraToCharacter();
    }

    public void QuitToMenu() 
    {
        UnInitializeAllPuzzles();
        SessionData = null;
        GameStarted = false;
    }

    private void InitializePuzzles() 
    {
        //SimonSays
        SimonSays.SetPuzzle(SessionData.SimonSaysPattern);
        SimonSays.SetIndex(0);

        //RadioPuzzle
        Radio.SetPuzzle(SessionData.RadioData);
        Radio.SetIndex(1);

        //SwitchPuzzle
        Switch.SetPuzzle(SessionData.SwitchData);
        Switch.SetIndex(2);

        //LockPuzzle
        Locked.SetPuzzle(SessionData.LockedPuzzleKeySpawnIndex);
        Locked.SetIndex(3);

        //WiresPuzzle
        Wires.SetPuzzle(SessionData.WireIndexOrder);
        Wires.SetIndex(4);
    }

    private void UnInitializeAllPuzzles() 
    {
        //SimonSays
        SimonSays.FullReset();

        //RadioPuzzle
        Radio.FullReset();

        //SwitchPuzzle
        Switch.FullReset();

        //LockPuzzle
        Locked.FullReset();

        //WiresPuzzle
        Wires.FullReset();
    }

    private void Update()
    {
        if (!GameStarted) 
        {
            return;
        }

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
        //Exta stuff for death
        ResetGame();
    }

    public void ResetAllPuzzles() 
    {
        KeyPad.ResetPuzzle();
        Locked.ResetPuzzle();
        Wires.ResetPuzzle();
        Switch.ResetPuzzle();
        SimonSays.ResetPuzzle();
        Radio.ResetPuzzle();
    }

    public void ResetPlayerProgress() 
    {
        if (PlayerController.current != null)
        {
            PlayerController.current.ResetProgress();
        }
    }

    public void ResetPlayerToStart() 
    {
        try
        {
            //Huh?
            if (PlayerCharacter.current != null && SpawnLocation != null && SpawnRotation != null)
            {
                PlayerCharacter.current.SetCharacterPosition(SpawnLocation, SpawnRotation);
            }

            if (PlayerController.current != null)
            {
                PlayerController.current.ClearPuzzle();
            }
        }
        catch { }
    }

    public void ResetTimer()
    {
        CurrentTime = timer;
    }

    public void ResetGame()
    {
        ResetTimer();

        ResetAllPuzzles();

        ResetPlayerProgress();

        if (CompleteReset)
        {
            ResetPlayerToStart();
        }
    }
}
