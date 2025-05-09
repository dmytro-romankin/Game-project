using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class DoorIntercation : MonoBehaviour
{
    public GameObject player;
    public CanvasGroup interactHint;
    public SpriteRenderer doorLitSprite; 
    public string nextSceneName = "PuzzleMouse"; 

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

        if (doorLitSprite != null)
            doorLitSprite.enabled = false;
    }

    private void Update()
    {
        if (DialogTrigger.finalDialogFinished && !isDialogComplete)
        {
            isDialogComplete = true;
            if (doorLitSprite != null)
                doorLitSprite.enabled = true;
        }

        if (isPlayerNearby && isDialogComplete && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetString("LastScene", nextSceneName);
            PlayerPrefs.Save();
            SceneTransition.Instance.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && isDialogComplete)
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
}
