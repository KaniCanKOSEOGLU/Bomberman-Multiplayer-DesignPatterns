using UnityEngine;

public class ForestFactory : MonoBehaviour, IThemeFactory
{
    [Header("Orman Temasý Prefablarý")]
    public GameObject solidWallPrefab;
    public GameObject destructibleWallPrefab;
    public GameObject hardWallPrefab;
    public GameObject floorPrefab;

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
