using UnityEngine;

// ABSTRACT FACTORY ARAYÜZÜ
// Her tema fabrikasý bu ürünleri üretmek zorundadýr.
public interface IThemeFactory
{
    GameObject CreateSolidWall(Vector2 position);       // Kýrýlmaz Duvar
    GameObject CreateDestructibleWall(Vector2 position);// Tuðla
    GameObject CreateFloor(Vector2 position);
    GameObject CreateHardWall(Vector2 position);// Zemin
}
