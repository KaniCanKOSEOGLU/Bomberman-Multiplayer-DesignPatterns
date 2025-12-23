using UnityEngine;

public class CityFactory : MonoBehaviour, IThemeFactory
{
    [Header("Þehir Temasý Prefablarý")]
    public GameObject solidWallPrefab;       // Metalik Gri
    public GameObject destructibleWallPrefab;// Kiremit Kýrmýzýsý
    public GameObject hardWallPrefab;        // Koyu Çelik
    public GameObject floorPrefab;           // Koyu Asfalt

    public GameObject CreateSolidWall(Vector2 position)
    {
        return Instantiate(solidWallPrefab, position, Quaternion.identity);
    }

    public GameObject CreateDestructibleWall(Vector2 position)
    {
        return Instantiate(destructibleWallPrefab, position, Quaternion.identity);
    }

    public GameObject CreateHardWall(Vector2 position)
    {
        return Instantiate(hardWallPrefab, position, Quaternion.identity);
    }

    public GameObject CreateFloor(Vector2 position)
    {
        return Instantiate(floorPrefab, position, Quaternion.identity);
    }
}
