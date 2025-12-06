using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerNumberAssigner;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private Transform _modelTransform = null;

    [SerializeField] private float _acceleration = 0.0f;
    [SerializeField] private float _moveSpeed = 0.0f;
    [SerializeField] private float _slideSpeed = 0.0f;
    private float _baseMoveSpeed = 0.0f;
    private float _currentDirection = 0.0f;
    private float _lastKnownDirection = 0.0f;
    [SerializeField] private float _rotationSpeed = 0.0f;
    private float _baseRotationSpeed = 0.0f;
    private float _slideTimer = 0.0f;
    [SerializeField] private bool _isSliding = false;
    [SerializeField] private bool _disableMoveBackwards = false;

    [SerializeField] private TMP_Text _velocityDebugText = null;
    [SerializeField] private TMP_Text _debugText2 = null;
    [SerializeField] private TMP_Text _debugText3 = null;
    [SerializeField] private TMP_Text _debugText4 = null;

    private int playerNumber = 0;

    //GETTERS
    public float MoveSpeed { get { return _moveSpeed; } }
    public bool IsSliding { get { return _isSliding; } }

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

        _baseMoveSpeed = _moveSpeed;
        _baseRotationSpeed = _rotationSpeed;

        playerNumber = PlayerNumberAssigner.Instance.GetPlayerNumberIndex(gameObject);
    }

    private void OnEnable()
    {
        PlayerInputGetter.Instance.SlideAction(playerNumber).performed += SlideAction;
    }

    private void OnDisable()
    {
        PlayerInputGetter.Instance.SlideAction(playerNumber).performed -= SlideAction;
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

        if (IsMoving(moveInput) || _isSliding)
        {
            accelerationRate = _acceleration;

            // If the player is moving backwards
            if (moveInput.y < 0.0f)
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

        currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, targetHorizontalVelocity, accelerationRate * Time.deltaTime);
        
        // Can't overwrite y, to allow gravity to still work
        currentVelocity.x = currentHorizontalVelocity.x;
        currentVelocity.z = currentHorizontalVelocity.z;
        _rigidBody.linearVelocity = currentVelocity;

        _debugText2.text = "Moving Forward: " + (IsMoving(moveInput) || _isSliding);
        _debugText3.text = "CurrentHorizontalVelocity: " + currentHorizontalVelocity;
        _debugText4.text = "TargetHorizontalVelocity: " + targetHorizontalVelocity;
        _velocityDebugText.text = ($"Velocity.x: {_rigidBody.linearVelocity.x.ToString("F2")}\nTarget Speed: {targetSpeed}");

        float rotation = moveInput.x * _rotationSpeed;
        _rigidBody.angularVelocity = new Vector3(0.0f, rotation, 0.0f);
    }

    private bool IsMoving(Vector2 moveInput)
    {
        return Mathf.Abs(moveInput.y) > 0.1f;
    }

    private void Move4()
    {
        Vector2 moveInput = PlayerInputGetter.Instance.MoveAction(playerNumber).ReadValue<Vector2>();

        Vector3 forward = _rigidBody.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        _currentDirection = _isSliding ? 1 : moveInput.y;

        float targetSpeed = _currentDirection * _moveSpeed;
        Vector3 targetHorizontalVelocity = forward * targetSpeed;

        Vector3 currentVelocity = _rigidBody.linearVelocity;
        Vector3 currentHorizontalVelocity = new Vector3(currentVelocity.x, 0.0f, currentVelocity.z);

        float accelerationRate = 0.0f;

        if (Mathf.Abs(moveInput.y) > 0.1f || _isSliding)
        {
            accelerationRate = _acceleration;
        }
        else
        {
            //accelerationRate = _deceleration;
            targetHorizontalVelocity = Vector3.zero;
        }

        // If the player is moving backwards
        if (moveInput.y < 0.0f)
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

        currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, targetHorizontalVelocity, accelerationRate * Time.deltaTime);

        // Can't overwrite y, to allow gravity to still work
        currentVelocity.x = currentHorizontalVelocity.x;
        currentVelocity.z = currentHorizontalVelocity.z;
        _rigidBody.linearVelocity = currentVelocity;

        _velocityDebugText.text = ($"Velocity.x: {_rigidBody.linearVelocity.x.ToString("F2")}\nTarget Speed: {targetSpeed}");
        //_velocityDebugText.text = ($"TargetHorizontalVelocity.x: {_rigidBody.linearVelocity.x.ToString("F2")}\nTarget Speed: {targetSpeed}");

        float rotation = moveInput.x * _rotationSpeed;
        _rigidBody.angularVelocity = new Vector3(0.0f, rotation, 0.0f);
    }

    private void Slide()
    {
        // Placeholder for possible option for slide if its wanted to be instantaneous speed boost
        //if (_isSliding)
        //{
        //    currentVelocity.x = targetHorinzontalVelocity.x;
        //    currentVelocity.z = targetHorinzontalVelocity.z;
        //    _rigidBody.linearVelocity = currentVelocity;
        //}
        //else
        //{
        //    if (Mathf.Abs(moveInput.y) > 0.1f || _isSliding)
        //    {
        //        accelerationRate = _acceleration;
        //    }
        //    else
        //    {
        //        accelerationRate = _deceleration;
        //        targetHorinzontalVelocity = Vector3.zero;
        //    }

        //    currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, targetHorinzontalVelocity, accelerationRate * Time.deltaTime);

        //    // Can't overwrite y, to allow gravity to still work
        //    currentVelocity.x = currentHorizontalVelocity.x;
        //    currentVelocity.z = currentHorizontalVelocity.z;
        //    _rigidBody.linearVelocity = currentVelocity;
        //}
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
}
