using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-20)]
public class PlayerInputGetter : MonoBehaviour
{
    private static PlayerInputGetter _instance = null;
    public static PlayerInputGetter Instance { get { return _instance; } }

    private PlayerInput _playerInput = null;

    private struct InputActions
    {
       public InputAction _moveAction;
       public InputAction _slideAction;
       public InputAction _throwAction;
       public InputAction _meleeAction;
       public InputAction _rollSnowballAction;
    }

    private List<InputActions> _playersInputActions = new List<InputActions>();

    //GETTERS
    //public InputAction MoveAction { get { return  _playersInputActions[playerNumber]._moveAction; } }
    //public InputAction SlideAction { get { return _playersInputActions[playerNumber]._slideAction; } }
    //public InputAction ThrowAction { get { return _playersInputActions[playerNumber]._throwAction; } }
    //public InputAction MeleeAttack { get { return _playersInputActions[playerNumber]._meleeAction; } }
    //public InputAction RollSnowballAction { get { return _playersInputActions[playerNumber]._rollSnowballAction; } }

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

        for (int i = 0; i < PlayerNumberAssigner.Instance.PlayersSupported; i++)
        {
            InputActions action = new InputActions();

            switch (i)
            {
                case 0:
                    action._moveAction = _playerInput.Player_1.Move;
                    action._slideAction = _playerInput.Player_1.Slide;
                    action._throwAction = _playerInput.Player_1.Throw;
                    action._meleeAction = _playerInput.Player_1.Melee;
                    action._rollSnowballAction = _playerInput.Player_1.Roll;
                    break;
                case 1:
                    action._moveAction = _playerInput.Player_2.Move;
                    action._slideAction = _playerInput.Player_2.Slide;
                    action._throwAction = _playerInput.Player_2.Throw;
                    action._meleeAction = _playerInput.Player_2.Melee;
                    action._rollSnowballAction = _playerInput.Player_2.Roll;
                    break;
                case 2:
                    action._moveAction = _playerInput.Player_2.Move;
                    action._slideAction = _playerInput.Player_2.Slide;
                    action._throwAction = _playerInput.Player_2.Throw;
                    action._meleeAction = _playerInput.Player_2.Melee;
                    action._rollSnowballAction = _playerInput.Player_2.Roll;
                    break;
                case 3:
                    action._moveAction = _playerInput.Player_2.Move;
                    action._slideAction = _playerInput.Player_2.Slide;
                    action._throwAction = _playerInput.Player_2.Throw;
                    action._meleeAction = _playerInput.Player_2.Melee;
                    action._rollSnowballAction = _playerInput.Player_2.Roll;
                    break;
            }

            _playersInputActions.Add(action);
        }
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        
        for(int i = 0; i < _playersInputActions.Count; i++)
        {
            _playersInputActions[i]._moveAction.Enable();
            _playersInputActions[i]._slideAction.Enable();
            _playersInputActions[i]._throwAction.Enable();
            _playersInputActions[i]._meleeAction.Enable();
            _playersInputActions[i]._rollSnowballAction.Enable();
        }
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        for (int i = 0; i < _playersInputActions.Count; i++)
        {
            _playersInputActions[i]._moveAction.Disable();
            _playersInputActions[i]._slideAction.Disable();
            _playersInputActions[i]._throwAction.Disable();
            _playersInputActions[i]._meleeAction.Disable();
            _playersInputActions[i]._rollSnowballAction.Disable();
        }
    }

    public InputAction MoveAction(int playerNumber)
    {
        return _playersInputActions[playerNumber]._moveAction;
    }

    public InputAction SlideAction(int playerNumber)
    {
        return _playersInputActions[playerNumber]._slideAction;
    }

    public InputAction ThrowAction(int playerNumber)
    {
        return _playersInputActions[playerNumber]._throwAction;
    }

    public InputAction MeleeAttack(int playerNumber)
    {
        return _playersInputActions[playerNumber]._meleeAction;
    }

    public InputAction RollSnowballAction(int playerNumber)
    {
        return _playersInputActions[playerNumber]._rollSnowballAction;
    }
}
