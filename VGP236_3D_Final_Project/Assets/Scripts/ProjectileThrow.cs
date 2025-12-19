using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileThrow : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab = null;
    [SerializeField] private Transform _spawnTransform = null;
    [SerializeField] private float _throwForce = 10.0f;
    [SerializeField] private float _throwRate = 1.0f;
    private float _throwTimer = 0.0f;

    private PlayerController _playerController = null;

    private bool _canThrow = true;

    private int playerNumber = 0;

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
        _playerController = GetComponent<PlayerController>();

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
        if(!_playerController.IsSliding && _canThrow && _throwTimer >= _throwRate)
        {
            _throwTimer = 0.0f;

            GameObject instantiatedProjectile = Instantiate(_projectilePrefab, _spawnTransform.position, _spawnTransform.rotation);

            if (instantiatedProjectile.TryGetComponent<Rigidbody>(out Rigidbody projectileRigidbody))
            {
                projectileRigidbody.AddForce(_spawnTransform.forward * _throwForce, ForceMode.VelocityChange);
            }
        }
    }

    public void SetCanThrow(bool value)
    {
        _canThrow = value;
    }
}
