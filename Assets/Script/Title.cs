using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Image fadeImage;          // Canvasに置いた黒（または色付き）Image
    public float fadeDuration = 1f;  // フェードにかける時間
    public AudioSource audioSource;
    public AudioClip clip;

    public void OnClickStart()
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);  // 透明 → 不透明
            fadeImage.color = c;
            yield return null;
        }

        // フェード完了後にシーン切り替え
        SceneManager.LoadScene("Stage1");
    }
}
