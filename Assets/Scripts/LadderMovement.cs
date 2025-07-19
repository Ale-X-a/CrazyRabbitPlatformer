using UnityEngine;

public class LadderMovement: MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rb2d;

    private float vertical;
    private bool isLadder;
    private bool isClimbing;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            rb2d.gravityScale = 3f;
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f); 
        }
    }

    private void Update()
    {
        vertical = Input.GetAxis("Vertical");
        isClimbing = isLadder && Mathf.Abs(vertical) > 0.1f;
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb2d.gravityScale = 0f;
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, vertical * speed);
        }
        else if (!isLadder)
        {
            rb2d.gravityScale = 3f;
        }
    }
}