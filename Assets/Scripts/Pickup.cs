using UnityEngine;

public class Pickup: MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player") && !wasCollected)
    {
        wasCollected = true;
        FindFirstObjectByType<GameSession>().AddToScore(pointsForCoinPickup);
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
