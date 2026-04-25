using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFrog : MonoBehaviour
{
    public GameObject deathEffect;
    public float bounceForce = 10f;

    [Header("Optional Key Drop")]
    public bool dropsKeyOnDeath;
    public GameObject keyPickupPrefab;
    public Transform keySpawnPoint;
    public string keyPickupID = "L2Key";

    private bool isDead;
    private Collider2D[] frogColliders;

    void Start()
    {
        frogColliders = GetComponents<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (SimplePlayerMovement.instance == null)
        {
            return;
        }

        Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRB == null)
        {
            return;
        }

        if (SimplePlayerMovement.instance.IsInDoubleJumpKillState())
        {
            isDead = true;

            if (frogColliders != null)
            {
                for (int i = 0; i < frogColliders.Length; i++)
                {
                    frogColliders[i].enabled = false;
                }
            }

            SimplePlayerMovement.instance.doubleJumpKillReady = false;
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, bounceForce);

            TryDropKey();

            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);
            return;
        }

        if (PlayerHealthController.instance != null)
        {
            PlayerHealthController.instance.DealDamage();
        }
    }

    private void TryDropKey()
    {
        if (!dropsKeyOnDeath)
        {
            return;
        }

        if (keyPickupPrefab == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(keyPickupID))
        {
            return;
        }

        if (PlayerPrefs.GetInt(GetKeySaveKey(), 0) == 1)
        {
            return;
        }

        Vector3 spawnPosition = transform.position;

        if (keySpawnPoint != null)
        {
            spawnPosition = keySpawnPoint.position;
        }

        GameObject spawnedKey = Instantiate(keyPickupPrefab, spawnPosition, Quaternion.identity);

        Pickup pickupScript = spawnedKey.GetComponent<Pickup>();
        if (pickupScript != null)
        {
            pickupScript.isGem = false;
            pickupScript.isHeal = false;
            pickupScript.isKey = true;
            pickupScript.pickupID = keyPickupID;
        }
    }

    private string GetKeySaveKey()
    {
        return SceneManager.GetActiveScene().name + "_" + keyPickupID + "_collected";
    }
}