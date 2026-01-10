using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager _instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int pointsPerSecond = 1;

    private int score = 0;
    private float timer = 0f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        scoreText.text = "Score: " + score;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            AddScore(pointsPerSecond);
            timer = 0f;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

}
