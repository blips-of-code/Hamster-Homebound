using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickup : MonoBehaviour
{
    public bool isGem, isHeal, isKey;

    public string pickupID;

    private bool isCollected;

    public GameObject pickupEffect;

    void Start()
    {
        if (isGem || isKey)
        {
            if (PlayerPrefs.GetInt(GetPickupSaveKey(), 0) == 1)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {

    }

    private string GetPickupSaveKey()
    {
        return SceneManager.GetActiveScene().name + "_" + pickupID + "_collected";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            if (isGem)
            {
                if (PlayerPrefs.GetInt(GetPickupSaveKey(), 0) == 0)
                {
                    LevelManager.instance.gemsCollected++;
                    LevelManager.instance.totalGemsCollected++;

                    PlayerPrefs.SetInt(GetPickupSaveKey(), 1);
                    PlayerPrefs.SetInt("TotalGemsCollected", LevelManager.instance.totalGemsCollected);
                    PlayerPrefs.Save();
                }

                isCollected = true;
                Destroy(gameObject);

                Instantiate(pickupEffect, transform.position, transform.rotation);

                if (UIController.instance != null)
                {
                    UIController.instance.UpdateGemCount();
                }

                AudioManager.instance.PlaySFX(6);
            }

            if (isKey)
            {
                if (PlayerPrefs.GetInt(GetPickupSaveKey(), 0) == 0)
                {
                    LevelManager.instance.keysCollected++;

                    PlayerPrefs.SetInt(GetPickupSaveKey(), 1);
                    PlayerPrefs.SetInt("TotalKeysCollected", LevelManager.instance.keysCollected);
                    PlayerPrefs.Save();
                }

                isCollected = true;
                Destroy(gameObject);

                Instantiate(pickupEffect, transform.position, transform.rotation);

                if (UIController.instance != null)
                {
                    UIController.instance.UpdateKeyCount();
                }

                AudioManager.instance.PlaySFX(6);
            }

            if (isHeal)
            {
                if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)
                {
                    PlayerHealthController.instance.HealPlayer();

                    isCollected = true;
                    Destroy(gameObject);

                    Instantiate(pickupEffect, transform.position, transform.rotation);

                    AudioManager.instance.PlaySFX(7);
                }
            }
        }
    }
}