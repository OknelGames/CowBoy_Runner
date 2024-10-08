using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFolow : MonoBehaviour
{
    public Transform target;
    public float distance = 6.3f;
    public float height = 3.5f;
    public float height_Damping = 3.25f;
    public float rotation_Damping = 0.27f;

    public float targetAspect = 1280.0f / 720.0f; // Целевое соотношение сторон (например, 16:9)




    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        Screen.SetResolution(1280, 720, true); // Принудительно устанавливаем разрешение 1280x720


        // // Текущее соотношение сторон экрана
        // float windowAspect = (float)Screen.width / (float)Screen.height;

        // // Соотношение сторон для сравнения
        // float scaleHeight = windowAspect / targetAspect;

        // Camera camera = GetComponent<Camera>();

        // if (scaleHeight < 1.0f)
        // {
        //     // Добавляем полосы сверху и снизу
        //     Rect rect = camera.rect;
        //     rect.width = 1.0f;
        //     rect.height = scaleHeight;
        //     rect.x = 0;
        //     rect.y = (1.0f - scaleHeight) / 2.0f;

        //     camera.rect = rect;
        // }
        // else
        // {
        //     // Добавляем полосы по бокам
        //     float scaleWidth = 1.0f / scaleHeight;
        //     Rect rect = camera.rect;
        //     rect.width = scaleWidth;
        //     rect.height = 1.0f;
        //     rect.x = (1.0f - scaleWidth) / 2.0f;
        //     rect.y = 0;

        //     camera.rect = rect;
        // }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        float wanted_Rotation_Angle = target.eulerAngles.y;
        float wanted_Height = target.position.y + height;

        float current_Rotation_Angle = transform.eulerAngles.y;
        float current_Height = transform.position.y;

        current_Rotation_Angle = Mathf.LerpAngle(current_Rotation_Angle, wanted_Rotation_Angle, rotation_Damping * Time.deltaTime);
        current_Height = Mathf.Lerp(current_Height, wanted_Height, height_Damping * Time.deltaTime);

        Quaternion current_Rotation = Quaternion.Euler(0f, current_Rotation_Angle, 0f);
        transform.position = target.position;
        transform.position -= current_Rotation * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x, current_Height, transform.position.z);
    }
}
