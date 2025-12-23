using UnityEngine;
using System;

public class GameEventManager : MonoBehaviour
{
    // Olay Tanýmlarý
    public static event Action<Vector2, float> OnExplosion;
    public static event Action<string> OnPlayerDeath;

    // Patlama Olayýný Tetikle
    public static void TriggerExplosion(Vector2 position, float range)
    {
        if (OnExplosion != null)
        {
            OnExplosion.Invoke(position, range);
        }
    }

    // Ölüm Olayýný Tetikle
    public static void TriggerPlayerDeath(string tag)
    {
        if (OnPlayerDeath != null) OnPlayerDeath.Invoke(tag);
    }
}