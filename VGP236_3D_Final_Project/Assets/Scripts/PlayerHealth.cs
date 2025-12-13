using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int _maxHealth = 3;
    public int MaxHealth { get { return _maxHealth; } }

    private int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } }

    public bool IsAlive { get { return _currentHealth > 0; } }

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void IncrementHealth(int amount)
    {
        _currentHealth += amount;
        Mathf.Clamp(_currentHealth, 0, _maxHealth);
    }
}
