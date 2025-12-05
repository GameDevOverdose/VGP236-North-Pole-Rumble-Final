using UnityEngine;

public class GiveThrowbackOnImpact : MonoBehaviour
{
    [Header("Throwback Settings")]
    [SerializeField] private float _hitThrowbackForce = 5.0f;
    [SerializeField] private float _hitThrowbackIncrement = 0.25f;
    private float _throwbackFactor = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerThrowback>(out PlayerThrowback throwbackScript))
        {
            throwbackScript.Throwback(transform.forward, _hitThrowbackForce * _throwbackFactor, _hitThrowbackIncrement * _throwbackFactor);
        }

        if (!collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerThrowback>(out PlayerThrowback throwbackScript))
        {
            throwbackScript.Throwback(transform.forward, _hitThrowbackForce * _throwbackFactor, _hitThrowbackIncrement * _throwbackFactor);
        }

        if (!other.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    public void SetThrowbackFactor(float factor)
    {
        _throwbackFactor = factor;
    }
}
