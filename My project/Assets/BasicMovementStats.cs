public class BasicMovementStats : IMovementStats
{
    private float baseSpeed;

    public BasicMovementStats(float speed)
    {
        this.baseSpeed = speed;
    }

    public float GetSpeed()
    {
        return baseSpeed; // Normal hýz neyse onu döndürür
    }
}
