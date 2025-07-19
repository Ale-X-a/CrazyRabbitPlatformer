using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(0f, 0f);
    [SerializeField] public int maxHealth = 5;

    [SerializeField] private ParticleSystem BounceEffect;
    [SerializeField] private AudioClip BounceSound;

    [SerializeField] private ParticleSystem rabbitHitEffect;
    [SerializeField] private AudioClip rabbitHitSound;

    [SerializeField] private ParticleSystem skeletonHitEffect;
    [SerializeField] private AudioClip skeletonHitSound;

    public int currentHealth;
    private bool isAlive = true;
    private bool isJumping = false;

    private Vector2 moveInput;
    private Rigidbody2D rb2d;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2d;
    private BoxCollider2D boxCollider2D;
    private AudioSource audioSource;

    public bool IsAlive => isAlive;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isAlive) return;

        Run();
        FlipSprite();

        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);

        if (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground", "Bouncy")))
        {
            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAlive) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Hazards"))
        {
            Die();
        }
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground", "Bouncy"))) return;

        if (value.isPressed)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpSpeed);
            isJumping = true;
            animator.SetBool("isJumping", true);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);
        rb2d.linearVelocity = playerVelocity;
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        if (!playerHasHorizontalSpeed) return;

        transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    public enum AttackerType
    {
        Rabbit,
        Skeleton
    }

    public void TakeDamage(int damage, AttackerType attackerType)
    {
        if (!isAlive) return;

        currentHealth -= damage;

        switch (attackerType)
        {
            case AttackerType.Rabbit:
                ApplyKnockbackWithRabbitEffect(new Vector2(-transform.localScale.x * 5f, 5f));
                if (rabbitHitEffect != null) rabbitHitEffect.Play();
                if (rabbitHitSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(rabbitHitSound);
                }
                break;

            case AttackerType.Skeleton:
                ApplyKnockbackWithSkeletonEffect(new Vector2(-transform.localScale.x * 10f, 5f));
                if (skeletonHitEffect != null) skeletonHitEffect.Play();
                if (skeletonHitSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(skeletonHitSound);
                }
                break;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        isAlive = false;
        moveInput = Vector2.zero;
        rb2d.linearVelocity = deathKick;
        animator.SetTrigger("dead");
        StartCoroutine(HandleDeathAfterAnimation());
    }

    IEnumerator HandleDeathAfterAnimation()
    {
        yield return new WaitForSeconds(3f);
        var session = FindFirstObjectByType<GameSession>();
        if (session != null)
        {
            session.ProcessPlayerDeath();
        }
    }

    public void ApplyKnockbackWithBounceEffect(Vector2 knockbackForce)
    {
        rb2d.AddForce(knockbackForce, ForceMode2D.Impulse);
        if (BounceEffect != null) BounceEffect.Play();
        if (BounceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(BounceSound);
        }
    }

    public void ApplyKnockbackWithRabbitEffect(Vector2 knockbackForce)
    {
        rb2d.AddForce(knockbackForce, ForceMode2D.Impulse);
        if (rabbitHitEffect != null) rabbitHitEffect.Play();
        if (rabbitHitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(rabbitHitSound);
        }
    }

    public void ApplyKnockbackWithSkeletonEffect(Vector2 knockbackForce)
    {
        rb2d.AddForce(knockbackForce, ForceMode2D.Impulse);
        if (skeletonHitEffect != null) skeletonHitEffect.Play();
        if (skeletonHitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(skeletonHitSound);
        }
    }
}
