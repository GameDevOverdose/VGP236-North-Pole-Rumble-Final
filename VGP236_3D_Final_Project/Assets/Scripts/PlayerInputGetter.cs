using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-20)]
public class PlayerInputGetter : MonoBehaviour
{
    private static PlayerInputGetter _instance = null;
    public static PlayerInputGetter Instance { get { return _instance; } }

    private PlayerInput _playerInput = null;

    private InputAction _moveAction = null;
    private InputAction _slideAction = null;
    private InputAction _throwAction = null;
    private InputAction _meleeAttack = null;
    private InputAction _growSnowball = null;

    //GETTERS
    public InputAction MoveAction { get { return _moveAction; } }
    public InputAction SlideAction { get { return _slideAction; } }
    public InputAction ThrowAction { get { return _throwAction; } }
    public InputAction MeleeAttack { get { return _meleeAttack; } }
    public InputAction GrowSnowballAction { get { return _growSnowball; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        _playerInput = new PlayerInput();

        _moveAction = _playerInput.Player.Move;
        _slideAction = _playerInput.Player.Slide;
        _throwAction = _playerInput.Player.Throw;
        _meleeAttack = _playerInput.Player.Attack;
        _growSnowball = _playerInput.Player.GrowSnowball;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _moveAction.Enable();
        _slideAction.Enable();
        _throwAction.Enable();
        _meleeAttack.Enable();
        _growSnowball.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _moveAction.Disable();
        _slideAction.Disable();
        _throwAction.Disable();
        _meleeAttack.Disable();
        _growSnowball.Disable();
    }
}
