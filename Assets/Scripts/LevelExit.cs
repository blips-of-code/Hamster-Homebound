using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (LevelManager.instance == null)
        {
            return;
        }

        if (LevelManager.instance.HasEnoughKeysToExit())
        {
            LevelManager.instance.EndLevel();
        }
        else
        {
            string message;

            if (LevelManager.instance.keysNeededToProgress == 1)
            {
                message = "I need to get the key before I can progress.";
            }
            else
            {
                message = "I need to get the key before I can progress. (Hint: Frogs)";
            }

            if (Level1TutorialManager.instance != null)
            {
                Level1TutorialManager.instance.ShowTemporaryMessage(message);
            }
            else if (MiltonMessagePopup.instance != null)
            {
                MiltonMessagePopup.instance.ShowTemporaryMessage(message);
            }
        }
    }
}