using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private int _playerNumber;

    private void Awake()
    {
        _playerNumber = PlayerManager.Instance.GetPlayerNumberIndex(gameObject);

        Spawn();
    }

    public void Spawn()
    {
        if (PlayerSpawnManager.Instance.GetPlayerSpawnPoint(_playerNumber) != null)
        {
            transform.position = PlayerSpawnManager.Instance.GetPlayerSpawnPoint(_playerNumber).position;
            transform.rotation = PlayerSpawnManager.Instance.GetPlayerSpawnPoint(_playerNumber).rotation;
        }
    }
}
