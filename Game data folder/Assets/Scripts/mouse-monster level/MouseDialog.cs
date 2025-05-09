using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using TMPro;

public class MouseDialog : MonoBehaviour
{
    public GameObject player;
    public CanvasGroup talkHint;
    public Button talkButton;
    public CanvasGroup dialogBox;
    public TextMeshProUGUI dialogText;

    public GameObject eyes;
    public GameObject cheese1, cheese2, cheese3;
    public GameObject mouseNear1, mouseNear2, mouseNear3, mouseOnShoulder;

    public float fadeDuration = 0.3f;
    public float textFadeDuration = 0.4f;
    public float timeBetweenLines = 1.5f;

    public MonsterDialog monsterDialogRef;
    public static bool MouseOnShoulder = false;

    private bool isPlayerNearby = false;
    private bool isDialogPlaying = false;
    private bool sequenceStarted = false;

    private void Start()
    {
        SetInitialVisibility();

        talkHint.alpha = 0f;
        talkHint.interactable = false;
        talkHint.blocksRaycasts = false;

        talkButton.onClick.RemoveAllListeners();
        talkButton.onClick.AddListener(StartDialogPublic);

        dialogBox.alpha = 0f;
        dialogBox.interactable = false;
        dialogBox.blocksRaycasts = false;
    }

    private void SetInitialVisibility()
    {
        if (mouseNear1) SetCanvasGroup(mouseNear1, 0f, false);
        if (mouseNear2) SetCanvasGroup(mouseNear2, 0f, false);
        if (mouseNear3) SetCanvasGroup(mouseNear3, 0f, false);
        if (mouseOnShoulder) SetCanvasGroup(mouseOnShoulder, 0f, false);
    }

    private void SetCanvasGroup(GameObject obj, float alpha, bool active)
    {
        var group = obj.GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.alpha = alpha;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        obj.SetActive(active);
    }

    private void Update()
    {
        if (isPlayerNearby && !isDialogPlaying && Input.GetKeyDown(KeyCode.E))
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

        if (CheeseInteraction.CheesePiecesPlaced < 3)
        {
            await FadeText("Scariest mouse squeaking noises!");
            await Task.Delay(1200);
        }
        else if (!sequenceStarted)
        {
            sequenceStarted = true;
            await FadeText("...");
            await Task.Delay(300);
            await FadeCanvasGroup(dialogBox, 0f);
            dialogText.text = "";
            talkHint.gameObject.SetActive(false);
            dialogBox.gameObject.SetActive(false);
            StartCoroutine(MouseSequence());
        }

        await FadeCanvasGroup(dialogBox, 0f);
        dialogText.text = "";
        dialogText.alpha = 0f;
        isDialogPlaying = false;
    }

    private IEnumerator MouseSequence()
    {
        if (eyes != null) eyes.SetActive(false);
        yield return FadeHop(mouseNear1, cheese1);
        yield return FadeHop(mouseNear2, cheese2);
        yield return FadeHop(mouseNear3, cheese3);
        yield return FadeIn(mouseOnShoulder);

        MouseOnShoulder = true;
        if (monsterDialogRef != null)
        {
            monsterDialogRef.MouseOnShoulder = true;
        }
    }

    private IEnumerator FadeHop(GameObject mouse, GameObject cheese)
    {
        yield return FadeIn(mouse);
        if (cheese != null) cheese.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        yield return FadeOut(mouse);
    }

    private IEnumerator FadeIn(GameObject obj)
    {
        var group = obj.GetComponent<CanvasGroup>();
        obj.SetActive(true);
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(0f, 1f, t / 0.3f);
            yield return null;
        }
    }

    private IEnumerator FadeOut(GameObject obj)
    {
        var group = obj.GetComponent<CanvasGroup>();
        if (!obj.activeSelf) yield break;
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(1f, 0f, t / 0.3f);
            yield return null;
        }
        obj.SetActive(false);
    }

    private async Task FadeCanvasGroup(CanvasGroup group, float target)
    {
        float start = group.alpha;
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, target, t / 0.3f);
            await Task.Yield();
        }

        group.alpha = target;
        group.interactable = target > 0.9f;
        group.blocksRaycasts = target > 0.9f;
    }

    private async Task FadeText(string newText)
    {
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            dialogText.alpha = Mathf.Lerp(1f, 0f, t / 0.3f);
            await Task.Yield();
        }

        dialogText.text = newText;

        t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            dialogText.alpha = Mathf.Lerp(0f, 1f, t / 0.3f);
            await Task.Yield();
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
}
