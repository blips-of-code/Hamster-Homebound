using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToRespawn;

    public int gemsCollected;
    public int totalGemsCollected;
    public int keysCollected;
    public int keysNeededToProgress;

    public string levelToLoad;

    public float timeInLevel;

    private void Awake()
    {
        instance = this;

        gemsCollected = 0;
        totalGemsCollected = PlayerPrefs.GetInt("TotalGemsCollected", 0);
        keysCollected = PlayerPrefs.GetInt("TotalKeysCollected", 0);
    }

    void Start()
    {
        timeInLevel = 0f;

        if (UIController.instance != null)
        {
            UIController.instance.UpdateGemCount();
            UIController.instance.UpdateKeyCount();
        }
    }

    void Update()
    {
        timeInLevel += Time.deltaTime;
    }

    public bool HasEnoughKeysToExit()
    {
        return keysCollected >= keysNeededToProgress;
    }

    public int KeysStillNeeded()
    {
        return Mathf.Max(0, keysNeededToProgress - keysCollected);
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    private IEnumerator RespawnCo()
    {
        Debug.Log("Respawn started");

        SimplePlayerMovement.instance.gameObject.SetActive(false);
        Debug.Log("Player disabled");

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(8);
        }

        yield return new WaitForSeconds(waitToRespawn);
        Debug.Log("Wait finished");

        SimplePlayerMovement.instance.gameObject.SetActive(true);
        SimplePlayerMovement.instance.transform.position = CheckpointController.instance.spawnPoint;

        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;

        if (UIController.instance != null)
        {
            UIController.instance.UpdateHealthDisplay();
        }
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

    public IEnumerator EndLevelCo()
    {
        Debug.Log("EndLevelCo started");

        if (CameraController.instance != null)
        {
            CameraController.instance.stopFollow = true;
        }

        if (SimplePlayerMovement.instance != null)
        {
            SimplePlayerMovement.instance.stopInput = true;
        }

        if (UIController.instance != null && UIController.instance.levelCompleteText != null)
        {
            UIController.instance.levelCompleteText.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_gems"))
        {
            if (gemsCollected > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_gems"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_gems", gemsCollected);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_gems", gemsCollected);
        }

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_time"))
        {
            if (timeInLevel < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_time"))
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
        }

        PlayerPrefs.SetInt("TotalGemsCollected", totalGemsCollected);
        PlayerPrefs.SetInt("TotalKeysCollected", keysCollected);
        PlayerPrefs.Save();

        SceneManager.LoadScene(levelToLoad);
    }
}