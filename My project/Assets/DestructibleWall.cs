using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    [Header("Duvar Özellikleri")]
    public int health = 1;
    public Color damageColor = Color.red;

    [Header("Loot Sistemi")]
    // Dizi (Array) yapýyoruz ki birden fazla iksir koyabilelim
    public GameObject[] powerUpPrefabs;

    [Range(0, 100)] public int dropChance = 30;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        GameEventManager.OnExplosion += HandleExplosion;
    }

    private void OnDisable()
    {
        GameEventManager.OnExplosion -= HandleExplosion;
    }

    private void HandleExplosion(Vector2 explosionPos, float range)
    {
        float distance = Vector2.Distance(transform.position, explosionPos);

        if (distance <= range)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            SpawnPowerUp();
            Destroy(gameObject);
        }
        else
        {
            if (spriteRenderer != null) spriteRenderer.color = damageColor;
        }
    }

    void SpawnPowerUp()
    {
        // Dizi boþ deðilse ve þans tutarsa
        if (powerUpPrefabs != null && powerUpPrefabs.Length > 0 && Random.Range(0, 100) < dropChance)
        {
            // Rastgele bir iksir seç (0 ile Liste Uzunluðu arasýnda)
            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject selectedPowerUp = powerUpPrefabs[randomIndex];

            Instantiate(selectedPowerUp, transform.position, Quaternion.identity);
        }
    }
}
