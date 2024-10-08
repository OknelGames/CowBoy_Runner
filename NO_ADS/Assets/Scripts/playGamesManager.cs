using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using System;

public class playGamesManager : MonoBehaviour
{
    public TextMeshProUGUI info;
    public GameObject LoginControler;

    void Start()
    {
        SignIN();
    }
    public void SignIN()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();

            info.text = "Welcome \n" + name;

            StartCoroutine(WaitAndPrint());

        }
        else
        {
            info.text = "login Fail";
        }
    }

    IEnumerator WaitAndPrint()
    {
        // Приостановить выполнение на 2 секунды
        yield return new WaitForSeconds(4f);
        LoginControler.SetActive(false);

        // Этот код выполнится после задержки
        Debug.Log("Прошло 4 секунды");
    }

}

