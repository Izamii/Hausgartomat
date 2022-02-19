using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Button[] menu;
    private Color pressed = new Color32(176,209,169,255);
    public void NavigationBarClick(GameObject activePanel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);

        }
        activePanel.SetActive(true);
        foreach (Button btn in menu)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        switch (activePanel.name.Split('_')[0])
        {
            case "Dashboard":
                SetButtonPressed(menu[0]);
                break;
            case "Planen":
                SetButtonPressed(menu[1]);
                break;
            case "Plantpedia":
                SetButtonPressed(menu[2]);
                break;
            case "Einstellungen":
                SetButtonPressed(menu[3]);
                break;
            default:
                break;
        }
    }

    public void SetButtonPressed(Button button)
    {
        button.GetComponent<Image>().color = pressed;
    }


}