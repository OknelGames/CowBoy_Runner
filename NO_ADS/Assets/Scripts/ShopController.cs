using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public TextMeshProUGUI coinsText;  // Текстовое поле для отображения количества монет
    public int reloadSpeedCost = 100;  // Стоимость ускоренной перезарядки
    public float reloadSpeedIncrease = 0.1f;  // Величина, на которую ускоряется перезарядка
    public TextMeshProUGUI Speed_cost_text;
    public TextMeshProUGUI Speed_curent_text;


    void Start()
    {
        UpdateCoinsDisplay();

        Speed_cost_text.text = "Cost" + "\n" + reloadSpeedCost.ToString();
        Speed_curent_text.text = "Speed reload" + "\n" + GlobalVariables.instance.reloadSpeedMultiplier.ToString();

    }

    void Update()
    {
        UpdateCoinsDisplay();
    }

    // Метод для обновления отображаемого количества монет
    void UpdateCoinsDisplay()
    {
        coinsText.text = GlobalVariables.instance.Money.ToString();
        Speed_cost_text.text = "Cost" + "\n" + reloadSpeedCost.ToString();
        Speed_curent_text.text = "Speed reload" + "\n" + GlobalVariables.instance.reloadSpeedMultiplier.ToString("F1");
    }

    // Метод для покупки здоровья


    // Метод для покупки ускоренной перезарядки
    public void BuyReloadSpeed()
    {
        if (GlobalVariables.instance.Money >= reloadSpeedCost)
        {
            GlobalVariables.instance.Money -= reloadSpeedCost;
            GlobalVariables.instance.reloadSpeedMultiplier += reloadSpeedIncrease;  // Уменьшаем множитель для ускорения перезарядки

            //UpdateCoinsDisplay();
            GlobalVariables.instance.SaveData();  // Сохраняем изменения
            Debug.Log("Ускоренная перезарядка куплена! Текущий множитель скорости перезарядки: " + GlobalVariables.instance.reloadSpeedMultiplier);
        }
        else
        {
            Debug.Log("Недостаточно монет для покупки ускоренной перезарядки!");
        }
    }

    public void Money_ADS()
    {
        Ads.instance.ShowRewardedAd();



    }
}
