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

        // �������� ������ � ���� (����� �������� ������� ���)
        gameObject.SetActive(false);

        // ���������� ������ � ��������� � ������� ���������
        if (InventoryManager.Instance != null)
            await InventoryManager.Instance.ShowStarAsync();
    }
}
