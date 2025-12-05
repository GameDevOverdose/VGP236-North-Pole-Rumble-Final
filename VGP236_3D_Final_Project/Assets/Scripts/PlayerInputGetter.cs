using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-20)]
public class PlayerInputGetter : MonoBehaviour
{
    private static PlayerInputGetter instance = null;
    public static PlayerInputGetter Instance { get { return instance; } }

    private PlayerInput playerInput = null;

    private InputAction moveAction = null;
    private InputAction slideAction = null;
    private InputAction throwAction = null;
    private InputAction meleeAttack = null;
    private InputAction GrowSnowball = null;

    //GETTERS
    public InputAction MoveAction { get { return moveAction; } }
    public InputAction SlideAction { get { return slideAction; } }
    public InputAction ThrowAction { get { return throwAction; } }
    public InputAction MeleeAttack { get { return meleeAttack; } }
    public InputAction GrowSnowballAction { get { return GrowSnowball; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        playerInput = new PlayerInput();

        moveAction = playerInput.Player.Move;
        slideAction = playerInput.Player.Slide;
        throwAction = playerInput.Player.Throw;
        meleeAttack = playerInput.Player.Attack;
        GrowSnowball = playerInput.Player.GrowSnowball;
    }

    private void OnEnable()
    {
        playerInput.Enable();
        moveAction.Enable();
        slideAction.Enable();
        throwAction.Enable();
        meleeAttack.Enable();
        GrowSnowball.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        moveAction.Disable();
        slideAction.Disable();
        throwAction.Disable();
        meleeAttack.Disable();
        GrowSnowball.Disable();
    }
}
