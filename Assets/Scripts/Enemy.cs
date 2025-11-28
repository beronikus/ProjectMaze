using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

    
    [SerializeField] private float enemyMovementSpeed;
    [SerializeField] private  int direction = 1;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        direction = Random.Range(0, 2) == 0 ? 1 : -1;
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = new Vector2(direction * enemyMovementSpeed, rb2D.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Boundary boundary))
        {
            direction *= -1;
        }
    }
}
