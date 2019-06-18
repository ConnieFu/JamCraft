using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_BaseMenuUI;
    [SerializeField] private GameObject m_GameplayUI;

    [SerializeField] private List<GameObject> m_Menus;
    
    public void ToggleMenuUI(bool on)
    {
        m_BaseMenuUI.SetActive(on);
        m_GameplayUI.SetActive(!on);
    }

    public void ShowMenu(string menuName)
    {
        for (int i = 0; i < m_Menus.Count; i++)
        {
            if (string.Compare(m_Menus[i].name, menuName) == 0)
            {
                m_Menus[i].SetActive(true);
            }
            else
            {
                m_Menus[i].SetActive(false);
            }
        }
    }

    public void HideMenu(string menuName)
    {
        for (int i = 0; i < m_Menus.Count; i++)
        {
            if (string.Compare(m_Menus[i].name, menuName) == 0)
            {
                m_Menus[i].SetActive(false);
            }
        }
    }
}
