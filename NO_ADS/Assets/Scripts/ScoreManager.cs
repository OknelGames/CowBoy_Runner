using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class ScoreManager : MonoBehaviour
{
    public Transform player;  // Ссылка на игрока (или объект, который двигается вперед)
    public TextMeshProUGUI scoreText;    // UI элемент для отображения очков
    public TextMeshProUGUI bestScoreText; // UI элемент для отображения лучшего результата
    public float scoreMultiplier = 1.0f;  // Множитель для скорости начисления очков

    private float currentScore = 0.0f;  // Текущие очки игрока
    private float bestScore = 0.0f;  // Лучший результат
    private Vector3 startPosition;  // Начальная позиция игрока

    public TextMeshProUGUI test_1;

    void Start()
    {
        // Запоминаем начальную позицию
        startPosition = player.position;

        // Загружаем лучший результат из GlobalVariables
        bestScore = GlobalVariables.instance.bestScore;
        bestScoreText.text = "Best Score: " + bestScore.ToString("F0");
    }

    void Update()
    {
        // Считаем очки на основе пройденного расстояния
        float distanceTravelled = player.position.z - startPosition.z;
        currentScore = distanceTravelled * scoreMultiplier;
        scoreText.text = "Score: " + currentScore.ToString("F0");

        // Обновляем лучший результат
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            bestScoreText.text = "Best Score: " + bestScore.ToString("F0");

            // Сохраняем лучший результат в GlobalVariables
            GlobalVariables.instance.bestScore = bestScore;
            GlobalVariables.instance.SaveData();  // Сохраняем данные
            PostToLeaderboard((long)bestScore);
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        startPosition = player.position;  // Обнуляем начальную позицию
        scoreText.text = "Score: 0";
    }




    // Метод для отправки результата в таблицу лидеров Google Play Games
    public void PostToLeaderboard(long score)
    {
        string leaderboardID = "CgkI8_6wwuMFEAIQAQ";  // Замените на ваш ID таблицы лидеров

        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, leaderboardID, success =>
            {
                if (success)
                {
                    Debug.Log("Результат отправлен в таблицу лидеров: " + score);
                    test_1.text = "DONE";
                }
                else
                {
                    Debug.Log("Не удалось отправить результат.");
                    test_1.text = "НЕХУЯ не DONE";
                }
            });
        }
        else
        {
            Debug.Log("Пользователь не аутентифицирован в Google Play Games.");
        }
    }


}