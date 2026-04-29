using System.Collections;
using UnityEngine;
using TMPro;

public class Level1TutorialManager : MonoBehaviour
{
    public static Level1TutorialManager instance;

    [Header("Player References")]
    public Transform player;
    public Transform followTarget;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    [Header("UI References")]
    public Camera mainCam;
    public Canvas canvas;
    public RectTransform speechBubble;
    public TextMeshProUGUI tutorialText;

    [Header("Follow Settings")]
    public Vector3 worldOffset = Vector3.zero;
    public Vector2 screenOffset = Vector2.zero;

    [Header("Timing")]
    public float introDuration = 10f;
    public float specialTextSwapDelay = 0.5f;
    public float temporaryMessageDuration = 2f;

    [Header("Ground Check")]
    public float groundCheckRadius = 0.2f;
    public float jumpPressGraceTime = 0.3f;

    private bool isGrounded;
    private bool wasGrounded;
    private bool jumpPressed;
    private float jumpPressedTimer;

    private float introTimer;
    private bool waitingForTransition;

    private Coroutine temporaryMessageRoutine;

    private enum TutorialStep
    {
        Intro,
        Move,
        Jump,
        DoubleJump,
        Spikes,
        HealthFruit,
        Checkpoint,
        Star,
        Done
    }

    private TutorialStep currentStep;

    private void Awake()
    {
        instance = this;

        if (speechBubble != null)
        {
            speechBubble.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        currentStep = TutorialStep.Intro;
        introTimer = introDuration;
        waitingForTransition = false;

        SetText("Hi, I'm Milton. My brothers, Let and Burg, were kidnapped! Help me collect the 3 keys so I can save them!");
    }

    private void Update()
    {
        UpdateGrounded();
        HandleIntroStep();
        HandleMovementStep();
        HandleJumpStep();
    }

    private void LateUpdate()
    {
        FollowSpeechBubble();
    }

    private void UpdateGrounded()
    {
        if (groundCheckPoint == null)
            return;

        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
    }

    private void HandleIntroStep()
    {
        if (currentStep != TutorialStep.Intro || waitingForTransition)
            return;

        introTimer -= Time.deltaTime;

        bool movementInput =
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow) ||
            Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f;

        if (movementInput || introTimer <= 0f)
        {
            currentStep = TutorialStep.Move;
            SetText("Use A and D or the left and right arrow keys to move back and forth.");
        }
    }

    private void HandleMovementStep()
    {
        if (currentStep != TutorialStep.Move || waitingForTransition)
            return;

        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentStep = TutorialStep.Jump;
            SetText("Press Space to jump.");
        }
    }

    private void HandleJumpStep()
    {
        if (currentStep != TutorialStep.Jump || waitingForTransition)
            return;

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            jumpPressedTimer = jumpPressGraceTime;
        }

        if (jumpPressed)
        {
            jumpPressedTimer -= Time.deltaTime;

            if (jumpPressedTimer <= 0f)
            {
                jumpPressed = false;
            }
        }

        if (jumpPressed && wasGrounded && !isGrounded)
        {
            jumpPressed = false;
            currentStep = TutorialStep.DoubleJump;
            SetText("Press Space twice in a row to double jump.");
        }
    }

    public void RegisterDoubleJump()
    {
        if (currentStep == TutorialStep.DoubleJump && !waitingForTransition)
        {
            StartCoroutine(DelayedStepChange(
                TutorialStep.Spikes,
                "I need to avoid those spikes or I'll lose some health."
            ));
        }
    }

    public bool TriggerSpikesStep()
    {
        if (currentStep == TutorialStep.Spikes && !waitingForTransition)
        {
            currentStep = TutorialStep.HealthFruit;
            SetText("That fruit can heal me up if I lose health.");
            return true;
        }

        return false;
    }

    public bool TriggerHealthFruitStep()
    {
        if (currentStep == TutorialStep.HealthFruit && !waitingForTransition)
        {
            currentStep = TutorialStep.Checkpoint;
            SetText("That sign is a checkpoint. Passing it will let me start back here if I fall.");
            return true;
        }

        return false;
    }

    public bool TriggerCheckpointStep()
    {
        if (currentStep == TutorialStep.Checkpoint && !waitingForTransition)
        {
            currentStep = TutorialStep.Star;
            SetText("It's very important that I collect 1 key per level and as many stars as I can.");
            return true;
        }

        return false;
    }

    public bool TriggerStarStep()
    {
        if (currentStep == TutorialStep.Star && !waitingForTransition)
        {
            currentStep = TutorialStep.Done;
            SetText("");
            return true;
        }

        return false;
    }

    public void ShowTemporaryMessage(string message)
    {
        if (temporaryMessageRoutine != null)
        {
            StopCoroutine(temporaryMessageRoutine);
        }

        temporaryMessageRoutine = StartCoroutine(ShowTemporaryMessageCo(message));
    }

    private IEnumerator ShowTemporaryMessageCo(string message)
    {
        string previousText = "";

        if (tutorialText != null)
        {
            previousText = tutorialText.text;
        }

        SetText(message);

        yield return new WaitForSeconds(temporaryMessageDuration);

        if (tutorialText != null && tutorialText.text == message)
        {
            SetText(previousText);
        }

        temporaryMessageRoutine = null;
    }

    private IEnumerator DelayedStepChange(TutorialStep nextStep, string nextMessage)
    {
        waitingForTransition = true;
        yield return new WaitForSeconds(specialTextSwapDelay);
        currentStep = nextStep;
        SetText(nextMessage);
        waitingForTransition = false;
    }

    private void SetText(string message)
    {
        if (tutorialText != null)
        {
            tutorialText.text = message;
        }

        if (speechBubble != null)
        {
            speechBubble.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }
    }

    private void FollowSpeechBubble()
    {
        if (mainCam == null || speechBubble == null)
            return;

        if (!speechBubble.gameObject.activeSelf)
            return;

        Transform target = followTarget != null ? followTarget : player;

        if (target == null)
            return;

        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(mainCam, target.position + worldOffset);
        speechBubble.position = screenPos + (Vector3)screenOffset;
    }
}