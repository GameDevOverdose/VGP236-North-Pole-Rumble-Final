using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-30)]
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance = null;
    public static PlayerManager Instance { get { return _instance; } }

    [SerializeField] private Transform _playerParentRoot;

    public enum PlayerNumber
    {
        Player_1,
        Player_2,
        Player_3,
        Player_4
    }

    private int playersSupported;
    public int PlayersSupported { get { return playersSupported; } }

    private List<GameObject> _playerObjectsInRoot = new List<GameObject>();
    public List <GameObject> _activatedPlayers = new List<GameObject>();
    private List <PlayerNumber> _assignedPlayerNumbers = new List<PlayerNumber>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        playersSupported = Enum.GetValues(typeof(PlayerNumber)).Length;

        foreach (Transform player in _playerParentRoot.transform)
        {
            player.gameObject.SetActive(false);
            _playerObjectsInRoot.Add(player.gameObject);
        }
    }

    public void SpawnPlayer(int amountOfPlayers)
    {
        _activatedPlayers.Clear();
        _assignedPlayerNumbers.Clear();

        for (int i = 0; i < _playerObjectsInRoot.Count; i++)
        {
            if(i < amountOfPlayers)
            {
                _activatedPlayers.Add(_playerObjectsInRoot[i]);

                _assignedPlayerNumbers.Add((PlayerNumber)i);
                Debug.Log($"Assigned {_activatedPlayers[i]} to {_assignedPlayerNumbers[i]}");

                _activatedPlayers[i].SetActive(true);

                if (!PlayersInSceneCheck())
                {
                    return;
                }

                _activatedPlayers[i].GetComponent<PlayerSpawn>().Initialize();
                _activatedPlayers[i].GetComponent<PlayerHealth>().Initialize();
            }
            else
            {
                _playerObjectsInRoot[i].SetActive(false);
            }
        }
    }

    public int GetPlayerNumberIndex(GameObject playerObject)
    {
        return (int)GetPlayerNumber(playerObject);
    }

    public PlayerNumber GetPlayerNumber(GameObject playerObject)
    {
        if(PlayersInSceneCheck())
        {
            for (int i = 0; i < _activatedPlayers.Count; i++)
            {
                if (_activatedPlayers[i] == playerObject)
                {
                    return _assignedPlayerNumbers[i];
                }
            }
        } 

        Debug.LogError($"Player {playerObject} not found in assigned players.");
        return default;
    }

    private bool PlayersInSceneCheck()
    {
        if (_activatedPlayers.Count > playersSupported)
        {
            Debug.Log($"Players in scene: {_activatedPlayers.Count}");
            Debug.Log($"Players supported: {playersSupported}");
            Debug.LogError($"More players detected in the scene! Only up to {playersSupported} players are supported.");
            return false;
        }
        return true;
    }
}
