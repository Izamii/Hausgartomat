using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToScreen : MonoBehaviour
{

    private PlantItem _plantItem;


    //Go to Plant State Screen
    public void GoTo(GameObject plant)
    {
        GameObject.Find("Dashboard_Plant").SetActive(true);
        GameObject.Find("Dashboard_Main").SetActive(false);
        _plantItem = plant.GetComponent<PlantItem>();

    }
    //Go to Plantpedia
    public void GoTo(string plantKind)
    {

    }
}
