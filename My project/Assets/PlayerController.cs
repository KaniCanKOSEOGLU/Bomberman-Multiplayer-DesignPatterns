using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- DECORATOR PATTERN ---
    private IMovementStats movementStats;
    private Rigidbody2D rb;

    private Vector2 lastPosition;
    private Vector2 inputDir;
    public float bombRange = 1.2f;

    void Start()
    {
        movementStats = new BasicMovementStats(5f);
        rb = GetComponent<Rigidbody2D>();

        // Baþlangýçta inaktif býrakýldýysa, Start'ta tekrar inaktif yap
        if (enabled == false) enabled = true;
    }

    public void ApplySpeedBoost()
    {
        if (movementStats.GetSpeed() < 10f)
        {
            movementStats = new SpeedDecorator(movementStats, 1.1f);
            Debug.Log("HIZLANDIN! Yeni Hýz: " + movementStats.GetSpeed());
        }
    }

    public void IncreaseBombRange()
    {
        bombRange += 1.0f;
        Debug.Log("GÜÇLENDÝN! Yeni Bomba Menzili: " + bombRange);
    }

    void Update()
    {
        // 1. GÝRDÝLERÝ ALMA
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        inputDir = new Vector2(moveX, moveY).normalized;

        // 2. NETWORK BÝLDÝRÝMÝ
        if ((Vector2)transform.position != lastPosition)
        {
            if (NetworkManager.instance != null)
                NetworkManager.instance.SendPlayerMove(transform.position.x, transform.position.y);
            lastPosition = transform.position;
        }

        // 3. BOMBA KOYMA
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float x = Mathf.Round(transform.position.x);
            float y = Mathf.Round(transform.position.y);

            if (NetworkManager.instance != null)
            {
                NetworkManager.instance.SendBomb(new Vector2(x, y), bombRange);
                if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.bombPlace);
            }
        }
    }

    // 4. FÝZÝKSEL HAREKET (FixedUpdate)
    void FixedUpdate()
    {
        // Command Pattern'i FixedUpdate'te çalýþtýrýyoruz
        if (inputDir != Vector2.zero)
        {
            float currentSpeed = movementStats.GetSpeed();
            ICommand moveCommand = new MoveCommand(rb, inputDir, currentSpeed);
            moveCommand.Execute();
        }
    }
}