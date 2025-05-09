using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class StarPickup : MonoBehaviour
{
    private bool pickedUp = false;

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUp || !collision.CompareTag("Player"))
            return;

        pickedUp = true;

        // Скрываем звезду с неба (можно анимацию сделать тут)
        gameObject.SetActive(false);

        // Показываем звезду в инвентаре с плавной анимацией
        if (InventoryManager.Instance != null)
            await InventoryManager.Instance.ShowStarAsync();
    }
}
