using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNo)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            //Greying all unselected tabs
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }

        //Highlighting selected tab
        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;        
    }
}

// Code references
// 1)Menu UI with Tab Switching - Top Down Unity 2D #6
//  Author: Game Code Library
//  Date accessed:  17/10/2025
//  Availability: https://www.youtube.com/watch?v=liba3xGI4gM