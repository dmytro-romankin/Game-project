using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class MoonInterraction : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer moonWhole;
    public SpriteRenderer moonBite;
    public CanvasGroup interactButton;
    public DialogTrigger MonsterDialog;

    [Header("Fade effect duration")]
    [Range(0, 1f)] public float duration = 0.25f;

    private bool isPlayerNearby = false;
    private bool hasBitten = false;

    private void Start()
    {
        moonBite.enabled = false;

        if (interactButton != null)
        {
            interactButton.alpha = 0f;
            interactButton.interactable = false;
            interactButton.blocksRaycasts = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && !hasBitten)
        {
            isPlayerNearby = true;
            _ = FadeButtonAsync(1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player && !hasBitten)
        {
            isPlayerNearby = false;
            _ = FadeButtonAsync(0f);
        }
    }

    private async void Update()
    {
        if (isPlayerNearby && !hasBitten && Input.GetKeyDown(KeyCode.E))
        {
            hasBitten = true;
            moonBite.enabled = true;
            CheeseInteraction.moonBiteTaken = true;

            await FadeButtonAsync(0f);
            Destroy(interactButton.gameObject);

            await SaveMoonStateAsync();
            var tasks = new Task[]
            {
                CheeseInventoryManager.Instance.AnimateCheeseAtIndex(0),
                CheeseInventoryManager.Instance.AnimateCheeseAtIndex(1),
                CheeseInventoryManager.Instance.AnimateCheeseAtIndex(2)
            };

            await Task.WhenAll(tasks);

            CheeseInventoryManager.Instance.SetCheeseCount(3);
        }
    }

    private async Task FadeButtonAsync(float targetAlpha)
    {
        if (interactButton == null) return;

        float startAlpha = interactButton.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            interactButton.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            await Task.Yield();
        }

        interactButton.alpha = targetAlpha;
        bool isVisible = targetAlpha > 0.95f;
        interactButton.interactable = isVisible;
        interactButton.blocksRaycasts = isVisible;
    }

    private async Task SaveMoonStateAsync()
    {
        string path = Path.Combine(Application.persistentDataPath, "moon_state.txt");
        try
        {
            await File.WriteAllTextAsync(path, "MoonBitten");
            Debug.Log("Moon state saved.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving moon state: " + e.Message);
        }
    }
}