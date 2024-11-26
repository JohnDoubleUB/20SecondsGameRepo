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
    private AudioSource MainAmbience;

    public float DeathEffectAmount { get; private set; } = 0;

    [SerializeField]
    public bool GameStarted { get; private set; }

    private bool SpawnLocationSet = false;

    public Vector3 SpawnLocation;
    public Vector2 SpawnRotation;

    public bool CompleteReset = true;
    public bool LoadEnvironmentAudio = true;

    public float timer = 5;

    public float TimeBetweenRestarts = 5f;

    private float RestartTimer = 0;
    public float CurrentTime { get; private set; }

    private void PlayMainAmbience() 
    {
        if (MainAmbience == null) 
        {
            return;
        }

        MainAmbience.Play();
    }

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

        PlayerCharacter.current.PlayDeath(false);

        SessionData newSession = new SessionData(5, 1)
        {
            LockedPuzzleSpawnLocationCount = Locked.SpawnLocationCount()
        };

        newSession.Initialize();

        SessionData = newSession;

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
            DeathEffectAmount = 0;
            return;
        }

        KeyPad.CodeIsValid = SessionData.AllCodesGiven();

        //Debug.Log("Time " + RestartTimer);

        if (RestartTimer > 0)
        {
            RestartTimer = Mathf.Max(0, RestartTimer - Time.deltaTime);

            if (RestartTimer == 0) 
            {
                ResetGame();
            }

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

        if (CurrentTime > 3)
        {
            DeathEffectAmount = CurrentTime.Remap(20, 3, 0, 0.2f);
        }
        else 
        {
            DeathEffectAmount = CurrentTime.Remap(3, 0.3f, 0.2f, 1f);
        }
    }

    public void TriggerPlayerDeath() 
    {

        //Exta stuff for death
        if (PlayerController.current != null)
        {
            PlayerController.current.ClearPuzzle();
        }

        PlayerCharacter.current.PlayDeath(true);
        RestartTimer = TimeBetweenRestarts;
        Debug.Log("Restart timer is " + RestartTimer);
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

            UIElementManager.current.ToggleCodeDisplayCenter(false);
        }
        catch { }
    }

    public void ResetTimer()
    {
        PlayMainAmbience();
        CurrentTime = timer;
    }

    public void ResetGame()
    {
        ResetTimer();

        PlayerCharacter.current.PlayDeath(false);

        ResetAllPuzzles();

        ResetPlayerProgress();

        if (CompleteReset)
        {
            ResetPlayerToStart();
        }
    }
}
