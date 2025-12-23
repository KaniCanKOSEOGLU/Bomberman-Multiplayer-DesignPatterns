using UnityEngine;

// --- ARAYÜZ ---
public interface IEnemyState
{
    void Enter(EnemyAI enemy);
    void Execute(EnemyAI enemy);
    void Exit(EnemyAI enemy);
}

// --- DEVRÝYE (Rastgele Gezme) ---
public class PatrolState : IEnemyState
{
    private float timer;
    private Vector2 randomDirection;

    public void Enter(EnemyAI enemy)
    {
        enemy.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0, 1f); // Mor
        SetRandomDirection();
    }

    public void Execute(EnemyAI enemy)
    {
        // Fiziksel hareket
        enemy.MoveEnemy(randomDirection);

        timer += Time.deltaTime;
        if (timer > 2f) // 2 saniyede bir yön deðiþtir
        {
            SetRandomDirection();
            timer = 0;
        }

        // Oyuncuyu gördü mü?
        if (enemy.playerTarget != null)
        {
            float distance = Vector2.Distance(enemy.transform.position, enemy.playerTarget.position);
            if (distance < enemy.detectionRange)
            {
                enemy.ChangeState(new ChaseState());
            }
        }
    }

    public void Exit(EnemyAI enemy) { }

    void SetRandomDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        randomDirection = new Vector2(x, y).normalized;
    }
}

// --- KOVALAMA (Basit Takip) ---
public class ChaseState : IEnemyState
{
    public void Enter(EnemyAI enemy)
    {
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void Execute(EnemyAI enemy)
    {
        if (enemy.playerTarget == null)
        {
            enemy.ChangeState(new PatrolState());
            return;
        }

        // Oyuncuya doðru dümdüz git (Duvar varsa takýlýr - Simple AI)
        Vector2 direction = (enemy.playerTarget.position - enemy.transform.position).normalized;
        enemy.MoveEnemy(direction);

        // Oyuncu uzaklaþtý mý?
        float distance = Vector2.Distance(enemy.transform.position, enemy.playerTarget.position);
        if (distance > enemy.stopChasingRange)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0, 1f);
    }
}

// --- YÖNETÝCÝ ---
public class EnemyAI : MonoBehaviour
{
    [Header("Ayarlar")]
    public float speed = 2f;
    public float detectionRange = 3.5f;
    public float stopChasingRange = 6f;

    [HideInInspector] public Transform playerTarget;
    private IEnemyState currentState;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTarget = playerObj.transform;

        ChangeState(new PatrolState());
    }

    void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.Execute(this);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null) currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    // --- FÝZÝKSEL HAREKET (Duvar Ýçinden Geçmeyi Önler) ---
    public void MoveEnemy(Vector2 direction)
    {
        if (rb != null)
        {
            // MovePosition, fizik motorunu kullanarak hareket ettirir.
            // Önünde duvar varsa gitmez, durur.
            Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }
}