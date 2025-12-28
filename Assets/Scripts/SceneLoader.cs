using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    [SerializeField] private int sceneBuildIndex = -1;

    [SerializeField] private LoadSceneMode loadMode = LoadSceneMode.Single;

    public void Load()
    {
        if (!string.IsNullOrWhiteSpace(sceneName))
        {
            SceneManager.LoadScene(sceneName, loadMode);
            return;
        }

        if (sceneBuildIndex >= 0)
        {
            SceneManager.LoadScene(sceneBuildIndex, loadMode);
            return;
        }

    }

    public void LoadByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        SceneManager.LoadScene(name, loadMode);
    }

    public void ReloadCurrent()
    {
        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name, loadMode);
    }

    public void LoadNextInBuild()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next >= SceneManager.sceneCountInBuildSettings)
        {
            return;
        }

        SceneManager.LoadScene(next, loadMode);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
