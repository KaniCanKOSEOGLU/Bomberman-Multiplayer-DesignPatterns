using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    void Start()
    {
        // 0.5 saniye sonra kendini yok et
        Destroy(gameObject, 0.5f);
    }
}
