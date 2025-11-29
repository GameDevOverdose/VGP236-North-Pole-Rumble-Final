using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private Transform _modelTransform = null;
    private Test_InputActions _playerInput = null;
    private InputAction _moveAction = null;
    private InputAction _slideAction = null;

    [SerializeField] private float _acceleration = 0.0f;
    [SerializeField] private float _deceleration = 0.0f;
    [SerializeField] private float _moveSpeed = 0.0f;
    [SerializeField] private float _slideSpeed = 0.0f;
    private float _baseMoveSpeed = 0.0f;
    private float _direction = 0.0f;
    [SerializeField] private float _rotationSpeed = 0.0f;
    [SerializeField] private float _slideTimer = 0.0f;
    [SerializeField] private bool _isSliding = false;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (_rigidBody == null)
        {
            _rigidBody = this.gameObject.GetComponent<Rigidbody>();
        }

        Debug.Assert(_rigidBody != null, $"{this} is missing a RigidBody");

        _playerInput = new Test_InputActions();
        _moveAction = _playerInput.Player.Move;
        _slideAction = _playerInput.Player.Slide;

        _baseMoveSpeed = _moveSpeed;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _moveAction.Enable();
        _slideAction.Enable();

        _slideAction.performed += SlideAction;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _moveAction.Disable();
        _slideAction.Disable();

        _slideAction.performed -= SlideAction;
    }

    private void Update()
    {
        if (_isSliding)
        {
            _slideTimer -= Time.deltaTime;

            if (_slideTimer < 0.0f)
            {
                _slideTimer = 0.0f;

                EndSlide();
            }
        }

        Move();
    }

    private void Move()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();

        Vector3 forward = _rigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        _direction = _isSliding ? 1 : moveInput.y;

        float targetSpeed = _direction * _moveSpeed;
        Vector3 targetHorinzontalVelocity = forward * targetSpeed;

        Vector3 currentVelocity = _rigidBody.linearVelocity;
        Vector3 currentHorizontalVelocity = new Vector3(currentVelocity.x, 0.0f, currentVelocity.z);

        float accelerationRate = 0.0f;
        if (Mathf.Abs(moveInput.y) > 0.1f || _isSliding)
        {
            accelerationRate = _acceleration;
        }
        else
        {
            accelerationRate = _deceleration;
            targetHorinzontalVelocity = Vector3.zero;
        }

        currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, targetHorinzontalVelocity, accelerationRate * Time.deltaTime);

        // Can't overwrite y, to allow gravity to still work
        currentVelocity.x = currentHorizontalVelocity.x;
        currentVelocity.z = currentHorizontalVelocity.z;
        _rigidBody.linearVelocity = currentVelocity;

        float rotation = moveInput.x * _rotationSpeed;
        _rigidBody.angularVelocity = new Vector3(0.0f, rotation, 0.0f);
    }

    private void Slide()
    {
        Vector3 forward = _rigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        float fowardAmount = 1 * _moveSpeed;
        Vector3 moveVelo = forward * fowardAmount;
        moveVelo.y = _rigidBody.linearVelocity.y;
        _rigidBody.linearVelocity = moveVelo;
    }

    private void SlideAction(InputAction.CallbackContext context)
    {
        StartCoroutine(SlideCoroutine());
    }

    private void EndSlide()
    {
        _modelTransform.localRotation = Quaternion.Euler(0, 0, 0);
        _moveSpeed = _baseMoveSpeed;
        _isSliding = false;
    }

    private IEnumerator SlideCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        _modelTransform.localRotation = Quaternion.Euler(90, _modelTransform.localRotation.y, _modelTransform.localRotation.y);

        _moveSpeed = _slideSpeed;
        _slideTimer = 0.7f;
        _isSliding = true;
    }

    private void Move2()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();

        Vector3 forward = _rigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        _direction = _isSliding ? 1 : moveInput.y;

        float forwardAmount = _direction * _moveSpeed;
        Vector3 moveVelo = forward * forwardAmount;
        moveVelo.y = _rigidBody.linearVelocity.y;
        _rigidBody.linearVelocity = moveVelo;

        float rotation = moveInput.x * _rotationSpeed;
        _rigidBody.angularVelocity = new Vector3(0.0f, rotation, 0.0f);
    }

    private void Move3()
    {
        // moveInput.x = left/right
        // moveInput.y = up/down
        // moveInput = joystick/dpad
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();

        Vector3 forward = _rigidBody.transform.forward;
        Vector3 right = _rigidBody.transform.right;

        forward.y = 0.0f;
        right.y = 0.0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveVelo = (forward * moveInput.y * _moveSpeed) + (right * moveInput.x * _moveSpeed);
        moveVelo.y = _rigidBody.linearVelocity.y;

        _rigidBody.linearVelocity = moveVelo;
        _rigidBody.angularVelocity = Vector3.zero;
    }
}
