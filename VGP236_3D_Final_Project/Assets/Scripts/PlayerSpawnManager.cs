using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class PlayerSpawnManager : MonoBehaviour
{
    private static PlayerSpawnManager _instance;
    public static PlayerSpawnManager Instance { get { return _instance; } }

    private List<Transform> playerSpawnPoints = new List<Transform>();

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

        foreach (Transform child in transform)
        {
            playerSpawnPoints.AddRange(child.GetComponentsInChildren<Transform>());
        }
    }

    public Transform GetPlayerSpawnPoint(int index)
    {
        if (index < 0 || index >= playerSpawnPoints.Count)
        {
            Debug.LogError("Invalid spawn point index");
            return null;
        }

        return playerSpawnPoints[index];
    }
}
