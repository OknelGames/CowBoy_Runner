using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;


public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance;

    public int Money;
    public float reloadSpeedMultiplier;
    public float bestScore;  // Лучший результат



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Coins", Money);
        PlayerPrefs.SetFloat("ReloadSpeedMultiplier", reloadSpeedMultiplier);
        PlayerPrefs.SetFloat("BestScore", bestScore);  // Сохраняем лучший результат
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        Money = PlayerPrefs.GetInt("Coins", 100);
        reloadSpeedMultiplier = PlayerPrefs.GetFloat("ReloadSpeedMultiplier", 1.0f);
        bestScore = PlayerPrefs.GetFloat("BestScore", 0f);  // Загружаем лучший результат
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        SaveData();
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt("Coins", 50);
        PlayerPrefs.SetFloat("ReloadSpeedMultiplier", 1);
        PlayerPrefs.SetFloat("BestScore", 0);  // Сброс лучшего результата

        PlayerPrefs.Save();
        GlobalVariables.instance.LoadData();
        Debug.Log("Данные сброшены до значений по умолчанию.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
