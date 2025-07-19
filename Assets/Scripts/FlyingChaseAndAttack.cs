using UnityEngine;

public class FlyingChaseAndAttack : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private float knockbackForce = 5f;

    private GameObject player;
    private PlayerMovement playerMovement;
    private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || playerMovement == null || !playerMovement.IsAlive) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        Vector2 knockbackDir = (player.transform.position - transform.position).normalized;
        Vector2 knockback = knockbackDir * knockbackForce;

        playerMovement.ApplyKnockbackWithRabbitEffect(knockback);
        playerMovement.TakeDamage(damageToPlayer, PlayerMovement.AttackerType.Rabbit);
    }

}