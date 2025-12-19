using UnityEngine;

[DefaultExecutionOrder(-10)]
public class PlayerNumber : MonoBehaviour
{
    private PlayerManager.PlayerNumber _currentPlayerNumber;
    public PlayerManager.PlayerNumber CurrentPlayerNumber { get { return _currentPlayerNumber; } }

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _currentPlayerNumber = PlayerManager.Instance.GetPlayerNumber(this.gameObject);
    }
}
