using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb2d.linearVelocity = new Vector2(speed, rb2d.linearVelocity.y);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        speed = -speed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb2d.linearVelocity.x)), transform.localScale.y);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(player.maxHealth, PlayerMovement.AttackerType.Skeleton);
            }
        }
    }


}