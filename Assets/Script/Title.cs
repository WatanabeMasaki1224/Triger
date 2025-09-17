using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Image fadeImage;          // Canvas�ɒu�������i�܂��͐F�t���jImage
    public float fadeDuration = 1f;  // �t�F�[�h�ɂ����鎞��
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
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);  // ���� �� �s����
            fadeImage.color = c;
            yield return null;
        }

        // �t�F�[�h������ɃV�[���؂�ւ�
        SceneManager.LoadScene("Stage1");
    }
}
