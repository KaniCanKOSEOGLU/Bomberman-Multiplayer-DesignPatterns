using UnityEngine;

public class BombFactory : MonoBehaviour
{
    [Header("Fabrika Ayarlarý")]
    public GameObject bombPrefab;

    // Bu deðiþkeni NetworkManager güncelleyecek
    public float nextBombRange = 1.2f;

    public GameObject CreateBomb(Vector2 position)
    {
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);

        // Menzil bilgisini bombanýn üzerindeki kontrole aktar
        BombController controller = bomb.GetComponent<BombController>();
        if (controller != null)
        {
            controller.explosionRange = nextBombRange;
        }

        return bomb;
    }
}
