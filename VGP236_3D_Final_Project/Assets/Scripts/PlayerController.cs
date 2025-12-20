using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _modelTransform = null;
    [SerializeField] private GroundCheck _groundCheck = null;
    [SerializeField] private PlayerKnockback _playerKnockback = null;
    [SerializeField] private RollingSnowball _rollingSnowball = null;
    private Coroutine _slideCoroutine = null;
    private Coroutine _meleeCoroutine = null;
    private Coroutine _throwCoroutine = null;

    [SerializeField] private float _acceleration = 0.0f;
    [SerializeField] private float _moveSpeed = 0.0f;
    [SerializeField] private float _slideSpeed = 0.0f;
    private float _baseMoveSpeed = 0.0f;
    private float _currentDirection = 0.0f;
    private float _lastKnownDirection = 0.0f;
    [SerializeField] private float _rotationSpeed = 0.0f;
    private float _baseRotationSpeed = 0.0f;
    [SerializeField] private float _maxSlideTime = 0.0f;
    private float _slideTimer = 0.0f;
    [SerializeField] private bool _isSliding = false;
    [SerializeField] private bool _isRolling = false;
    [SerializeField] private bool _disableMoveBackwards = false;

    private int playerNumber = 0;

    //GETTERS
    public float MoveSpeed { get { return _moveSpeed; } }
    public bool IsSliding { get { return _isSliding; } }
    public bool IsRolling { get { return _isRolling; } }

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

        //Debug.Assert(_rigidBody != null, $"{this} is missing a RigidBody");

        _baseMoveSpeed = _moveSpeed;
        _baseRotationSpeed = _rotationSpeed;

        playerNumber = PlayerManager.Instance.GetPlayerNumberIndex(gameObject);
    }

    private void OnEnable()
    {
        PlayerInputGetter.Instance.SlideAction(playerNumber).performed += SlideAction;
        PlayerInputGetter.Instance.RollSnowballAction(playerNumber).performed += RollingAction;
        PlayerInputGetter.Instance.MeleeAttack(playerNumber).performed += MeleeAction;
        PlayerInputGetter.Instance.ThrowAction(playerNumber).performed += ThrowAction;

        Initialize();
    }

    private void OnDisable()
    {
        PlayerInputGetter.Instance.SlideAction(playerNumber).performed -= SlideAction;
        PlayerInputGetter.Instance.RollSnowballAction(playerNumber).performed -= RollingAction;
        PlayerInputGetter.Instance.MeleeAttack(playerNumber).performed -= MeleeAction;
        PlayerInputGetter.Instance.ThrowAction(playerNumber).performed -= ThrowAction;
    }

    private void Update()
    {
        if (_slideTimer > 0.0f)
        {
            _slideTimer -= Time.deltaTime;

            if (_slideTimer < 0.0f)
            {
                //_slideTimer = 0.0f;

                EndSlide();
            }
        }

        Move();
    }

    private void Move()
    {
        Vector2 moveInput = PlayerInputGetter.Instance.MoveAction(playerNumber).ReadValue<Vector2>();

        Vector3 forward = _rigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        if (IsMoving(moveInput))
        {
            _currentDirection = _isSliding ? 1 : moveInput.y;
            _lastKnownDirection = _currentDirection;
        }

        float targetSpeed = _lastKnownDirection * _moveSpeed;
        Vector3 targetHorizontalVelocity = forward * targetSpeed;

        Vector3 currentVelocity = _rigidBody.linearVelocity;
        Vector3 currentHorizontalVelocity = new Vector3(currentVelocity.x, 0.0f, currentVelocity.z);

        float accelerationRate = 0.0f;

        if (IsMoving(moveInput) || _isSliding || _isRolling)
        {
            accelerationRate = _acceleration;

            if (_isSliding || _isRolling)
            {
                targetHorizontalVelocity = forward * Mathf.Abs(targetSpeed);
            }
            // If the player is moving backwards
            else if (moveInput.y < 0.0f)
            {
                accelerationRate *= 0.5f;

                if (_disableMoveBackwards)
                {
                    targetHorizontalVelocity = Vector3.zero;
                }
                else
                {
                    targetHorizontalVelocity = forward * (targetSpeed * 0.65f);
                }
            }
        }

        if (_isSliding || _isRolling)
        {
            _rotationSpeed = Mathf.Max(_rotationSpeed - (1.5f * Time.deltaTime), 1.0f);
        }

        currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, targetHorizontalVelocity, accelerationRate * Time.deltaTime);

        if (_groundCheck.IsGrounded)
        {
            // Can't overwrite y, to allow gravity to still work
            currentVelocity.x = currentHorizontalVelocity.x;
            currentVelocity.z = currentHorizontalVelocity.z;
            _rigidBody.linearVelocity = currentVelocity;
            _animator.SetFloat("Horizontal Speed", Math.Abs(currentVelocity.x));
        }

        float rotation = moveInput.x * _rotationSpeed;
        _rigidBody.angularVelocity = new Vector3(0.0f, rotation, 0.0f);

        //_animator.SetFloat("Vertical Speed", _rigidBody.linearVelocity.y);
    }

    private bool IsMoving(Vector2 moveInput)
    {
        return Mathf.Abs(moveInput.y) > 0.1f;
    }

    private void RollingAction(InputAction.CallbackContext context)
    {
        if (_isRolling)
        {
            EndRolling();
        }
        else if (_groundCheck.IsGrounded && !_isSliding)
        {
            _isRolling = true;
        }
    }

    public void EndRolling()
    {
        if (!_isRolling)
        {
            return;
        }

        _rotationSpeed = _baseRotationSpeed;
        _isRolling = false;
    }

    private void SlideAction(InputAction.CallbackContext context)
    {
        if (_slideCoroutine != null || !_groundCheck.IsGrounded || _isRolling)
        {
            return;
        }

        _slideCoroutine = StartCoroutine(SlideCoroutine());
    }

    private void MeleeAction(InputAction.CallbackContext context)
    {
        if (_meleeCoroutine != null || _isSliding || IsRolling)
        {
            return;
        }

        _meleeCoroutine = StartCoroutine(MeleeCoroutine());
    }

    private void ThrowAction(InputAction.CallbackContext context)
    {
        if (_throwCoroutine != null || _isSliding || IsRolling)
        {
            return;
        }

        _throwCoroutine = StartCoroutine(ThrowCoroutine());
    }

    private IEnumerator ThrowCoroutine()
    {
        Debug.Log("Started Melee Coroutine");
        _animator.SetBool("IsThrow", true);
        yield return new WaitForSeconds(0.3f);

        _animator.SetBool("IsThrow", false);
        _throwCoroutine = null;
    }

    private IEnumerator MeleeCoroutine()
    {
        Debug.Log("Started Melee Coroutine");
        _animator.SetBool("IsMelee", true);
        yield return new WaitForSeconds(0.3f);

        _animator.SetBool("IsMelee", false);
        _meleeCoroutine = null;
    }

    private IEnumerator SlideCoroutine()
    {
        Debug.Log("Started Slide Coroutine");
        yield return new WaitForSeconds(0.3f);
        _modelTransform.localRotation = Quaternion.Euler(90, _modelTransform.localRotation.y, _modelTransform.localRotation.y);

        _moveSpeed = _slideSpeed;
        _slideTimer = _maxSlideTime;
        _isSliding = true;
        _animator.SetBool("IsSliding", true);
    }
    private void EndSlide()
    {
        if (!_isSliding)
        {
            return;
        }

        Debug.Log("Ended Slide");
        _modelTransform.localRotation = Quaternion.Euler(0, 0, 0);
        _moveSpeed = _baseMoveSpeed;
        _rotationSpeed = _baseRotationSpeed;
        _slideTimer = 0.0f;
        _isSliding = false;
        _animator.SetBool("IsSliding", false);
        StopCoroutine(_slideCoroutine);
        _slideCoroutine = null;
    }

    public void ResetPlayerState()
    {
        Debug.Log($"reset player {gameObject.name}");
        _rigidBody.linearVelocity = Vector3.zero;
        _modelTransform.localRotation = Quaternion.Euler(0, 0, 0);

        EndSlide();
        EndRolling();
        _rollingSnowball.DestroySnowball();
    }
}
