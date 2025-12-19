using UnityEngine;

public class GiveKnockbackOnImpact : MonoBehaviour
{
    private PlayerController _controller = null;

    [Header("Throwback Settings")]
    [SerializeField] private float _knockbackForce = 5.0f;
    [SerializeField] private float _knockbackIncrement = 0.25f;
    private float _knockbackFactor = 1.0f;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _impactSound;

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerKnockback>(out PlayerKnockback throwbackScript))
        {
            throwbackScript.Knockback(transform.forward, _knockbackForce * _knockbackFactor, _knockbackIncrement * _knockbackFactor);
        }

        if (!collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerKnockback>(out PlayerKnockback throwbackScript))
        {
            Debug.Log(other.gameObject.name);
            Debug.Log("throwback");
            throwbackScript.Knockback(transform.forward, _knockbackForce * _knockbackFactor, _knockbackIncrement * _knockbackFactor);

            _controller.EndRolling();
        }

        if (!other.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    public void SetThrowbackFactor(float factor)
    {
        _knockbackFactor = factor;
    }
}
