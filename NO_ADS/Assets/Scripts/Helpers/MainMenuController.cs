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


public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;
    public Animator cameraAnim;

    public TextMeshProUGUI Text_Coins;

    public TextMeshProUGUI Best_Scoore;
    private int all_Coins;

    public GameObject Menu;

    public GameObject Shop;

    public GameObject LeaderBoard;

    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }



    public void Update()
    {
        int all_Coins = GlobalVariables.instance.Money;
        Text_Coins.text = all_Coins.ToString();
        Best_Scoore.text = "Best Score: " + "\n" + ((int)PlayerPrefs.GetFloat("BestScore", 0.0f)).ToString();

    }


    public void PlayGames()
    {
        cameraAnim.Play("CameraSlider");
        Menu.gameObject.SetActive(false);

    }

    public void ShopOpen()
    {
        Shop.SetActive(true);
        Menu.SetActive(false);
    }

    public void ShopClose()
    {
        Shop.SetActive(false);
        Menu.SetActive(true);
    }

    public void OpenLeaderBoard()
    {
        LeaderBoard.SetActive(true);
    }
    public void ClosedLeaderBoard()
    {
        LeaderBoard.SetActive(false);

    }


    // show achievements UI



}
