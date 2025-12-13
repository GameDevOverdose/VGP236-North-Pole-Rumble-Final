using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance = null;
    public static GameStateManager Instance { get { return _instance; } }

    private bool _gameWon = false;
    public bool GameWon { get { return _gameWon; } }

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

    }

    public void CheckWinCondition()
    {
        int playersCurrentlyAlive = 0;
        int lastPlayerAliveIndex = -1;

        for (int i = 0; i < PlayerManager.Instance._playersInScene.Count; i++)
        {
            if (PlayerManager.Instance._playersInScene[i].GetComponent<PlayerHealth>().IsAlive)
            {
                playersCurrentlyAlive++;
                lastPlayerAliveIndex = i;
            }
        }

        if (playersCurrentlyAlive == 1)
        {
            _gameWon = true;
            Debug.Log($"Player {lastPlayerAliveIndex + 1} wins the game!");
        }
    }
}
