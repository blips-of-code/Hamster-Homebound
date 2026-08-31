using UnityEngine;

public class MessagePopupTrigger : MonoBehaviour
{
    [TextArea]
    public string message = "Watch out! That's a dangerous frog, double jump on it to kill it.";

    public float messageDuration = 3f;
    public bool triggerOnce = true;

    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (triggerOnce && hasTriggered)
        {
            return;
        }

        if (MiltonMessagePopup.instance != null)
        {
            MiltonMessagePopup.instance.ShowTemporaryMessage(message, messageDuration);
        }

        if (triggerOnce)
        {
            hasTriggered = true;
            gameObject.SetActive(false);
        }
    }
}