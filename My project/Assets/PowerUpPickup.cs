using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sadece "Player" etiketli (Bizim karakterimiz) alabilir
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Ýksirin adýna göre güç ver
                // (Prefab isimleri "SpeedPowerUp" ve "FirePowerUp" olmalý)
                if (gameObject.name.Contains("Speed"))
                {
                    player.ApplySpeedBoost();
                }
                else if (gameObject.name.Contains("Fire"))
                {
                    player.IncreaseBombRange();
                }

                // Ses Çal
                if (AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(AudioManager.instance.powerUp);

                // Ýksiri yok et
                Destroy(gameObject);
            }
        }
    }
}