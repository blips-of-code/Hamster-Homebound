using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CageRescueController : MonoBehaviour
{
    public Animator anim;

    public int keysNeeded = 3;

    [Header("Optional Message")]
    [TextArea]
    public string notEnoughKeysMessage = "I need all 3 keys before I can free them.";
    public float messageDuration = 2.5f;

    [Header("Optional Next Scene")]
    public bool loadSceneAfterOpen;
    public string sceneToLoad;
    public float sceneLoadDelay = 2f;

    private bool isOpened;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Cage trigger entered by: " + other.name);

        if (isOpened)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (LevelManager.instance == null)
        {
            Debug.Log("LevelManager.instance is null");
            return;
        }

        Debug.Log("Player keys: " + LevelManager.instance.keysCollected + " / Needed: " + keysNeeded);

        if (LevelManager.instance.keysCollected >= keysNeeded)
        {
            OpenCage();
        }
        else
        {
            Debug.Log("Not enough keys");

            if (MiltonMessagePopup.instance != null)
            {
                MiltonMessagePopup.instance.ShowTemporaryMessage(notEnoughKeysMessage, messageDuration);
            }
        }
    }

    public void OpenCage()
    {
        if (isOpened)
        {
            return;
        }

        isOpened = true;
        Debug.Log("Opening cage");

        Collider2D triggerCol = GetComponent<Collider2D>();
        if (triggerCol != null)
        {
            triggerCol.enabled = false;
        }

        if (anim != null)
        {
            anim.SetTrigger("OpenCage");
            Debug.Log("OpenCage trigger sent to animator");
        }
        else
        {
            Debug.Log("Animator is not assigned");
        }

        if (loadSceneAfterOpen)
        {
            Debug.Log("Scene load requested. Scene name: " + sceneToLoad);
            StartCoroutine(LoadSceneCo());
        }
        else
        {
            Debug.Log("loadSceneAfterOpen is false");
        }
    }

    private IEnumerator LoadSceneCo()
    {
        yield return new WaitForSeconds(sceneLoadDelay);

        Debug.Log("Trying to load scene: " + sceneToLoad);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.Log("sceneToLoad is empty");
        }
    }
}