using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Mobile Joystick")]
    [SerializeField] private FixedJoystick fixedJoystick;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string rotateObject = "RotateObject";

    private InputAction movementAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction rotateObjectAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool RotateObjectTriggered { get; private set; }

    private void Awake()
    {
        var map = playerControls.FindActionMap(actionMapName);

        movementAction = map.FindAction(movement);
        lookAction = map.FindAction(look);
        jumpAction = map.FindAction(jump);
        sprintAction = map.FindAction(sprint);
        rotateObjectAction = map.FindAction(rotateObject);

        RegisterInputEvents();
    }

    private void Update()
    {
        // Jika joystick digerakkan, gunakan joystick.
        // Jika tidak, keyboard tetap bekerja seperti biasa.
        if (fixedJoystick != null)
        {
            Vector2 joystickInput = new Vector2(
                fixedJoystick.Horizontal,
                fixedJoystick.Vertical);

            if (joystickInput.sqrMagnitude > 0.01f)
            {
                MovementInput = joystickInput;
            }
        }
    }

    private void RegisterInputEvents()
    {
        movementAction.performed += ctx =>
        {
            if (fixedJoystick == null ||
                new Vector2(fixedJoystick.Horizontal, fixedJoystick.Vertical).sqrMagnitude <= 0.01f)
            {
                MovementInput = ctx.ReadValue<Vector2>();
            }
        };

        movementAction.canceled += ctx =>
        {
            if (fixedJoystick == null ||
                new Vector2(fixedJoystick.Horizontal, fixedJoystick.Vertical).sqrMagnitude <= 0.01f)
            {
                MovementInput = Vector2.zero;
            }
        };

        lookAction.performed += ctx => RotationInput = ctx.ReadValue<Vector2>();
        lookAction.canceled += ctx => RotationInput = Vector2.zero;

        jumpAction.performed += ctx => JumpTriggered = true;
        jumpAction.canceled += ctx => JumpTriggered = false;

        sprintAction.performed += ctx => SprintTriggered = true;
        sprintAction.canceled += ctx => SprintTriggered = false;

        rotateObjectAction.performed += ctx => RotateObjectTriggered = true;
        rotateObjectAction.canceled += ctx => RotateObjectTriggered = false;
    }

    private void OnEnable() => playerControls.FindActionMap(actionMapName).Enable();
    private void OnDisable() => playerControls.FindActionMap(actionMapName).Disable();
}