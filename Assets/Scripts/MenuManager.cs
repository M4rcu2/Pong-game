using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // BUTTON CALLS
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void StartCPUGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameCPUScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
