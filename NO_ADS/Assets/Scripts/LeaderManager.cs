using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class LeaderManager : MonoBehaviour
{
    string leaderboardID = "CgkI8_6wwuMFEAIQAQ";

    void Start()
    {
        // Инициализация Google Play Games
        PlayGamesPlatform.Activate();

        // Аутентификация игрока
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Успешная аутентификация!");
        }
        else
        {
            Debug.LogError("Не удалось аутентифицироваться");
        }
    }

    public void ShowLeaderboard()
    {
        // Проверка аутентификации перед отображением таблицы лидеров
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
            Debug.Log("Таблица лидеров открыта.");
        }
        else
        {
            Debug.LogError("Пользователь не аутентифицирован.");
            // Попытка повторной аутентификации, если пользователь не аутентифицирован
            PlayGamesPlatform.Instance.Authenticate(status =>
            {
                if (status == SignInStatus.Success)  // Здесь мы проверяем статус аутентификации
                {
                    Debug.Log("Успешная повторная аутентификация!");
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
                }
                else
                {
                    Debug.LogError("Не удалось аутентифицироваться повторно.");
                }
            });
        }
    }
}
