using UnityEngine;

public class GiveKnockbackOnImpact : MonoBehaviour
{
    private PlayerController _controller = null;
    private RollingSnowball _rollingSnowball = null;

    [Header("Throwback Settings")]
    [SerializeField] private float _knockbackForce = 5.0f;
    [SerializeField] private float _knockbackIncrement = 0.25f;
    private float _knockbackFactor = 1.0f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] _impactSounds;
    [SerializeField] private float _soundsVolume = 1.0f;

    public void Initialize(PlayerController controller, RollingSnowball rollingSnowball)
    {
        _controller = controller;
        _rollingSnowball = rollingSnowball;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerKnockback>(out PlayerKnockback throwbackScript))
        {
            throwbackScript.Knockback(transform.forward, _knockbackForce * _knockbackFactor, _knockbackIncrement * _knockbackFactor);
        }

        if (!collision.gameObject.CompareTag("Projectile"))
        {
            SoundEffectManager.Instance.PlaySoundInSpace(SoundEffectManager.Instance.GetRandomSound(_impactSounds), transform.position);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerKnockback>(out PlayerKnockback throwbackScript))
        {
            Debug.Log(other.gameObject.name);
            throwbackScript.Knockback(transform.forward, _knockbackForce * _knockbackFactor, _knockbackIncrement * _knockbackFactor);

            Debug.Log("_controller is not null: " + _rollingSnowball != null);
            if (_rollingSnowball != null)
            {
                _rollingSnowball.DestroySnowball();
            }
            if (_controller != null)
            {
                _controller.EndRolling();
            }
        }

        if (!other.gameObject.CompareTag("Projectile"))
        {
            SoundEffectManager.Instance.PlaySoundInSpace(SoundEffectManager.Instance.GetRandomSound(_impactSounds), transform.position, _soundsVolume);
            if (_rollingSnowball != null)
            {
                _rollingSnowball.DestroySnowball();
            }
            if (_controller != null)
            {
                _controller.EndRolling();
            }

            Destroy(gameObject);
        }
    }

    public void SetThrowbackFactor(float factor)
    {
        _knockbackFactor = factor;
    }
}
