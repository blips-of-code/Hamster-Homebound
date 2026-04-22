using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToRespawn;

    public int gemsCollected;

    public string levelToLoad;

    public float timeInLevel;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeInLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeInLevel += Time.deltaTime;
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

    if(AudioManager.instance != null)
    {
        AudioManager.instance.PlaySFX(8);
    }

<<<<<<< Updated upstream
    yield return new WaitForSeconds(waitToRespawn);
    Debug.Log("Wait finished");

    SimplePlayerMovement.instance.gameObject.SetActive(true);
    SimplePlayerMovement.instance.transform.position = CheckpointController.instance.spawnPoint;

    PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;

    if(UIController.instance != null)
    {
        UIController.instance.UpdateHealthDisplay();
    }
}
        

=======
>>>>>>> Stashed changes
    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

public IEnumerator EndLevelCo()
{
    Debug.Log("EndLevelCo started");

    if(CameraController.instance != null)
    {
<<<<<<< Updated upstream
=======
        //AudioManager.instance.PlayLevelVictory();

        SimplePlayerMovement.instance.stopInput = true;

>>>>>>> Stashed changes
        CameraController.instance.stopFollow = true;
    }

    if(UIController.instance != null && UIController.instance.levelCompleteText != null)
    {
        UIController.instance.levelCompleteText.SetActive(true);
    }

    yield return new WaitForSeconds(1.5f);

<<<<<<< Updated upstream
    yield return new WaitForSeconds(3f);

    SceneManager.LoadScene(levelToLoad);
}
=======
        //UIController.instance.FadeToBlack();

       // yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + 3f);

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_gems"))
        {
            if(gemsCollected > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_gems"))
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
            if(timeInLevel < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_time"))
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
        }

        SceneManager.LoadScene(levelToLoad);
    }
>>>>>>> Stashed changes
}
