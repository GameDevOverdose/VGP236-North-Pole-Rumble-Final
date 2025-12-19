using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using static PlayerManager;

public class GameStateManager : MonoBehaviour
{

    [System.Serializable]
    public enum GameState
    {
        MENU = 0,
        GAME = 1,
        GAME_WON = 2,
        GAME_PAUSED = 3,
        GAME_COUNTDOWN = 4
    }

    private static GameStateManager _instance = null;
    public static GameStateManager Instance { get { return _instance; } }

    private GameState _lastState = GameState.MENU;
    private GameState _currentGameState = GameState.MENU;
    public GameState CurrentGameState { get { return _currentGameState; } }

    [SerializeField] private CinemachineCamera menuCamera = null;
    [SerializeField] private CinemachineCamera gameCamera = null;
    [SerializeField] private CinemachineCamera winCamera = null;

    private int playersCurrentlyAlive;
    private int lastPlayerAliveIndex;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //RUNS EVERY FRAME
        switch (_currentGameState)
        {
            case GameState.GAME:
                CheckWinCondition();
                break;
            case GameState.GAME_WON:
                CheckWinInput();
                break;
            default:
                //Debug.LogError("Unhandled game state in GameStateManager Update");
                break;
        }

        //ONLY RUNS ON STATE CHANGE
        if(_lastState != _currentGameState)
        {
            switch (_currentGameState)
            {
                case GameState.MENU:

                    CameraManager.Instance.ActivateCamera(menuCamera);

                    StartCoroutine(UIManager.Instance.EnableMainMenu(0.0f));
                    StartCoroutine(UIManager.Instance.DisableGameUI(0.0f));
                    StartCoroutine(UIManager.Instance.DisableWinUI(0.0f));
                    StartCoroutine(UIManager.Instance.DisablePauseUI(0.0f));
                    break;

                case GameState.GAME:

                    CameraManager.Instance.ActivateCamera(gameCamera);

                    StartCoroutine(UIManager.Instance.DisableMainMenu(0.0f));
                    StartCoroutine(UIManager.Instance.EnableGameUI(0.0f));
                    StartCoroutine(UIManager.Instance.DisableWinUI(0.0f));
                    StartCoroutine(UIManager.Instance.DisablePauseUI(0.0f));

                    Time.timeScale = 1.0f;

                    break;

                case GameState.GAME_WON:

                    CameraManager.Instance.ActivateCamera(winCamera);

                    StartCoroutine(UIManager.Instance.DisableMainMenu(0.0f));
                    StartCoroutine(UIManager.Instance.DisableGameUI(0.0f));
                    StartCoroutine(UIManager.Instance.EnableWinUI(0.0f));
                    StartCoroutine(UIManager.Instance.DisablePauseUI(0.0f));
                    break;

                case GameState.GAME_PAUSED:

                    StartCoroutine(UIManager.Instance.DisableMainMenu(0.0f));
                    //StartCoroutine(UIManager.Instance.DisableGameUI(0.0f));
                    StartCoroutine(UIManager.Instance.DisableWinUI(0.0f));
                    StartCoroutine(UIManager.Instance.EnablePauseUI(0.0f));

                    Time.timeScale = 0.0f;

                    break;

                default:
                    Debug.LogError("Unhandled game state in GameStateManager Update");
                    break;
            }

            _lastState = _currentGameState;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_currentGameState == GameState.GAME)
            {
                _currentGameState = GameState.GAME_PAUSED;
            }    
            else if(_currentGameState == GameState.GAME_PAUSED)
            {
                _currentGameState = GameState.GAME;
            }
        }
    }

    public void CheckWinCondition()
    {
        playersCurrentlyAlive = 0;
        lastPlayerAliveIndex = -1;

        for (int i = 0; i < PlayerManager.Instance._activatedPlayers.Count; i++)
        {
            if (PlayerManager.Instance._activatedPlayers[i].GetComponent<PlayerHealth>().IsAlive)
            {
                playersCurrentlyAlive++;
                lastPlayerAliveIndex = i;
            }
        }

        if (playersCurrentlyAlive == 1)
        {
            _currentGameState = GameState.GAME_WON;
            Debug.Log($"Player {lastPlayerAliveIndex + 1} wins the game!");

            winCamera.Follow = PlayerManager.Instance._activatedPlayers[lastPlayerAliveIndex].transform;
        }
    }

    void CheckWinInput()
    {
        if (PlayerInputGetter.Instance.MeleeAttack(lastPlayerAliveIndex).triggered)
        {
            _currentGameState = GameState.MENU;
        }
    }

    public void SetGameState(GameState gameState)
    {
        _currentGameState = gameState;
    }

    public void SetGameState(int gameState)
    {
        if (System.Enum.IsDefined(typeof(GameState), gameState))
        {
            _currentGameState = (GameState)gameState;
        }
        else
        {
            Debug.LogWarning($"Invalid GameState value: {gameState}");
        }
    }

    public int ReturnWinningPlayerIndex()
    {
        if (_currentGameState == GameState.GAME_WON)
        {
            return lastPlayerAliveIndex;
        }
        else
        {
            //Debug.LogWarning("No winning player yet!");
            return -1;
        }
    }
}
