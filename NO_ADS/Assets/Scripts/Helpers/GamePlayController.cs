using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;
    public GameObject[] obstaclePrefabs;
    public GameObject[] zombiePrefabs;
    public Transform[] lanes;
    public float min_ObstacleDelay = 10f, max_ObstecalDelay = 40f;
    private float halfGroundSize;
    private BaseController playerController;

    private TextMeshProUGUI score_Text;
    private int Kill_count;

    [SerializeField]
    private GameObject PausePanel;

    [SerializeField]
    private GameObject gameover_Panel;

    [SerializeField]
    private TextMeshProUGUI Final_Score;
    public int gameover_count = 0;
    public int game_before_ADS = 2;




    public TextMeshProUGUI speedUpText; // UI Text или TMP_Text для отображения сообщения
    public float displayTime = 2.0f; // Время, в течение которого будет отображаться сообщение "Speed UP"
    private float speedUpDisplayTimer = 0.0f;

    public float speedIncreaseInterval = 30.0f;  // Интервал увеличения скорости (в секундах)
    public float speedMultiplier = 0.1f;         // Множитель увеличения скорости
    private float elapsedTime = 0.0f;            // Прошедшее время с начала игры



    void Update()
    {
        // Увеличиваем прошедшее время
        elapsedTime += Time.deltaTime;

        // Проверяем, прошел ли интервал для увеличения скорости
        if (elapsedTime >= speedIncreaseInterval)
        {
            // Увеличиваем скорость игры
            Time.timeScale += speedMultiplier;

            // Сбрасываем счетчик времени
            elapsedTime = 0.0f;

            // Отображаем текст "Speed UP"
            ShowSpeedUpText();
        }

        // Скрываем текст через заданное время
        if (speedUpText.gameObject.activeSelf)
        {
            speedUpDisplayTimer += Time.deltaTime;
            if (speedUpDisplayTimer >= displayTime)
            {
                HideSpeedUpText();
            }
        }
    }

    // Метод для отображения текста "Speed UP"
    void ShowSpeedUpText()
    {
        speedUpText.gameObject.SetActive(true); // Включаем текстовый элемент
        speedUpDisplayTimer = 0.0f; // Сбрасываем таймер отображения
        Debug.Log("Speed UP!");
    }

    // Метод для скрытия текста "Speed UP"
    void HideSpeedUpText()
    {
        speedUpText.gameObject.SetActive(false); // Отключаем текстовый элемент
    }



    void Awake()
    {
        MakeIntance();
    }

    void Start()
    {
        halfGroundSize = GameObject.Find("GroundBlock Main").GetComponent<GroundBlock>().halfLength;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseController>();
        StartCoroutine("GenerateObstacles");
        score_Text = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

    }


    IEnumerator GenerateObstacles()
    {
        // Рассчитываем базовый таймер
        float baseTimer = Random.Range(min_ObstacleDelay, max_ObstecalDelay) / playerController.speed.z;

        // Если скорость игры увеличена, уменьшаем время задержки между спавнами препятствий
        if (Time.timeScale > 1.4f)
        {
            baseTimer /= Time.timeScale;  // Уменьшаем задержку в зависимости от Time.timeScale
        }

        yield return new WaitForSeconds(baseTimer);

        // Создаём препятствия
        CreateObstacles(playerController.gameObject.transform.position.z + halfGroundSize);

        // Повторяем генерацию
        StartCoroutine(GenerateObstacles());
    }

    void CreateObstacles(float zPos)
    {
        int obstacleCount = 1;

        // Если скорость игры увеличена, увеличиваем количество препятствий
        if (Time.timeScale > 1.4f)
        {
            obstacleCount = Random.Range(1, 3);  // Создаём от 1 до 3 препятствий
        }

        for (int i = 0; i < obstacleCount; i++)
        {
            int r = Random.Range(0, 10);
            if (0 <= r && r < 7)
            {
                int obstacleLane = Random.Range(0, lanes.Length);
                AddObstacle(new Vector3(lanes[obstacleLane].transform.position.x, 0f, zPos), Random.Range(0, obstaclePrefabs.Length));

                int zombieLane = 0;
                if (obstacleLane == 0)
                {
                    zombieLane = Random.Range(0, 2) == 1 ? 1 : 2;
                }
                else if (obstacleLane == 1)
                {
                    zombieLane = Random.Range(0, 2) == 1 ? 0 : 2;
                }
                else if (obstacleLane == 2)
                {
                    zombieLane = Random.Range(0, 2) == 1 ? 1 : 0;
                }
                AddZombies(new Vector3(lanes[zombieLane].transform.position.x, 0.15f, zPos));
            }

            // Увеличиваем zPos для следующего препятствия, чтобы они не появлялись в одной точке
            zPos += 2.0f; // расстояние между препятствиями
        }
    }

    void MakeIntance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void AddObstacle(Vector3 position, int type)
    {
        GameObject obstacle = Instantiate(obstaclePrefabs[type], position, Quaternion.identity);
        bool mirror = Random.Range(0, 2) == 1;

        switch (type)
        {
            case 0:
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -20 : 20, 0f);
                break;
            case 1:
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -20 : 20, 0f);
                break;
            case 2:
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -1 : 1, 0f);
                break;
            case 3:
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -170 : 170, 0f);
                break;
        }
        obstacle.transform.position = position;
    }

    void AddZombies(Vector3 pos)
    {
        int count = Random.Range(0, 3) + 1;

        for (int i = 0; i < count; i++)
        {
            Vector3 shift = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(1f, 10f) * i);
            Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Length)], pos + shift * i, Quaternion.identity);
        }
    }

    public void IncreaseScore()
    {
        Kill_count++;
        score_Text.text = Kill_count.ToString();

        // Увеличиваем количество монет в глобальных переменных
        GlobalVariables.instance.AddMoney(1); // Каждое убийство добавляет 1 монету
    }

    public void PauseGame()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;

    }
    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameover_Panel.SetActive(true);
        Final_Score.text = "Killed: " + Kill_count.ToString() + "\nCoins: " + GlobalVariables.instance.Money.ToString();

        // Сохраняем прогресс при завершении игры
        GlobalVariables.instance.SaveData();

        // Увеличиваем и сохраняем gameover_count
        gameover_count = PlayerPrefs.GetInt("GameOverCount", 0); // Загружаем текущее значение
        gameover_count++;
        PlayerPrefs.SetInt("GameOverCount", gameover_count); // Сохраняем обновленное значение

        Debug.Log(gameover_count);

        if (gameover_count >= game_before_ADS)
        {
            Debug.Log("SHOW ADS");
            Ads.instance.ShowInterstitialAd();
            gameover_count = 0;
            PlayerPrefs.SetInt("GameOverCount", gameover_count); // Сбрасываем счетчик и сохраняем
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
        Ads.instance.LoadInterstitialAd();
    }


}
