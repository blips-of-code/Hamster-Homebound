using UnityEngine;

public class TutorialTriggerZone : MonoBehaviour
{
    public enum TriggerType
    {
        Spikes,
        HealthFruit,
        Checkpoint,
        Star
    }

    public TriggerType triggerType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isPlayer =
            other.CompareTag("Player") ||
            other.GetComponent<SimplePlayerMovement>() != null ||
            other.GetComponent<PlayerController>() != null;

        if (!isPlayer)
            return;

        if (Level1TutorialManager.instance == null)
            return;

        switch (triggerType)
        {
            case TriggerType.Spikes:
                Level1TutorialManager.instance.TriggerSpikesStep();
                break;

            case TriggerType.HealthFruit:
                Level1TutorialManager.instance.TriggerHealthFruitStep();
                break;

            case TriggerType.Checkpoint:
                Level1TutorialManager.instance.TriggerCheckpointStep();
                break;

            case TriggerType.Star:
                Level1TutorialManager.instance.TriggerStarStep();
                break;
        }

        gameObject.SetActive(false);
    }
}