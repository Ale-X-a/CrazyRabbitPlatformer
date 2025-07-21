using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteCanvas;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private float delayBeforeNextLevel = 3f;

    private bool levelCompleted = false;

    void Start()
    {
        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(false);

        if (endCanvas != null)
            endCanvas.SetActive(false);
    }

    public void ShowLevelComplete()
    {
        if (levelCompleted) return;

        levelCompleted = true;
        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(true);

        Invoke(nameof(HandleNextStep), delayBeforeNextLevel);
    }

    private void HandleNextStep()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 < totalScenes)
        {
            Debug.Log("Loading next scene: " + (currentSceneIndex + 1));
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            if (endCanvas != null)
                endCanvas.SetActive(true);
        }
    }
}