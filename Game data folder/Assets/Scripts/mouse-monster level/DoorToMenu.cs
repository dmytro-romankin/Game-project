using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class DoorToMenu : MonoBehaviour
{
    public GameObject player;
    public CanvasGroup interactHint;
    public CanvasGroup fadePanel;
    public SpriteRenderer doorLight;
    public string nextSceneName = "Menu";

    private bool isPlayerNearby = false;
    private bool isDialogComplete = false;

    private void Start()
    {
        if (interactHint != null)
        {
            interactHint.alpha = 0f;
            interactHint.interactable = false;
            interactHint.blocksRaycasts = false;
        }

        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
            fadePanel.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (MouseDialog.MouseOnShoulder && !isDialogComplete)
        {
            isDialogComplete = true;
        }

        if (isPlayerNearby && isDialogComplete && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && MouseDialog.MouseOnShoulder && doorLight.enabled)
        {
            isPlayerNearby = true;
            ShowHint(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerNearby = false;
            ShowHint(false);
        }
    }

    private void ShowHint(bool show)
    {
        if (interactHint != null)
        {
            interactHint.alpha = show ? 1f : 0f;
            interactHint.interactable = show;
            interactHint.blocksRaycasts = show;
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (fadePanel == null) yield break;

        fadePanel.gameObject.SetActive(true);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, t / 1f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        PlayerPrefs.SetString("LastScene", nextSceneName);
        PlayerPrefs.Save();
        SceneTransition.Instance.LoadScene("Menu");
    }
}
