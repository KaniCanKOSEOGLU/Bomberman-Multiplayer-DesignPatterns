using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Harita Ayarlarý")]
    public int width = 19;
    public int height = 13;

    [Header("Tema Fabrikasý")]
    public GameObject factoryObject;
    private IThemeFactory currentFactory;

    [Header("Çakýþma Ayarlarý")]
    public LayerMask obstacleLayer;

    [Range(0, 1)] public float hardWallChance = 0.1f;
    [Range(0, 1)] public float brickWallChance = 0.2f;

    public void GenerateMap()
    {
        if (factoryObject != null)
        {
            currentFactory = factoryObject.GetComponent<IThemeFactory>();
        }

        if (currentFactory == null)
        {
            Debug.LogError("HATA: Fabrika bulunamadý!");
            return;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int xOffset = width / 2;
        int yOffset = height / 2;

        for (int x = -xOffset; x <= xOffset; x++)
        {
            for (int y = -yOffset; y <= yOffset; y++)
            {
                Vector2 pos = new Vector2(x, y);

                currentFactory.CreateFloor(pos).transform.SetParent(transform);

                if (Physics2D.OverlapCircle(pos, 0.4f, obstacleLayer) != null) continue;

                if (x == -xOffset || x == xOffset || y == -yOffset || y == yOffset)
                {
                    currentFactory.CreateSolidWall(pos).transform.SetParent(transform);
                }
                else if (x % 2 == 0 && y % 2 == 0)
                {
                    currentFactory.CreateSolidWall(pos).transform.SetParent(transform);
                }
                else if (IsSafeZone(x, y) == false)
                {
                    float randomValue = Random.value;

                    if (randomValue < hardWallChance)
                    {
                        currentFactory.CreateHardWall(pos).transform.SetParent(transform);
                    }
                    else if (randomValue < (hardWallChance + brickWallChance))
                    {
                        currentFactory.CreateDestructibleWall(pos).transform.SetParent(transform);
                    }
                }
            }
        }
    }

    bool IsSafeZone(int x, int y)
    {
        int xOffset = width / 2;
        int yOffset = height / 2;
        if (x <= -xOffset + 2 && y >= yOffset - 2) return true;
        if (x >= xOffset - 2 && y <= -yOffset + 2) return true;
        return false;
    }
}