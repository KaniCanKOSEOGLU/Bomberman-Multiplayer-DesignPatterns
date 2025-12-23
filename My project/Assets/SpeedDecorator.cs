using UnityEngine;

// DECORATOR PATTERN: Hýzlandýrýcý Katman
public class SpeedDecorator : IMovementStats
{
    private IMovementStats wrappedStats; // Ýçindeki (önceki) hýz yapýsý
    private float multiplier;            // Çarpan (Örn: 1.2)

    // Kurucu Metot (Constructor)
    public SpeedDecorator(IMovementStats stats, float multiplier)
    {
        this.wrappedStats = stats;
        this.multiplier = multiplier;
    }

    public float GetSpeed()
    {
        // ÖNEMLÝ: Sabit sayý döndürmüyoruz.
        // Ýçindeki hýzý alýp çarpanla çarpýyoruz.
        // Böylece iç içe 10 tane de olsa hepsi birbirini çarparak büyütür.
        return wrappedStats.GetSpeed() * multiplier;
    }
}