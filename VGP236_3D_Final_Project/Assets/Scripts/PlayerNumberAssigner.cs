using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-30)]
public class PlayerNumberAssigner : MonoBehaviour
{
    private static PlayerNumberAssigner _instance = null;
    public static PlayerNumberAssigner Instance { get { return _instance; } }

    public enum PlayerNumber
    {
        Player_1,
        Player_2,
        Player_3,
        Player_4
    }

    private int playersSupported;
    public int PlayersSupported { get { return playersSupported; } }

    private List <GameObject> _playersInScene = new List<GameObject>();
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

        InitializePlayers();
    }

    private void InitializePlayers()
    {
        PlayerController[] playerControllers = GameObject.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        for (int i = 0; i < playerControllers.Length; i++)
        {
            _playersInScene.Add(playerControllers[i].gameObject);
        }

        playersSupported = Enum.GetValues(typeof(PlayerNumber)).Length;

        if(!PlayersInSceneCheck())
        {
            return;
        }

        for (int i = 0; i < _playersInScene.Count; i++)
        {
            _assignedPlayerNumbers.Add((PlayerNumber)i);
            Debug.Log($"Assigned {_playersInScene[i]} to {_assignedPlayerNumbers[i]}");
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
            for (int i = 0; i < _playersInScene.Count; i++)
            {
                if (_playersInScene[i] == playerObject)
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
        if (_playersInScene.Count > playersSupported)
        {
            Debug.Log($"Players in scene: {_playersInScene.Count}");
            Debug.Log($"Players supported: {playersSupported}");
            Debug.LogError($"More players detected in the scene! Only up to {playersSupported} players are supported.");
            return false;
        }
        return true;
    }
}
