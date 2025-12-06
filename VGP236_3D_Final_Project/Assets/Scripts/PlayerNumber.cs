using UnityEngine;

[DefaultExecutionOrder(-10)]
public class PlayerNumber : MonoBehaviour
{
    private PlayerNumberAssigner.PlayerNumber _currentPlayerNumber;
    public PlayerNumberAssigner.PlayerNumber CurrentPlayerNumber { get { return _currentPlayerNumber; } }

    private void Awake()
    {
        _currentPlayerNumber = PlayerNumberAssigner.Instance.GetPlayerNumber(this.gameObject);
    }
}
