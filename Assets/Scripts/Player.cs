using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private int enemyKilled = 0;
    [SerializeField] private float playerMovementSpeed = 1f;
    [SerializeField] private float speedMultiplier = 0.5f;
    [SerializeField] private float raycastLengthUp = 0.2f;
    [SerializeField] private float raycastLengthDown = 0.2f;
    [SerializeField] private float raycastLengthRight = 0.2f;
    [SerializeField] private float raycastLengthLeft = 0.2f;
    [SerializeField] private float rayMaxDistance = 10f;
    [SerializeField] private float offsetX = 0.5f;
    [SerializeField] private float offsetY = 0.5f;
    [SerializeField] private float speedWayBoostMultiplier = 3f;
    [SerializeField] private bool IsCarryingKey = false;
    [SerializeField] private GameObject Door;
    [SerializeField] private GameObject swordSprite;
    [SerializeField] private LayerMask wallLayerMask;

    public EventHandler OnEnemyKilled;
    public EventHandler OnEnemySecondKilled;
    
    private Rigidbody2D rb2D;
    private Vector2 movementInput;
    private BoxCollider2D boxCollider2D;
    private bool hasSwordEquipped;
    
    
    


    private void Awake()
    {
        Instance = this;
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hasSwordEquipped = false;
        swordSprite.SetActive(false);

    }

    private void Update()
    {
        GetIsNearWall();
        movementInput = PlayerInput.Instance.GetMovementInput();
    }

    private void FixedUpdate()
    {
        

        // Movement Code, vereinfachbar.
        if (movementInput.x > 0)
        {
            SpeedBoost();
            Vector2 right = transform.right;
            
            rb2D.linearVelocity = right * playerMovementSpeed;
        }
        else if (movementInput.x < 0)
        {
            SpeedBoost();
            Vector2 left = -transform.right;
            
            rb2D.linearVelocity = left * playerMovementSpeed;
        }
        else if (movementInput.y > 0)
        {
            SpeedBoost();
            Vector2 up = transform.up;
            
            rb2D.linearVelocity = up * playerMovementSpeed;
        }
        else if (movementInput.y < 0)
        {
            SpeedBoost();
            Vector2 down = -transform.up;
            
            rb2D.linearVelocity = down * playerMovementSpeed;
        }
        
    }

    // Player stirbt wenn er Wand berührt
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Wall wall))
        {
            Death();
        }
        else if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            if (hasSwordEquipped)
            {

                Destroy(other.gameObject);
                OnEnemyKilled?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Death();
            }
        }
        else if (other.gameObject.TryGetComponent(out EnemySecond enemySecond))
        {
            if (hasSwordEquipped)
            {
                Destroy(other.gameObject);
                enemyKilled++;
                if (enemyKilled == 7)
                {
                    OnEnemySecondKilled?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                Death();
            }
        }
        
        
    }

    //Überprüft ob Tür betretbar ist
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Key key))
        {
            Destroy(Door);
        }

        if (other.TryGetComponent(out SwordPickup sword))
        {
            hasSwordEquipped = true;
            swordSprite.SetActive(true);
            
        }

        if (other.TryGetComponent(out SpeedWay speedWay))
        {
            playerMovementSpeed += speedWayBoostMultiplier;
        }
        
        
        if(other.TryGetComponent(out Goal goal))
        {
            GameManager.Instance.LoadNextLevel();
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out SpeedWay speedWay))
        {
            playerMovementSpeed -= speedWayBoostMultiplier;
        }
        
    }

    private void Death()
    {
        Destroy(gameObject);
      SceneManager.LoadScene(0);
   }

    public void KillMobility()
    {
        rb2D.linearVelocity = Vector2.zero;
    }


    //Checken ob der Charakter nahe einer Wand ist.
    private bool GetIsNearWall()
    {
        Vector3 rayVectorOriginUp = new Vector3(boxCollider2D.bounds.center.x, boxCollider2D.bounds.max.y, rayMaxDistance);
        RaycastHit2D raycastHitUp2D = Physics2D.Raycast(rayVectorOriginUp, Vector2.up, raycastLengthUp, wallLayerMask);

        Vector2 rayVectorOriginDown = new Vector2(boxCollider2D.bounds.center.x, boxCollider2D.bounds.min.y);
        RaycastHit2D raycastHitDown2D = Physics2D.Raycast(rayVectorOriginDown, Vector2.down, raycastLengthDown, wallLayerMask);
        
        Vector2 rayVectorOriginRight = new Vector2(boxCollider2D.bounds.center.x + offsetX, boxCollider2D.bounds.min.y + offsetY);
        RaycastHit2D raycastHitRight2D = Physics2D.Raycast(rayVectorOriginRight, Vector2.right, raycastLengthRight, wallLayerMask);
        
        Vector2 rayVectorOriginLeft = new Vector2(boxCollider2D.bounds.center.x - offsetX, boxCollider2D.bounds.min.y + offsetY);
        RaycastHit2D raycastHitLeft2D = Physics2D.Raycast(rayVectorOriginLeft, Vector2.left, raycastLengthLeft, wallLayerMask);
        
        
        //Nur für Raycast Visualisierung.
        Vector2 raycastDirectionUp = Vector2.up * raycastLengthUp;
        Vector2 raycastDirectionDown = Vector2.down * raycastLengthDown;
        Vector2 raycastDirectionRight = Vector2.right * raycastLengthRight;
        Vector2 raycastDirectionLeft = Vector2.left * raycastLengthLeft;
        
        Debug.DrawRay(rayVectorOriginUp, raycastDirectionUp, raycastHitUp2D.collider ? Color.brown : Color.blue);
        Debug.DrawRay(rayVectorOriginDown, raycastDirectionDown, raycastHitDown2D.collider ? Color.brown : Color.blue);
        Debug.DrawRay(rayVectorOriginRight, raycastDirectionRight, raycastHitRight2D.collider ? Color.brown : Color.blue);
        Debug.DrawRay(rayVectorOriginLeft, raycastDirectionLeft, raycastHitLeft2D.collider ? Color.brown : Color.blue);


        return raycastHitUp2D.collider != null || raycastHitDown2D.collider != null ||
               raycastHitLeft2D.collider != null || raycastHitRight2D.collider != null;

    }

    public float SpeedBoost()
    {
        if (GetIsNearWall())
        {
            return playerMovementSpeed += speedMultiplier;
        }

        return 0;
    }
    
    
    
}
