using UnityEngine;

public class EnemySecond : MonoBehaviour
{
    [SerializeField] private float enemyMovementSpeed;
    [SerializeField] private  int direction = 1;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canMoveUp;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        direction = Random.Range(0, 2) == 0 ? 1 : -1;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb2D.linearVelocity = new Vector2(direction * enemyMovementSpeed, rb2D.linearVelocity.y);
        }

        if (canMoveUp)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, direction * enemyMovementSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Boundary boundary))
        {
            direction *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Wall wall))
        {
            direction *= -1;
        }
        
    }
    
}
