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
    [SerializeField] private Sprite icon;
    [SerializeField] private string nickname;
    [SerializeField] private string kind;
    //[SerializeField] private PlantState plantState;
    [SerializeField] private GameObject manager;
    private PlantState test;

    public Sprite Icon { get => icon; set => icon = value; }
    public string Nickname { get => nickname; set => nickname = value; }
    public string Kind { get => kind; set => kind = value; }
    //public PlantState PlantState { get => plantState; set => plantState = value; }
    public GameObject Manager { get => manager; set => manager = value; }

    public PlantItem(Sprite icon, string nickname, string kind, PlantState plantState)
    {
        this.Icon = icon;
        this.Nickname = nickname;
        this.Kind = kind;
        //this.PlantState = plantState;
        this.Manager = GameObject.Find("Manager");
        Debug.Log(plantState);
    }
    public PlantItem(Sprite icon, string nickname, string kind)
    {
        this.Icon = icon;
        this.Nickname = nickname;
        this.Kind = kind;
        //this.PlantState = new PlantState(kind);
        this.Manager = GameObject.Find("Manager");
        //test = gameObject.AddComponent<PlantState>();
    }
    public void OpenPlantScreen()
    {
        GoToScreen _goToScreen = this.Manager.GetComponent<GoToScreen>();
        _goToScreen.GoToPlantScreen(Icon, Nickname, Kind, gameObject.GetComponent<PlantState>());
    }
}
