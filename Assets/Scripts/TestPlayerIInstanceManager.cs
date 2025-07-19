using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerInstanceManager : MonoBehaviour
{
    void Awake()
    {
        int playerCount = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None).Length;
        if (playerCount > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}