using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using UnityEngine.SocialPlatforms;

public class ButtonPulse : MonoBehaviour
{
    public float pulseAmount = 0.1f; // Сколько будет двигаться кнопка
    public float pulseSpeed = 1.0f; // Скорость пульсации

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition; // Сохраняем начальное положение кнопки
        //Debug.Log("ButtonPulse script started."); // Сообщение для проверки
    }

    void Update()
    {
        // Пульсация кнопки
        float newY = originalPosition.y + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localPosition = new Vector3(originalPosition.x, newY, originalPosition.z);

        // Для отладки
        //Debug.Log("Current position: " + transform.localPosition); // Вывод текущей позиции
    }
}