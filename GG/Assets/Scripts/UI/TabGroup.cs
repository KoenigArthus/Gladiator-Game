using JSAM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButtonG> tabButtons;
    public TabButtonG selectedTab;
    public List<GameObject> objectsToSwap;
    public Button leftButton, rightButton;
    [Tooltip("This prevents you from closing the Tab by clicking on the Button you used to open again")]
    public bool thisGroupsTabsStayOpen;
    [Tooltip("Will load the selected Tab onEnable if there is one")]
    public bool LoadTabOnEnable;

    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabSelected;



    private void OnEnable()
    {
        if (LoadTabOnEnable)
        {
            if (selectedTab != null)
            {
                OnTabSelected(selectedTab);
            }
            else
            {
                Debug.Log("You are trying to Load a Tab OnEnable but selectedTab is null");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            GoLeft();

        if (Input.GetKeyDown(KeyCode.E))
            GoRight();
    }


    public void Subscribe(TabButtonG button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButtonG>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtonG button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButtonG button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtonG button)
    {
        if ((selectedTab == button) && !thisGroupsTabsStayOpen)
        {
            selectedTab = null;
            objectsToSwap[button.transform.GetSiblingIndex()].SetActive(false);
            selectedTab.Select();
        }
        else
        {
            if (selectedTab != null)
                selectedTab.Deselect();

            selectedTab = button;
            selectedTab.Select();
            ResetTabs();
            button.background.sprite = tabSelected;
            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    objectsToSwap[i].SetActive(true);
                }
                else
                {
                    objectsToSwap[i].SetActive(false);
                }
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButtonG button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
                continue;

            button.background.sprite = tabIdle;
        }
    }

    public void GoLeft()
    {
        if (selectedTab.transform.GetSiblingIndex() > 0)
            OnTabSelected(transform.GetChild(selectedTab.transform.GetSiblingIndex() - 1).GetComponent<TabButtonG>());
        else
            OnTabSelected(transform.GetChild(transform.childCount - 1).GetComponent<TabButtonG>());
    }

    public void GoRight()
    {
        if (selectedTab.transform.GetSiblingIndex() < transform.childCount - 1)
            OnTabSelected(transform.GetChild(selectedTab.transform.GetSiblingIndex() + 1).GetComponent<TabButtonG>());
        else
            OnTabSelected(transform.GetChild(0).GetComponent<TabButtonG>());
    }



}

