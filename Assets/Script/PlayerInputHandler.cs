using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Mobile Joystick")]
    [SerializeField] private FixedJoystick fixedJoystick;

    [Header("Mobile Look")]
    [SerializeField] private bool enableMobileLook = true;
    [SerializeField] private bool useRightSideForLook = true;
    [SerializeField] private bool ignoreTouchOverUI = true;
    [SerializeField] private float mobileLookMultiplier = 1f;

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

    private bool jumpPressedThisFrame = false;

    private int lookFingerId = -1;
    private Vector2 lastLookPosition;

    private void Awake()
    {
        if (playerControls == null)
        {
            Debug.LogError("Player Controls belum diassign pada PlayerInputHandler!");
            return;
        }

        InputActionMap map = playerControls.FindActionMap(actionMapName);

        if (map == null)
        {
            Debug.LogError("Action Map tidak ditemukan: " + actionMapName);
            return;
        }

        movementAction = map.FindAction(movement);
        lookAction = map.FindAction(look);
        jumpAction = map.FindAction(jump);
        sprintAction = map.FindAction(sprint);
        rotateObjectAction = map.FindAction(rotateObject);

        RegisterInputEvents();
    }

    private void Update()
    {
        HandleMobileJoystick();
        HandleMobileLook();
    }

    private void HandleMobileJoystick()
    {
        if (fixedJoystick == null)
            return;

        Vector2 joystickInput = new Vector2(
            fixedJoystick.Horizontal,
            fixedJoystick.Vertical
        );

        if (joystickInput.sqrMagnitude > 0.01f)
        {
            MovementInput = joystickInput;
        }
        else
        {
            MovementInput = Vector2.zero;
        }
    }

    private void HandleMobileLook()
    {
        if (!enableMobileLook)
            return;

        if (Touchscreen.current == null)
            return;

        bool lookFingerStillActive = false;

        foreach (var touch in Touchscreen.current.touches)
        {
            if (!touch.press.isPressed)
                continue;

            int fingerId = touch.touchId.ReadValue();
            Vector2 currentPosition = touch.position.ReadValue();

            if (lookFingerId == -1)
            {
                if (useRightSideForLook && currentPosition.x < Screen.width * 0.5f)
                    continue;

                if (ignoreTouchOverUI &&
                    EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject(fingerId))
                    continue;

                lookFingerId = fingerId;
                lastLookPosition = currentPosition;
                RotationInput = Vector2.zero;

                return;
            }

            if (fingerId == lookFingerId)
            {
                lookFingerStillActive = true;

                Vector2 delta = currentPosition - lastLookPosition;
                lastLookPosition = currentPosition;

                RotationInput = delta * mobileLookMultiplier;

                return;
            }
        }

        if (!lookFingerStillActive)
        {
            lookFingerId = -1;

            if (Application.isMobilePlatform)
            {
                RotationInput = Vector2.zero;
            }
        }
    }

    private void RegisterInputEvents()
    {
        if (movementAction != null)
        {
            movementAction.performed += ctx =>
            {
                if (fixedJoystick == null)
                {
                    MovementInput = ctx.ReadValue<Vector2>();
                }
            };

            movementAction.canceled += ctx =>
            {
                if (fixedJoystick == null)
                {
                    MovementInput = Vector2.zero;
                }
            };
        }

        if (lookAction != null)
        {
            lookAction.performed += ctx =>
            {
                if (!Application.isMobilePlatform)
                {
                    RotationInput = ctx.ReadValue<Vector2>();
                }
            };

            lookAction.canceled += ctx =>
            {
                if (!Application.isMobilePlatform)
                {
                    RotationInput = Vector2.zero;
                }
            };
        }

        if (jumpAction != null)
        {
            jumpAction.performed += ctx =>
            {
                JumpTriggered = true;
                jumpPressedThisFrame = true;
            };

            jumpAction.canceled += ctx =>
            {
                JumpTriggered = false;
            };
        }

        if (sprintAction != null)
        {
            sprintAction.performed += ctx => SprintTriggered = true;
            sprintAction.canceled += ctx => SprintTriggered = false;
        }

        if (rotateObjectAction != null)
        {
            rotateObjectAction.performed += ctx => RotateObjectTriggered = true;
            rotateObjectAction.canceled += ctx => RotateObjectTriggered = false;
        }
    }

    public void MobileJumpPressed()
    {
        JumpTriggered = true;
        jumpPressedThisFrame = true;
    }

    public void MobileSprintDown()
    {
        SprintTriggered = true;
    }

    public void MobileSprintUp()
    {
        SprintTriggered = false;
    }

    public bool ConsumeJumpPressed()
    {
        if (jumpPressedThisFrame)
        {
            jumpPressedThisFrame = false;
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        if (playerControls != null)
        {
            InputActionMap map = playerControls.FindActionMap(actionMapName);

            if (map != null)
                map.Enable();
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            InputActionMap map = playerControls.FindActionMap(actionMapName);

            if (map != null)
                map.Disable();
        }
    }
}