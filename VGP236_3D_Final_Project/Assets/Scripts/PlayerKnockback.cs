using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private Rigidbody _rigidbody = null;

    [SerializeField] private float _maxKnockbackFactor = 2.0f;
    private float _minKnockbackFactor = 1.0f;
    private float _currentKnockbackFactor = 1.0f;

    public float CurrentKnockbackFactor { get => _currentKnockbackFactor; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        _currentKnockbackFactor = _minKnockbackFactor;
    }

    public void Knockback(Vector3 impactDirection, float forcePower, float _knockbackFactorIncrementRate)
    {
        Vector3 direction = impactDirection + (Vector3.up * 1.0f);
        direction.Normalize();

        _rigidbody.AddForce(direction * forcePower * _currentKnockbackFactor, ForceMode.VelocityChange);

        _currentKnockbackFactor += _knockbackFactorIncrementRate;
        Mathf.Clamp(_currentKnockbackFactor, _minKnockbackFactor, _maxKnockbackFactor);
    }    
}
