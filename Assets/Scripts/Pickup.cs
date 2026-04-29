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

    [Header("Pickup Delay")]
    public bool usePickupDelay = false;
    public float pickupDelay = 0f;
    private float pickupDelayCounter;

    void Start()
    {
        pickupDelayCounter = pickupDelay;

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
        if (usePickupDelay && pickupDelayCounter > 0)
        {
            pickupDelayCounter -= Time.deltaTime;
        }
    }

    private string GetPickupSaveKey()
    {
        return SceneManager.GetActiveScene().name + "_" + pickupID + "_collected";
    }

    private bool CanBePickedUp()
    {
        if (!usePickupDelay)
        {
            return true;
        }

        return pickupDelayCounter <= 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void TryCollect(Collider2D other)
    {
        if (!other.CompareTag("Player") || !CanBePickedUp() || isCollected)
        {
            return;
        }

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