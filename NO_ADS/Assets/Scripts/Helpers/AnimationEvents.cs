using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AnimationEvents : MonoBehaviour
{

    private PlayerController playerController;
    private Animator anim;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        anim = GetComponent<Animator>();

    }



    public void ResetShooting()
    {
        playerController.canShot = true;
        anim.Play("idle");
        if (!playerController.canShot)
        {
            playerController.canShot = true;
        }
    }

    void CameraStartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
