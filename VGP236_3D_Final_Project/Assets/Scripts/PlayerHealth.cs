using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int _maxHealth = 3;
    private int _minHealth = 0;
    public int MaxHealth { get { return _maxHealth; } }

    private int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } }

    public bool IsAlive { get { return _currentHealth > _minHealth; } }

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        _currentHealth = _maxHealth;
    }

    public void IncrementHealth(int amount)
    {
        _currentHealth += amount;
        Mathf.Clamp(_currentHealth, _minHealth, _maxHealth);

        //GameStateManager.Instance.CheckWinCondition();
    }
}
