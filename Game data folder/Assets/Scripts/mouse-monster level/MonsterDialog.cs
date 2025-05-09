using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using TMPro;

public class MonsterDialog : MonoBehaviour
{
    public GameObject player;
    public CanvasGroup talkHint;
    public Button talkButton;
    public CanvasGroup dialogBox;
    public TextMeshProUGUI dialogText;
    public SpriteRenderer doorLight;

    public static bool finalDialogFinished = false;

    [Header("Dialogs")]
    [TextArea] public string[] scaredLines;
    [TextArea] public string[] afterMoonBite;
    [TextArea] public string[] finalLines;

    public float fadeDuration = 0.3f;
    public float textFadeDuration = 0.4f;
    public float timeBetweenLines = 1.5f;

    public bool MouseOnShoulder = false;

    private bool isPlayerNearby = false;
    private bool isDialogPlaying = false;

    private void Start()
    {
        talkHint.alpha = 0f;
        talkHint.interactable = false;
        talkHint.blocksRaycasts = false;

        talkButton.onClick.RemoveAllListeners();
        talkButton.onClick.AddListener(StartDialogPublic);

        dialogBox.alpha = 0f;
        dialogBox.interactable = false;
        dialogBox.blocksRaycasts = false;

        if (doorLight != null)
            doorLight.enabled = false;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isDialogPlaying)
        {
            StartDialogPublic();
        }
    }

    public void StartDialogPublic()
    {
        _ = StartDialog();
    }

    private async Task StartDialog()
    {
        if (isDialogPlaying) return;

        isDialogPlaying = true;
        _ = FadeCanvasGroup(talkHint, 0f);
        await FadeCanvasGroup(dialogBox, 1f);

        string[] linesToShow = scaredLines;

        if (!MouseOnShoulder)
        {
            if (CheeseInteraction.moonBiteTaken)
                linesToShow = afterMoonBite;
            else
                linesToShow = scaredLines;
        }
        else
        {
            linesToShow = finalLines;
        }

        if (MouseDialog.MouseOnShoulder)
        {
            linesToShow = finalLines;
        }

        foreach (string line in linesToShow)
        {
            await FadeText(line);
            await Task.Delay((int)(timeBetweenLines * 1000));
        }

        await FadeCanvasGroup(dialogBox, 0f);
        dialogText.text = "";
        dialogText.alpha = 0f;
        isDialogPlaying = false;

        if (CheeseInteraction.moonBiteTaken && MouseDialog.MouseOnShoulder && doorLight != null)
        {
            doorLight.enabled = true;
            finalDialogFinished = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && !isDialogPlaying)
        {
            isPlayerNearby = true;
            _ = FadeCanvasGroup(talkHint, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerNearby = false;
            _ = FadeCanvasGroup(talkHint, 0f);
        }
    }

    private async Task FadeCanvasGroup(CanvasGroup group, float target)
    {
        float start = group.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            await Task.Yield();
        }

        group.alpha = target;
        group.interactable = target > 0.9f;
        group.blocksRaycasts = target > 0.9f;
    }

    private async Task FadeText(string newText)
    {
        float t = 0f;

        while (t < textFadeDuration)
        {
            t += Time.deltaTime;
            dialogText.alpha = Mathf.Lerp(1f, 0f, t / textFadeDuration);
            await Task.Yield();
        }

        dialogText.text = newText;

        t = 0f;
        while (t < textFadeDuration)
        {
            t += Time.deltaTime;
            dialogText.alpha = Mathf.Lerp(0f, 1f, t / textFadeDuration);
            await Task.Yield();
        }
    }
}
