using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantpediaDetailUtility : MonoBehaviour
{
    private GameObject detailScreen;

    private GameObject plantpediaScreen;

    private Plant plant;
    
    public void GoToDetailScreen()
    {
        if (plant == null || plantpediaScreen == null || detailScreen == null) return;
        detailScreen.GetComponent<PlantpediaInfoUtility>().FillInfoScreen(plant);
        plantpediaScreen.SetActive(false);
        detailScreen.SetActive(true);
    }

    public void SetUpUtility(Plant plantData, GameObject mainScreen, GameObject infoScreen)
    {
        plant = plantData;
        plantpediaScreen = mainScreen;
        detailScreen = infoScreen;
    }
}
