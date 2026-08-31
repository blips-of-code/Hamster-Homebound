using System.Collections;
using UnityEngine;
using TMPro;

public class MiltonMessagePopup : MonoBehaviour
{
    public static MiltonMessagePopup instance;

    public Transform player;
    public Transform followTarget;

    public Camera mainCam;
    public Canvas canvas;

    public RectTransform speechBubble;
    public TextMeshProUGUI messageText;

    public Vector3 worldOffset = Vector3.zero;
    public Vector2 screenOffset = Vector2.zero;

    public float defaultMessageDuration = 2f;

    private Coroutine currentMessageRoutine;

    private void Awake()
    {
        instance = this;

        if (speechBubble != null)
        {
            speechBubble.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        FollowSpeechBubble();
    }

    public void ShowTemporaryMessage(string message)
    {
        if (currentMessageRoutine != null)
        {
            StopCoroutine(currentMessageRoutine);
        }

        currentMessageRoutine = StartCoroutine(ShowTemporaryMessageCo(message, defaultMessageDuration));
    }

    public void ShowTemporaryMessage(string message, float duration)
    {
        if (currentMessageRoutine != null)
        {
            StopCoroutine(currentMessageRoutine);
        }

        currentMessageRoutine = StartCoroutine(ShowTemporaryMessageCo(message, duration));
    }

    private IEnumerator ShowTemporaryMessageCo(string message, float duration)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }

        if (speechBubble != null)
        {
            speechBubble.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(duration);

        if (speechBubble != null)
        {
            speechBubble.gameObject.SetActive(false);
        }

        currentMessageRoutine = null;
    }

    private void FollowSpeechBubble()
    {
        if (mainCam == null || canvas == null || speechBubble == null)
        {
            return;
        }

        if (!speechBubble.gameObject.activeSelf)
        {
            return;
        }

        Transform target = followTarget != null ? followTarget : player;

        if (target == null)
        {
            return;
        }

        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position + worldOffset);
        Vector3 targetPos = screenPos + (Vector3)screenOffset;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            speechBubble.position = targetPos;
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 localPoint;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                targetPos,
                canvas.worldCamera,
                out localPoint
            );

            speechBubble.anchoredPosition = localPoint;
        }
    }
}