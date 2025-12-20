using UnityEngine;
using UnityEngine.InputSystem;

public class RollingSnowball : MonoBehaviour
{
    private PlayerController _playerController = null;
    [SerializeField] private GroundCheck _groundCheck = null;

    [SerializeField] private GameObject _rollingSnowballPrefab = null;
    private GameObject _rollingSnowballInstance = null;

    private Vector3 _startScale = Vector3.zero;
    [SerializeField] Vector3 _endScale = Vector3.one;

    [SerializeField] private float _maxGrowSpeed = 1.0f;
    [SerializeField] private float _rotateGrowSpeed = 1.0f;

    private Rigidbody _playerRigidbody = null;
    private float _playerMaxSpeed = 0.0f;
    private ProjectileThrow _projectileThrowScript = null;

    private float _currentKnockbackFactor = 0.0f;
    private GiveKnockbackOnImpact _throwbackScript = null;

    //Can use this in case implementing a cooldown/powerup
    private bool _canRollSnowball = true;

    private int playerNumber = 0;

    [SerializeField] private AudioClip[] rollingSound;
    [SerializeField] private float rollingSoundVolume = 1.0f;
    private AudioSource _rollingSnowballAudioSource = null;

    private void Awake()
    {
        _playerController = transform.parent.GetComponent<PlayerController>();
        _playerRigidbody = transform.parent.GetComponent<Rigidbody>();
        _playerMaxSpeed = _playerController.MoveSpeed;
        _projectileThrowScript = transform.parent.GetComponent<ProjectileThrow>();

        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerNumber = PlayerManager.Instance.GetPlayerNumberIndex(transform.parent.gameObject);
        PlayerInputGetter.Instance.RollSnowballAction(playerNumber).performed += GrowSnowball;
    }

    private void Update()
    {
        if (_rollingSnowballInstance == null)
        {
            _projectileThrowScript.SetCanThrow(true);
            _currentKnockbackFactor = 0.0f;
            return;
        }

        //Handle Rolling Snowball Scale
        _projectileThrowScript.SetCanThrow(false);

        Vector3 growth = Vector3.one * _maxGrowSpeed * GrowthFactor() * Time.deltaTime;
        Vector3 newScale = Vector3.Min(_rollingSnowballInstance.transform.localScale + growth, _endScale);

        Vector3 deltaScale = newScale - _rollingSnowballInstance.transform.localScale;

        _rollingSnowballInstance.transform.position += _rollingSnowballInstance.transform.forward * (deltaScale.z / 2f);

        _rollingSnowballInstance.transform.localScale = newScale;

        _rollingSnowballInstance.transform.GetChild(0).Rotate(Vector3.right, _rotateGrowSpeed * GrowthFactor() * Time.deltaTime * 360f);

        //Handle Scaling Throw Back Force
        _throwbackScript.SetThrowbackFactor(_currentKnockbackFactor);
    }

    private float GrowthFactor()
    {
        float playerCurrentSpeed = _playerRigidbody.linearVelocity.magnitude;

        _currentKnockbackFactor = Mathf.Clamp(_rollingSnowballInstance.transform.localScale.z / _endScale.z, 0.0f, 1.0f);
        return Mathf.Clamp(playerCurrentSpeed / _playerMaxSpeed, 0.0f, 1.0f);
    }

    private void GrowSnowball(InputAction.CallbackContext context)
    {
        if (_rollingSnowballInstance == null && _canRollSnowball && _groundCheck.IsGrounded)
        {
            InstantiateSnowball();
        }
        else
        {
            DestroySnowball();
        }
    }

    private void InstantiateSnowball()
    {
        _rollingSnowballInstance = Instantiate(_rollingSnowballPrefab, this.transform);
        _rollingSnowballInstance.transform.position = transform.position;
        _rollingSnowballInstance.transform.localScale = _startScale;
        _throwbackScript = _rollingSnowballInstance.GetComponent<GiveKnockbackOnImpact>();
        _throwbackScript.Initialize(_playerController);

        _rollingSnowballAudioSource = SoundEffectManager.Instance.PlayLoopingSound(
            SoundEffectManager.Instance.GetRandomSound(rollingSound),
            _rollingSnowballInstance.transform.position,
            rollingSoundVolume);
    }

    public void DestroySnowball()
    {
        if (_rollingSnowballInstance != null)
        {
            Destroy(_rollingSnowballInstance);
            _rollingSnowballInstance = null;
        }

        SoundEffectManager.Instance.StopLoopingSound(_rollingSnowballAudioSource);
    }
}
