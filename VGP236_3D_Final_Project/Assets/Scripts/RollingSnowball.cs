using UnityEngine;
using UnityEngine.InputSystem;

public class RollingSnowball : MonoBehaviour
{
    [SerializeField] private GameObject _rollingSnowballPrefab = null;
    private GameObject _rollingSnowballInstance = null;

    private Vector3 _startScale = Vector3.zero;
    [SerializeField] Vector3 _endScale = Vector3.one;

    [SerializeField] private float _maxGrowSpeed = 1.0f;

    private Rigidbody _playerRigidbody = null;
    private float _playerMaxSpeed = 0.0f;
    private ProjectileThrow _projectileThrowScript = null;

    private float _currentKnockbackFactor = 0.0f;
    private GiveKnockbackOnImpact _throwbackScript = null;

    //Can use this in case implementing a cooldown/powerup
    private bool _canRollSnowball = true;

    private int playerNumber = 0;

    private void Awake()
    {
        _playerRigidbody = transform.parent.GetComponent<Rigidbody>();
        _playerMaxSpeed = transform.parent.GetComponent<PlayerController>().MoveSpeed;
        _projectileThrowScript = transform.parent.GetComponent<ProjectileThrow>();

        playerNumber = PlayerNumberAssigner.Instance.GetPlayerNumberIndex(transform.parent.gameObject);
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
        if (_rollingSnowballInstance == null && _canRollSnowball)
        {
            InstantiateSnowball();
        }
        else
        {
            //JUST DESTROY FOR TESTING PURPOSES
            Destroy(_rollingSnowballInstance);
            _rollingSnowballInstance = null; // Temp, convert from instantiate
        }
    }

    private void InstantiateSnowball()
    {
        _rollingSnowballInstance = Instantiate(_rollingSnowballPrefab, this.transform);
        _rollingSnowballInstance.transform.position = transform.position;
        _rollingSnowballInstance.transform.localScale = _startScale;
        _throwbackScript = _rollingSnowballInstance.GetComponent<GiveKnockbackOnImpact>();
    }
}
