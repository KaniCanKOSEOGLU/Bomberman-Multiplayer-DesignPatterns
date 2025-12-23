using UnityEngine;

public class DesertFactory : MonoBehaviour, IThemeFactory
{
    [Header("Çöl Temasý Prefablarý")]
    public GameObject solidWallPrefab;       // Sarý Kýrýlmaz Duvar
    public GameObject destructibleWallPrefab;// Kumtaþý Tuðla
    public GameObject floorPrefab;           // Kum Zemini
    public GameObject hardWallPrefab; // Buna "Wall_Hard" veya özel bir çöl taþý sürükleyeceðiz
    public GameObject CreateSolidWall(Vector2 position)
    {
        return Instantiate(solidWallPrefab, position, Quaternion.identity);
    }

    public GameObject CreateDestructibleWall(Vector2 position)
    {
        return Instantiate(destructibleWallPrefab, position, Quaternion.identity);
    }

    public GameObject CreateFloor(Vector2 position)
    {
        return Instantiate(floorPrefab, position, Quaternion.identity);
    }
    // --- YENÝ EKLENEN FONKSÝYON ---
    public GameObject CreateHardWall(Vector2 position)
    {
        return Instantiate(hardWallPrefab, position, Quaternion.identity);
    }
}