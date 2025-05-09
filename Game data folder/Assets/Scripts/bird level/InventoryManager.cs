using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Image starIcon;
    private CanvasGroup starCanvasGroup;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (starIcon != null)
        {
            starCanvasGroup = starIcon.GetComponent<CanvasGroup>();
            if (starCanvasGroup == null)
                starCanvasGroup = starIcon.gameObject.AddComponent<CanvasGroup>();

            starCanvasGroup.alpha = 0f; 
        }
    }

    public async Task ShowStarAsync()
    {
        if (starCanvasGroup == null) return;

        starIcon.gameObject.SetActive(true);
        await FadeAsync(starCanvasGroup, 1f, 0.5f);
    }

    public async Task HideStarAsync()
    {
        if (starCanvasGroup == null) return;

        await FadeAsync(starCanvasGroup, 0f, 0.5f);
        starIcon.gameObject.SetActive(false);
    }

    private async Task FadeAsync(CanvasGroup group, float targetAlpha, float duration)
    {
        float startAlpha = group.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            await Task.Yield(); 
        }
        group.alpha = targetAlpha;
    }
}
