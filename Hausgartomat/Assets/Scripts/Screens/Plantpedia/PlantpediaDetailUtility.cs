using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * 
 * 
 */
public class PlantpediaDetailUtility : MonoBehaviour
{
    private GameObject detailScreen;

    private GameObject plantpediaScreen;

    private Plant plant;

    private GetPlantData _getPlantData;

    public void GoToDetailScreen()
    {
        if (plant == null || plantpediaScreen == null || detailScreen == null) return;
        detailScreen.GetComponent<PlantpediaInfoUtility>().FillInfoScreen(plant);
        plantpediaScreen.SetActive(false);
        detailScreen.SetActive(true);
        detailScreen.GetComponent<PlantpediaInfoUtility>().LastScreen = plantpediaScreen;
    }

    public void SetUpUtility(Plant plantData, GameObject mainScreen, GameObject infoScreen)
    {
        plant = plantData;
        plantpediaScreen = mainScreen;
        detailScreen = infoScreen;
    }
    public void SetUpUtility(string kind, GameObject mainScreen, GameObject infoScreen)
    {
        _getPlantData = GameObject.Find("Firebase").GetComponent<GetPlantData>(); ;
        plant = _getPlantData.GETSinglePlant(kind);
        plantpediaScreen = mainScreen;
        detailScreen = infoScreen;
    }
}
