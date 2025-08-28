using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Pages (Assign in Inspector)")]
    public GameObject inventoryPage;
    public GameObject shopPage;
    public GameObject skillPage;
    public GameObject background;

    private Dictionary<string, GameObject> pages;

    void Awake()
    {
        pages = new Dictionary<string, GameObject>()
        {
            { "Inventory", inventoryPage },
            { "Shop", shopPage },
            { "Skill", skillPage }
        };

        foreach (var page in pages.Values)
        {
            if (page != null)
                page.SetActive(false);
        }
        background.SetActive(false);
    }

    public void ShowPage(string pageName)
    {
        background.SetActive(true);
        foreach (var page in pages.Values)
        {
            if (page != null)
                page.SetActive(false);
        }

        if (pages.ContainsKey(pageName) && pages[pageName] != null)
        {
            pages[pageName].SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Page {pageName} not found in UIManager!");
        }
    }

    public void ClosePage()
    {
        background.SetActive(false);
        foreach (var pages in pages.Values)
        {
            if (pages != null)
            {
                pages.SetActive(false);
            }
        }
    }
}
