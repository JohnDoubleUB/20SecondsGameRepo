using UIManagerLibrary.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public SessionData SessionData;

    public delegate void CurrentCodeUpdate(string currentCodeUpdate);
    public event CurrentCodeUpdate OnCurrentCodeUpdate;

    public KeypadPuzzle KeyPad;

    public SimonSaysPuzzle SimonSays;
    public RadioPuzzle Radio;
    public WirePuzzle Wires;
    public SwitchPuzzle Switch;
    public LockedPuzzle Locked;

    public Transform MainMenuCameraPos;

    [SerializeField]
    private AudioSource MainAmbience;

    [SerializeField]
    private AudioSource MainOutroAmbience;

    public char PasswordGapCharacter = '_';

    public float DeathEffectAmount { get; private set; } = 0;
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

    public bool Paused { get; private set; }

    public bool GameIsWon { get; private set; } = false;

    [SerializeField]
    private DialogueSequenceData OutroSequence;

    public void PauseToggle() 
    {
        if (!GameStarted) 
        {
            return;
        }

        Paused = !Paused;

        UIManager.current.SetActiveContexts(Paused, "Pause","PauseMain");

        if (!Paused) 
        {
            UIManager.current.SetActiveContexts(false, true, "Options");
        }
    }

    public void EndGamePrematurely() 
    {
        CurrentTime = 0.01f;
    }

    public void CompleteGame() 
    {
        GameIsWon = true;
        RestartTimer = TimeBetweenRestarts * 2;
        MainAmbience.Stop();
        MainOutroAmbience.Play();
        EnableEnvironmentalEffects(false, false);
        DeathEffectAmount = 0;

    }

    public bool TryGetNextUnusedCode(out string code)
    {
        if (SessionData == null) 
        {
            code = null;
            return false;
        }

        bool result = SessionData.TryGetNextUnusedCode(out code);

        if (result) 
        {
            NotifyOfCodeUpdate();
        }

        return result;
    }

    private void NotifyOfCodeUpdate() 
    {
        string value = SessionData.CurrentCode.PadRight(SessionData.CodeCount * SessionData.CodeLength, PasswordGapCharacter);
        OnCurrentCodeUpdate?.Invoke(value);
    }

    private void PlayMainAmbience() 
    {
        if (MainAmbience == null) 
        {
            return;
        }

        MainAmbience.Play();
    }

    private void StopMainAmbience() 
    {
        if (MainAmbience == null)
        {
            return;
        }

        MainAmbience.Stop();
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UIManager.current.SetActiveContexts(true, true, "Menu", "MenuMain");
        //StartGame();
    }

    private void EnableEnvironmentalEffects(bool enable, bool clearParticlesImmediate = true) 
    {
        if(EnvironmentalEffectManager.current == null) 
        {
            return; 
        }

        EnvironmentalEffectManager.current.EnableEffects(enable, clearParticlesImmediate);
    }

    public void StopOutroAmbience() 
    {
        MainOutroAmbience.Stop();
    }

    public void StartGame() 
    {
        StopOutroAmbience();
        GameIsWon = false;
        GameStarted = true;
        
        PlayerCharacter.current.PlayDeath(false);

        SessionData newSession = new SessionData(5, 1)
        {
            LockedPuzzleSpawnLocationCount = Locked.SpawnLocationCount()
        };

        newSession.Initialize();

        SessionData = newSession;

        NotifyOfCodeUpdate();

        //Debug.Log("Codes! " + string.Join(", ", SessionData.GetAllCodes()));

        string completeCode = SessionData.GetCompleteCode();

        //Debug.Log("Complete Code! " + completeCode);

        if (KeyPad != null)
        {
            KeyPad.SetPassword(completeCode);
        }

        ResetTimer();

        InitializePuzzles();

        ResetPlayerProgress();


        ResetPlayerToStart();

        

        PlayerController.current.SetInGame(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.current.SetActiveContexts(true, "Game");

        EnableEnvironmentalEffects(true);

        PlayerController.current.BindToCameraToCharacter();
    }

    public void QuitToMenu(bool withEndingCutscene = false) 
    {
        EnableEnvironmentalEffects(false);
        GameIsWon = false;
        UnInitializeAllPuzzles();
        SessionData = null;
        GameStarted = false;

        ResetPlayerToStart();

        PlayerController.current.SetInGame(false);
        PlayerController.current.ResetPosition();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Paused = false;
        StopMainAmbience();

        UIManager.current.SetActiveContexts(false, true, "Game", "Pause", "PauseMain", "Options");

        if (withEndingCutscene) 
        {
            DialogueSequencePlayer.current.StartDialogueSequence(OutroSequence, () => { 
                UIManager.current.SetActiveContexts(true, true, "Menu", "MenuMain");
                UIElementManager.current.PauseOptions = false; //Just in case something goes wrong
                //MainOutroAmbience.Stop(); 
            });
            return;
        }

        StopOutroAmbience();
        UIManager.current.SetActiveContexts(true, "Menu", "MenuMain");
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

        //Keypad
        KeyPad.FullReset();
    }

    private void Update()
    {
        if (GameIsWon) 
        {
            if (RestartTimer > 0) 
            {
                RestartTimer = Mathf.Max(0, RestartTimer - Time.deltaTime);
                return;
            }

            //UnInitializeAllPuzzles();
            //PlayerController.current.SetInGame(false);
            QuitToMenu(true);


            return;
        }

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
        //Debug.Log("Restart timer is " + RestartTimer);
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
        EnableEnvironmentalEffects(true);

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
