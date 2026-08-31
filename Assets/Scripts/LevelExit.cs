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
            if (MiltonMessagePopup.instance != null)
            {
                if (LevelManager.instance.KeysStillNeeded() <= 1)
                {
                    MiltonMessagePopup.instance.ShowTemporaryMessage("I need to get the key before I can progress.");
                }
                else
                {
                    MiltonMessagePopup.instance.ShowTemporaryMessage("I need to get the rest of the keys before I can progress.");
                }
            }
        }
    }
}