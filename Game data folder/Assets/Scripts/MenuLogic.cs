using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("LastScene"))
        {
            continueButton.interactable = false;
        }
    }
    public void NewGame()
    {
        PlayerPrefs.DeleteKey("LastScene");
        SceneTransition.Instance.LoadScene("PuzzleBird");
    }

    public void ContinueGame()
    {
        string sceneName = PlayerPrefs.GetString("LastScene", "PuzzleBird");
        SceneTransition.Instance.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}