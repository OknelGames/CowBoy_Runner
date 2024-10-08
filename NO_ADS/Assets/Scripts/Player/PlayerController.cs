using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : BaseController
{
    private Rigidbody myBody;

    public Transform bullet_StartPoint;
    public GameObject bullet_Prefab;
    public ParticleSystem shotFX;

    private Animator shootSliderAnim;

    private bool isSwipingLeft = false;
    private bool isSwipingRight = false;
    private Vector2 swipeVelocity;
    private float swipeStartTime;
    //private float maxSwipeTime = 0.5f; // Максимальное время для полного свайпа




    [HideInInspector]
    public bool canShot;

    public float reloadSpeedMultiplier = 5.0f;  // Базовый множитель скорости перезарядки (1.0 = нормальная скорость)



    void Start()
    {
        myBody = GetComponent<Rigidbody>();

        shootSliderAnim = GameObject.Find("Fire Bar").GetComponent<Animator>();

        GameObject.Find("Shoot_BTN").GetComponent<Button>().onClick.AddListener(ShootingControl);

        canShot = true;

    }

    // Update is called once per frame
    void Update()
    {

        //ControlMovementWithKeyboard();
        HandleTouchInput();
        ChangeRotation();

    }

    void FixedUpdate()
    {
        MoveTank();
    }

    void MoveTank()
    {
        myBody.MovePosition(myBody.position + speed * Time.deltaTime);
    }

    void HandleTouchInput()
    {
        // Эмуляция сенсорного ввода с помощью мыши в редакторе Unity
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            touchEndPos = Input.mousePosition;
            Vector2 swipeDelta = (Vector2)touchEndPos - (Vector2)touchStartPos;

            if (swipeDelta.magnitude > minSwipeDistance)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    if (swipeDelta.x > 0)
                    {
                        isSwipingRight = true;
                        isSwipingLeft = false;
                    }
                    else
                    {
                        isSwipingLeft = true;
                        isSwipingRight = false;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Когда пользователь отпускает кнопку мыши, останавливаем движение
            isSwipingLeft = false;
            isSwipingRight = false;
            MoveStraight();
        }
#else
    // Оригинальный сенсорный ввод для мобильных устройств
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStartPos = touch.position;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            touchEndPos = touch.position;
            Vector2 swipeDelta = touchEndPos - touchStartPos;

            if (swipeDelta.magnitude > minSwipeDistance)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    if (swipeDelta.x > 0)
                    {
                        isSwipingRight = true;
                        isSwipingLeft = false;
                    }
                    else
                    {
                        isSwipingLeft = true;
                        isSwipingRight = false;
                    }
                }
            }
        }

        if (touch.phase == TouchPhase.Ended)
        {
            // Когда пользователь отпускает палец, останавливаем движение
            isSwipingLeft = false;
            isSwipingRight = false;
            MoveStraight();
        }
    }
#endif

        // Обрабатываем движение в зависимости от флагов
        if (isSwipingLeft)
        {
            MoveLeft();
        }
        else if (isSwipingRight)
        {
            MoveRight();
        }
    }



    //     void ControlMovementWithKeyboard()
    // {
    //     if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
    //     {
    //         MoveLeft();
    //     }
    //     if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
    //     {
    //         MoveRight();
    //     }
    //     if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
    //     {
    //         MoveFast();
    //     }

    //     if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
    //     {
    //         MoveSlow();
    //     }
    //     if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
    //     {
    //         MoveStraight();
    //     }
    //     if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
    //     {
    //         MoveStraight();
    //     }
    //     if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
    //     {
    //         MoveNormal();
    //     }
    //     if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
    //     {
    //         MoveNormal();
    //     }
    // }

    void ChangeRotation()
    {
        if (speed.x > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, maxAngel, 0f), Time.deltaTime * rotationSpeed);
        }
        else if (speed.x < 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, -maxAngel, 0f), Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * rotationSpeed);
        }


    }

    public void ShootingControl()
    {
        if (Time.timeScale != 0)
        {
            if (canShot)
            {
                GameObject bullet = Instantiate(bullet_Prefab, bullet_StartPoint.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Move(2000f);
                shotFX.Play();

                canShot = false;

                // Устанавливаем скорость анимации с учётом глобального множителя
                shootSliderAnim.speed = GlobalVariables.instance.reloadSpeedMultiplier;
                shootSliderAnim.Play("FadeIN");
            }
            else
            {
                // Устанавливаем скорость анимации с учётом глобального множителя
                shootSliderAnim.speed = GlobalVariables.instance.reloadSpeedMultiplier;
                shootSliderAnim.Play("FadeIN");
            }
        }
    }
}
