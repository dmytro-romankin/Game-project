using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (fadePanel != null)
        {
            fadePanel.alpha = 1f;
            fadePanel.interactable = false;
            fadePanel.blocksRaycasts = true;
            StartCoroutine(Fade(0f));
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndSwitchScene(sceneName));
    }

    private IEnumerator FadeAndSwitchScene(string sceneName)
    {
        if (fadePanel == null)
        {
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        yield return Fade(1f);
        bool sceneLoaded = false;
        SceneManager.sceneLoaded += (scene, mode) => sceneLoaded = true;
        SceneManager.LoadScene(sceneName);
        while (!sceneLoaded) yield return null;
        yield return Fade(0f);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadePanel.alpha;
        float time = 0f;

        fadePanel.gameObject.SetActive(true);

        fadePanel.blocksRaycasts = true;
        fadePanel.interactable = true;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = targetAlpha;

        // Ѕлокируем взаимодействие, если стало прозрачным
        bool visible = targetAlpha > 0.95f;
        fadePanel.blocksRaycasts = visible;
        fadePanel.interactable = visible;

        // ≈сли полностью прозрачна€ панель Ч можно скрыть
        if (!visible)
            fadePanel.gameObject.SetActive(false);
    }
}
