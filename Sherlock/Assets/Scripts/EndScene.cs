using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class EndSceneUI : MonoBehaviour
{
    public TextMeshProUGUI runText;
    public Button restartButton;
    public Button exitButton;
    public float fadeDuration = 2f;

    private void Start()
    {
        restartButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        StartCoroutine(FadeTextToBlack());

        
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    IEnumerator FadeTextToBlack()
    {
        Color originalColor = runText.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            runText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        runText.gameObject.SetActive(false); 
        restartButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }
    public void RestartGame() 
    {
        SceneManager.LoadScene("StartScene");  
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

}
