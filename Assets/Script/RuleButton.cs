using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleButton : MonoBehaviour
{
    public GameObject[] pages;    // �y�[�W�� Inspector �Őݒ�
    private int currentPage = 0;
    [Header("���ʉ��ݒ�")]
    public AudioClip openSound;   // �J����
    public AudioClip closeSound;  // ���鉹
    public AudioClip pageSound;   // �߂��鉹

    private AudioSource audioSource;
    public GameObject backgroundParticle;
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void ShowRule()
    {
        gameObject.SetActive(true);
        currentPage = 0;
        UpdatePages();
        PlaySound(openSound);
        if (backgroundParticle != null)
            backgroundParticle.SetActive(false);
    }

    public void HideRule()
    {
        audioSource.PlayOneShot(closeSound);
        StartCoroutine(DisableAfterSound(closeSound.length));
    }

    private IEnumerator DisableAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); 
       
        if (backgroundParticle != null)
            backgroundParticle.SetActive(true);
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePages();
            PlaySound(pageSound);
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
            PlaySound(pageSound);
        }
    }

    private void UpdatePages()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
