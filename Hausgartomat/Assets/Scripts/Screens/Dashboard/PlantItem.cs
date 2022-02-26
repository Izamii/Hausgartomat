using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
* This class contains the information of one plant that 
* Dashboard and Plant Information require to display it.
* */
public class PlantItem:MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private string nickname;
    [SerializeField] private string kind;
    [SerializeField] private PlantState plantState;
    [SerializeField] private GameObject manager;

    public PlantItem(Image icon, string nickname, string kind, PlantState plantState, GameObject manager)
    {
        this.icon = icon;
        this.nickname = nickname;
        this.kind = kind;
        this.plantState = plantState;
        this.manager = manager;
    }

    public string getNickname()
    {
        return nickname;
    }

    public void Go()
    {
        GoToScreen _goToScreen = manager.GetComponent<GoToScreen>();
        _goToScreen.GoToPlantScreen(icon, nickname, kind, plantState);
    }
}
