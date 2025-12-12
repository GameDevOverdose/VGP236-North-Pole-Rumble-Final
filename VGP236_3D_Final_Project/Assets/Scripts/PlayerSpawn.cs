using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private int _playerNumber;

    private void Awake()
    {
        _playerNumber = PlayerNumberAssigner.Instance.GetPlayerNumberIndex(gameObject);

        if(PlayerSpawnManager.Instance.GetPlayerSpawnPoint(_playerNumber) != null)
        {
            transform.position = PlayerSpawnManager.Instance.GetPlayerSpawnPoint(_playerNumber).position;
        }
    }
}
