using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private void OnEnable()
    {
        // Radyoyu (Patlama Olayýný) dinlemeye baþla
        // Observer Pattern: Olay tetiklenince HandleExplosion çalýþýr
        GameEventManager.OnExplosion += HandleExplosion;
    }

    private void OnDisable()
    {
        // Obje yok olunca veya kapanýnca dinlemeyi býrak (Hata almamak için)
        GameEventManager.OnExplosion -= HandleExplosion;
    }

    // --- 1. PATLAMA HASARI ---
    // Patlama pozisyonu sunucudan geldiði için herkes için aynýdýr.
    // Bu yüzden burada "Player" veya "RemotePlayer" ayrýmý yapmaya gerek yok.
    // Patlamaya yakýn olan herkes ölür.
    private void HandleExplosion(Vector2 explosionPos, float range)
    {
        // Oyuncu ile patlama arasýndaki mesafeyi ölç
        float distance = Vector2.Distance(transform.position, explosionPos);

        // Eðer mesafe menzil içindeyse öl
        if (distance <= range)
        {
            Die();
        }
    }

    // --- 2. DÜÞMAN ÇARPIÞMASI (Kritik Düzeltme) ---
    // Düþmanlarýn konumu her bilgisayarda biraz farklý olabilir (Lag yüzünden).
    // Bu yüzden sadece KENDÝ karakterimiz düþmana deðerse ölüyoruz.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eðer çarpan þey bir "Düþman" (Enemy AI) ise
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // KONTROL: Çarpan kiþi BEN miyim? (Local Player)
            if (gameObject.CompareTag("Player"))
            {
                Debug.Log("Düþman BANA çarptý! Ölüyorum...");
                Die();
            }

            // EÐER "RemotePlayer" ÝSE HÝÇBÝR ÞEY YAPMA!
            // Býrak o kendi bilgisayarýnda ölsün ve sunucu bize "Rakip Öldü" desin.
        }
    }

    // --- ÖLÜM FONKSÝYONU ---
    void Die()
    {
        Debug.Log($"{gameObject.name} Öldü!");

        // Ölüm olayýný yayýnlama (GameManager bunu duyup oyunu bitirecek)
        // gameObject.tag sayesinde kimin öldüðünü ("Player" mý "RemotePlayer" mý) bildiriyoruz.
        GameEventManager.TriggerPlayerDeath(gameObject.tag);

        // Karakteri sahneden sil
        Destroy(gameObject);
    }
}