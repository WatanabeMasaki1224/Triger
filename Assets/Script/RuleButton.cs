using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleButton : MonoBehaviour
{
    public GameObject[] pages;    // ÉyÅ[ÉWÇ Inspector Ç≈ê›íË
    private int currentPage = 0;

    public void ShowRule()
    {
        gameObject.SetActive(true);
        currentPage = 0;
        UpdatePages();
    }

    public void HideRule()
    {
        gameObject.SetActive(false);
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePages();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
        }
    }

    private void UpdatePages()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }
    }
}
