using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileThrow : MonoBehaviour
{
    private PlayerController _controller = null;

    [SerializeField] private GameObject _projectilePrefab = null;
    [SerializeField] private Transform _spawnTransform = null;
    [SerializeField] private float _throwForce = 10.0f;
    [SerializeField] private float _throwRate = 1.0f;
    private float _throwTimer = 0.0f;

    private bool _canThrow = true;

    private int playerNumber = 0;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _throwSounds;
    [SerializeField] private float _throwSoundVolume = 1.0f;

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
        _controller = GetComponent<PlayerController>();

        playerNumber = PlayerManager.Instance.GetPlayerNumberIndex(transform.gameObject);
        PlayerInputGetter.Instance.ThrowAction(playerNumber).performed += Throw;

        _throwTimer = _throwRate;
    }

    private void Update()
    {
        if(_throwTimer < _throwRate)
        {
            _throwTimer += Time.deltaTime;
        }
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if(!_controller.IsSliding && !_controller.IsRolling && _canThrow && _throwTimer >= _throwRate)
        {
            _throwTimer = 0.0f;

            GameObject instantiatedProjectile = Instantiate(_projectilePrefab, _spawnTransform.position, _spawnTransform.rotation);

            if (instantiatedProjectile.TryGetComponent<Rigidbody>(out Rigidbody projectileRigidbody))
            {
                projectileRigidbody.AddForce(_spawnTransform.forward * _throwForce, ForceMode.VelocityChange);
            }

            SoundEffectManager.Instance.PlaySoundInSpace(SoundEffectManager.Instance.GetRandomSound(_throwSounds), transform.position, _throwSoundVolume);
        }
    }

    public void SetCanThrow(bool value)
    {
        _canThrow = value;
    }
}
