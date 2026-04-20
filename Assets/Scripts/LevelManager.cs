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
        

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

public IEnumerator EndLevelCo()
{
    Debug.Log("EndLevelCo started");

    if(CameraController.instance != null)
    {
        CameraController.instance.stopFollow = true;
    }

    if(UIController.instance != null && UIController.instance.levelCompleteText != null)
    {
        UIController.instance.levelCompleteText.SetActive(true);
    }

    yield return new WaitForSeconds(1.5f);

    yield return new WaitForSeconds(3f);

    SceneManager.LoadScene(levelToLoad);
}
}
