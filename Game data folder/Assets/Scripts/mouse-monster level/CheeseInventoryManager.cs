using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class CheeseInventoryManager : MonoBehaviour
{
    public static CheeseInventoryManager Instance;
    public CanvasGroup[] cheeseIcons;
    private int cheeseCount = 0;

    private void Start()
    {
        foreach (var icon in cheeseIcons)
        {
            icon.alpha = 0f;
            icon.interactable = false;
            icon.blocksRaycasts = false;
            icon.gameObject.SetActive(false);
        }
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public async Task AnimateCheeseAtIndex(int index)
    {
        if (index >= cheeseIcons.Length) return;

        var icon = cheeseIcons[index];
        icon.gameObject.SetActive(true);
        await FadeCanvasGroup(icon, 1f, 0.4f);
    }

    public void SetCheeseCount(int value)
    {
        cheeseCount = Mathf.Clamp(value, 0, cheeseIcons.Length);
    }

    public async Task AddCheeseAsync()
    {
        if (cheeseCount >= cheeseIcons.Length) return;

        var icon = cheeseIcons[cheeseCount];
        icon.gameObject.SetActive(true);
        await FadeCanvasGroup(icon, 1f, 0.4f);
        cheeseCount++;
    }

    public async Task RemoveCheeseAsync()
    {
        if (cheeseCount <= 0) return;

        var icon = cheeseIcons[0]; 
        await FadeCanvasGroup(icon, 0f, 0.4f);
        icon.gameObject.SetActive(false);

        for (int i = 1; i < cheeseCount; i++)
        {
            cheeseIcons[i - 1] = cheeseIcons[i];
        }

        cheeseIcons[cheeseCount - 1] = icon;
        cheeseCount--;
    }

    private async Task FadeCanvasGroup(CanvasGroup group, float target, float duration)
    {
        float start = group.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, target, time / duration);
            await Task.Yield();
        }

        group.alpha = target;
    }

    public int GetCheeseCount() => cheeseCount;
}