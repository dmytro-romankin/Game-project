using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class LeftLampInterraction : MonoBehaviour
{
    public GameObject player; 
    public SpriteRenderer LeftLamp; 
    public CanvasGroup interactButton;
    public DialogTrigger crowDialog;

    [Header("Fade effect duration")]
    [Range(0, 1f)] public float duration = 0.25f;

    private bool isPlayerNearby = false;
    private bool hasInteracted = false;

    private void Start()
    {
        LeftLamp.enabled = true;

        if (interactButton != null)
        {
            interactButton.alpha = 0f;
            interactButton.interactable = false;
            interactButton.blocksRaycasts = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && !hasInteracted)
        {
            isPlayerNearby = true;
            StartCoroutine(FadeInButton());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerNearby = false;
            StartCoroutine(FadeOutButton());
        }
    }

    private IEnumerator FadeInButton()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            interactButton.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        interactButton.interactable = true;
        interactButton.blocksRaycasts = true;
    }

    private IEnumerator FadeOutButton(bool destroy = false)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            interactButton.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }
        interactButton.interactable = false;
        interactButton.blocksRaycasts = false;

        if (destroy)
        {
            Destroy(interactButton.gameObject);
        }
    }

    private void Update()
    {
        if (isPlayerNearby && !hasInteracted && Input.GetKeyDown(KeyCode.E))
        {
            hasInteracted = true;
            LeftLamp.enabled = false;

            if (crowDialog != null)
            {
                crowDialog.lampOff = true;
            }

            StartCoroutine(FadeOutButton(true));
            SaveLampStateAsync();
        }
    }

    private async void SaveLampStateAsync()
    {
        string path = Path.Combine(Application.persistentDataPath, "left_lamp_state.txt");
        try
        {
            await File.WriteAllTextAsync(path, "LeftLampOff");
            Debug.Log("Left lamp state saved!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save left lamp state: " + e.Message);
        }
    }
}