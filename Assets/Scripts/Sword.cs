using UnityEngine;

public class PlayerSwordPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerAttack>().EnableSword();
            Destroy(gameObject);
        }
    }
}