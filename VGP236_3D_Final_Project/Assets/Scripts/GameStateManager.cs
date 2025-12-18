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
        INGAME = 1,
        GAMEWON = 2
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
        switch (_currentGameState)
        {
            case GameState.MENU:
                break;
            case GameState.INGAME:
                CheckWinCondition();
                break;
            case GameState.GAMEWON:
                CheckWinInput();
                break;
            default:
                Debug.LogError("Unhandled game state in GameStateManager Update");
                break;
        }

        if(_lastState != _currentGameState)
        {
            switch (_currentGameState)
            {
                case GameState.MENU:
                    CameraManager.Instance.ActivateCamera(menuCamera);

                    StartCoroutine(UIManager.Instance.EnableMainMenu(0.0f));
                    StartCoroutine(UIManager.Instance.DisableGameUI(0.0f));
                    StartCoroutine(UIManager.Instance.DisableWinUI(0.0f));
                    break;
                case GameState.INGAME:
                    CameraManager.Instance.ActivateCamera(gameCamera);

                    StartCoroutine(UIManager.Instance.DisableMainMenu(0.0f));
                    StartCoroutine(UIManager.Instance.EnableGameUI(0.0f));
                    StartCoroutine(UIManager.Instance.DisableWinUI(0.0f));
                    break;
                case GameState.GAMEWON:
                    CameraManager.Instance.ActivateCamera(winCamera);

                    StartCoroutine(UIManager.Instance.DisableMainMenu(0.0f));
                    StartCoroutine(UIManager.Instance.DisableGameUI(0.0f));
                    StartCoroutine(UIManager.Instance.EnableWinUI(0.0f));
                    break;
                default:
                    Debug.LogError("Unhandled game state in GameStateManager Update");
                    break;
            }

            _lastState = _currentGameState;
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
            _currentGameState = GameState.GAMEWON;
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
        if (_currentGameState == GameState.GAMEWON)
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
