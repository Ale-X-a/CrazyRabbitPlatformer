using UnityEngine;

public class BounceCharacters : MonoBehaviour
{
    [SerializeField] private float interactionRange = 1.5f;
    [SerializeField] private float interactionCooldown = 1.5f;
    [SerializeField] private float knockbackForce = 5f;

    private float lastInteractionTime;
    private GameObject player;
    private PlayerMovement playerMovement;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (player == null || playerMovement == null || !playerMovement.IsAlive) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= interactionRange && Time.time - lastInteractionTime >= interactionCooldown)
        {
            BounceInteraction();
            lastInteractionTime = Time.time;
        }
    }

    void BounceInteraction()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        if (playerMovement != null)
        {
            Vector2 knockbackDir = (player.transform.position - transform.position).normalized;
            Vector2 knockback = knockbackDir * knockbackForce;
            playerMovement.ApplyKnockbackWithBounceEffect(knockback);
        }
    }
}