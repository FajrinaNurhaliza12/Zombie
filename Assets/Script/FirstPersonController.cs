using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandlers;

    [Header("Movement SFX")]
    [SerializeField] private AudioSource jumpAudioSource;
    [SerializeField] private AudioClip jumpSound;

    [SerializeField] private AudioSource sprintAudioSource;
    [SerializeField] private AudioClip sprintSound;

    [SerializeField] private AudioSource walkAudioSource;
    [SerializeField] private AudioClip walkSound;

    private Vector3 currentMovement;
    private float verticalRotation;

    private float CurrentSpeed =>
        walkSpeed * (playerInputHandlers.SprintTriggered ? sprintMultiplier : 1);

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (jumpAudioSource != null)
        {
            jumpAudioSource.playOnAwake = false;
            jumpAudioSource.loop = false;
        }

        if (sprintAudioSource != null)
        {
            sprintAudioSource.playOnAwake = false;
            sprintAudioSource.loop = true;
        }

        if (walkAudioSource != null)
        {
            walkAudioSource.playOnAwake = false;
            walkAudioSource.loop = true;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            StopAllMovementSounds();
            currentMovement = Vector3.zero;
            return;
        }

        if (GameManager.Instance != null &&
            (GameManager.Instance.GameEnded || GameManager.Instance.IsPaused))
        {
            StopAllMovementSounds();
            currentMovement = Vector3.zero;
            return;
        }

        HandleMovement();
        HandleRotation();
        HandleMovementSounds();
    }

    private void HandleMovement()
    {
        Vector3 input = new Vector3(
            playerInputHandlers.MovementInput.x,
            0f,
            playerInputHandlers.MovementInput.y
        );

        Vector3 worldDir = transform.TransformDirection(input).normalized;

        currentMovement.x = worldDir.x * CurrentSpeed;
        currentMovement.z = worldDir.z * CurrentSpeed;

        HandleJumping();

        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (playerInputHandlers.ConsumeJumpPressed())
            {
                currentMovement.y = jumpForce;

                StopWalkSound();
                StopSprintSound();

                PlayJumpSound();

                Debug.Log("JUMP SOUND DIPANGGIL");
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    private void HandleRotation()
    {
        float mouseX = playerInputHandlers.RotationInput.x * mouseSensitivity;
        float mouseY = playerInputHandlers.RotationInput.y * mouseSensitivity;

        transform.Rotate(0, mouseX, 0);

        verticalRotation = Mathf.Clamp(
            verticalRotation - mouseY,
            -upDownLookRange,
            upDownLookRange
        );

        mainCamera.transform.localRotation =
            Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void PlayJumpSound()
    {
        if (jumpAudioSource == null)
        {
            Debug.LogWarning("Jump Audio Source belum diassign!");
            return;
        }

        if (jumpSound == null)
        {
            Debug.LogWarning("Jump Sound belum diassign!");
            return;
        }

        jumpAudioSource.PlayOneShot(jumpSound);
    }

    private void HandleMovementSounds()
    {
        bool isMoving = playerInputHandlers.MovementInput.sqrMagnitude > 0.01f;
        bool isGrounded = characterController.isGrounded;
        bool isSprinting = playerInputHandlers.SprintTriggered;

        if (!isMoving || !isGrounded)
        {
            StopWalkSound();
            StopSprintSound();
            return;
        }

        if (isSprinting)
        {
            StopWalkSound();
            PlaySprintSound();
        }
        else
        {
            StopSprintSound();
            PlayWalkSound();
        }
    }

    private void PlayWalkSound()
    {
        if (walkAudioSource == null)
        {
            Debug.LogWarning("Walk Audio Source belum diassign!");
            return;
        }

        if (walkSound == null)
        {
            Debug.LogWarning("Walk Sound belum diassign!");
            return;
        }

        if (!walkAudioSource.isPlaying)
        {
            walkAudioSource.clip = walkSound;
            walkAudioSource.loop = true;
            walkAudioSource.Play();

            Debug.Log("WALK SOUND DIPANGGIL");
        }
    }

    private void PlaySprintSound()
    {
        if (sprintAudioSource == null)
        {
            Debug.LogWarning("Sprint Audio Source belum diassign!");
            return;
        }

        if (sprintSound == null)
        {
            Debug.LogWarning("Sprint Sound belum diassign!");
            return;
        }

        if (!sprintAudioSource.isPlaying)
        {
            sprintAudioSource.clip = sprintSound;
            sprintAudioSource.loop = true;
            sprintAudioSource.Play();

            Debug.Log("SPRINT SOUND DIPANGGIL");
        }
    }

    private void StopWalkSound()
    {
        if (walkAudioSource != null && walkAudioSource.isPlaying)
        {
            walkAudioSource.Stop();
        }
    }

    private void StopSprintSound()
    {
        if (sprintAudioSource != null && sprintAudioSource.isPlaying)
        {
            sprintAudioSource.Stop();
        }
    }

    private void StopAllMovementSounds()
    {
        StopWalkSound();
        StopSprintSound();
    }
}