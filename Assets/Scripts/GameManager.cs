using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverImage;
    public GameObject startButton;
    public GameObject player;
    public GameObject background;
    public GameObject restartText;
    private bool gameOver = false;
    private static bool restart = false;
    private void Start()
    {
        if (restart)
        {
            startButton.SetActive(false);
            gameOverImage.SetActive(false);
            player.gameObject.SetActive(true);
            background.SetActive(false);
            restartText.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
            player.SetActive(false);
            restart = true;
        }
    }

    private void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.Return))
        {
            restart = true;
            StartGame();
        }
    }
    public void GameOver()
    {
        background.gameObject.SetActive(true);
        gameOverImage.SetActive(true);
        //player.gameObject.SetActive(false);
        //startButton.SetActive(true);
        restartText.SetActive(true);
        gameOver = true;

        Time.timeScale = 0f;
    }
    public void StartGame()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
