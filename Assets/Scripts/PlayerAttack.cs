using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject swordObject;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;

    private bool hasSword = false;

    void Start()
    {
        if (swordObject != null)
            swordObject.SetActive(false);
    }

    void Update()
    {
        if (hasSword && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F)))
        {
            Attack();
        }

    }

    public void EnableSword()
    {
        hasSword = true;
        if (swordObject != null)
            swordObject.SetActive(true);
    }


    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }

}