using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0f;

        if (gameObject != null)
            gameObject.SetActive(true);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        if (gameObject != null)
            gameObject.SetActive(false);
    }
}