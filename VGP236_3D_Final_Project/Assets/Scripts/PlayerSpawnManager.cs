using UnityEngine;

[DefaultExecutionOrder(-20)]
public class PlayerSpawnManager : MonoBehaviour
{
    private static PlayerSpawnManager _instance;
    public static PlayerSpawnManager Instance { get { return _instance; } }

    [SerializeField] private Transform[] playerSpawnPoints = new Transform[4];

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

    public Transform GetPlayerSpawnPoint(int index)
    {
        if (index < 0 || index >= playerSpawnPoints.Length)
        {
            Debug.LogError("Invalid spawn point index");
            return null;
        }

        return playerSpawnPoints[index];
    }
}
