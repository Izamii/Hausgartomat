using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToScreen : MonoBehaviour
{
    private DashboardPlant _dashboardPlant;
    [SerializeField] private GameObject navi;
    [SerializeField] private GameObject[] planes;

    public GameObject Navi { get => navi; set => navi = value; }


    //Go to Plant State Screen
    public void GoToPlantScreen(Sprite icon, string nickname, string kind, PlantState state)
    {
        Navigation _navi = Navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(planes[1]);
        _dashboardPlant = planes[1].GetComponent<DashboardPlant>();
        _dashboardPlant.SetScreen(icon, nickname, kind, state);
    }

    //Go to Add Plant
    public void GoToAddPlant()
    {
        Navigation _navi = Navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(planes[2]);
    }


    public void GoBack(GameObject lastScreen)
    {
        Navigation _navi = Navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(lastScreen);
    }

    public void GoBackAddPlant(GameObject lastScreen, GameObject thisScreen)
    {
        thisScreen.SetActive(false);
        lastScreen.SetActive(true);
    }
}
