using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToScreen : MonoBehaviour
{

    private PlantItem _plantItem;
    private DashboardPlant _dashboardPlant;
    private GameObject lastScreen;
    private GameObject actualScreen;
    [SerializeField] private GameObject navi;

    [SerializeField] private GameObject[] planes;

    public GameObject[] getPlanes()
    {
        return planes;
    }


    //Go to Plant State Screen
    public void GoToPlantScreen(Sprite icon, string nickname, string kind, PlantState state)
    {
        planes[1].SetActive(true); //Dashboard Plant
        planes[0].SetActive(false); //Main Dashboard
        _dashboardPlant = planes[1].GetComponent<DashboardPlant>();
        _dashboardPlant.setScreen(icon, nickname, kind, state);
    }

    //Go to Add Plant
    public void GoToAddPlant()
    {
        Navigation _navi = navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(planes[2]);
    }

    //Go to Plantpedia
    public void GoBack(GameObject lastScreen)
    {
        Navigation _navi = navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(lastScreen);
    }
}
