using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowback : MonoBehaviour
{
    private Rigidbody _rigidbody = null;

    [SerializeField] private float _maxThrowbackFactor = 2.0f;
    private float _minThrowbackFactor = 1.0f;
    private float _currentThrowbackFactor = 1.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _currentThrowbackFactor = _minThrowbackFactor;
    }

    public void Throwback(Vector3 impactDirection, float forcePower, float _throwbackFactorIncrementRate)
    {
        Vector3 direction = impactDirection + (Vector3.up * 1.0f);
        direction.Normalize();

        _rigidbody.AddForce(direction * forcePower * _currentThrowbackFactor, ForceMode.VelocityChange);

        _currentThrowbackFactor += _throwbackFactorIncrementRate;
        Mathf.Clamp(_currentThrowbackFactor, _minThrowbackFactor, _maxThrowbackFactor);
    }    
}
