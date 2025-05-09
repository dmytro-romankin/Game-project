using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class CheeseInteraction : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer cheesePiece; 
    public CanvasGroup interactButton; 
    public int CheeseCount;

    [Header("Fade effect duration")]
    [Range(0, 1f)] public float duration = 0.25f;

    private bool isPlayerNearby = false;
    public static bool moonBiteTaken = false;
    public static int CheesePiecesPlaced = 0;

    
    private void Start()
    {
        cheesePiece.enabled = false;

        if (interactButton != null)
        {
            interactButton.alpha = 0f;
            interactButton.interactable = false;
            interactButton.blocksRaycasts = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && moonBiteTaken && CheesePiecesPlaced + 1 == CheeseCount)
        {
            isPlayerNearby = true;
            _ = FadeButtonAsync(1f); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player && moonBiteTaken && CheesePiecesPlaced + 1 == CheeseCount)
        {
            isPlayerNearby = false;
            _ = FadeButtonAsync(0f);
        }
    }

    private async void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) &&
            moonBiteTaken && CheesePiecesPlaced + 1 == CheeseCount)
        {
            cheesePiece.enabled = true; 
            CheesePiecesPlaced++;

            await CheeseInventoryManager.Instance.RemoveCheeseAsync();

            await FadeButtonAsync(0f);
            Destroy(interactButton.gameObject);
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

        bool visible = targetAlpha > 0.95f;
        interactButton.interactable = visible;
        interactButton.blocksRaycasts = visible;
    }
}
