using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * This class contains the information of one plant that 
 * Dashboard and Plant Information require to display it.
 * */
public class PlantItem:MonoBehaviour
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string nickname;
    [SerializeField] private string kind;
    [SerializeField] private PlantState plantState;
    [SerializeField] private GameObject manager;

    public string getNickname()
    {
        return nickname;
    }

    public void Go()
    {
        GoToScreen _goToScreen = manager.GetComponent<GoToScreen>();
        _goToScreen.GoTo(icon, nickname, kind, plantState);
    }
}
