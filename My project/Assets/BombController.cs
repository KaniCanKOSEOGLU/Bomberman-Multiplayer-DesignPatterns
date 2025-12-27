using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour
{
    public float delay = 3f;           // Patlama süresi
    public GameObject explosionPrefab; // Kýrmýzý Kare / Efekt

    // Bu deðer Fabrika (BombFactory) tarafýndan atanýr
    public float explosionRange = 1.2f;

    void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(delay);

        // 1. GÖRSEL EFEKT
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Ses Çalma
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySFX(AudioManager.instance.explosion);

        // 2. MANTIK (OBSERVER YAYINI)
        // Menzili dinamik olarak gönderiyoruz
        GameEventManager.TriggerExplosion(transform.position, explosionRange);

        // 3. YOK OLMA
        Destroy(gameObject);
    }
}