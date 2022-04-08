using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * <summary>Utility class to handle the transfers from various screens to the Plantpedia Detail screen.</summary>
 * 
 */
public class PlantpediaDetailUtility : MonoBehaviour
{
    private GameObject detailScreen;

    private GameObject plantpediaScreen;

    private Plant plant;

    private GetPlantData _getPlantData;
    /**
     * <summary>Method to swap the current screen with the Plantpedia Detail screen. Only swaps views if utility has been initially set up.</summary>
     */
    public void GoToDetailScreen()
    {
        if (plant == null || plantpediaScreen == null || detailScreen == null) return;
        detailScreen.GetComponent<PlantpediaInfoUtility>().FillInfoScreen(plant);
        plantpediaScreen.SetActive(false);
        detailScreen.SetActive(true);
        detailScreen.GetComponent<PlantpediaInfoUtility>().LastScreen = plantpediaScreen;
    }
    /**
     * <summary>Sets up the utility functionality through a given <c>Plant</c> object.</summary>
     * <param name="plantData">Plant data as an instance of <c>Plant</c></param>
     * <param name="mainScreen">The screen that the utility is triggered from. (Used to set up the returnButton in the detail view.)</param>
     * <param name="infoScreen">Plantpedia Detail screen that is activated by the utility.</param>
     */
    public void SetUpUtility(Plant plantData, GameObject mainScreen, GameObject infoScreen)
    {
        plant = plantData;
        plantpediaScreen = mainScreen;
        detailScreen = infoScreen;
    }
    /**
     * <summary>Sets up the utility functionality by getting the plant data from the Firebase GameObject through a given plant name.</summary>
     * <param name="kind">Plant name as a string in order to retrieve plant data</param>
     * <param name="mainScreen">The screen that the utility is triggered from. (Used to set up the returnButton in the detail view.)</param>
     * <param name="infoScreen">Plantpedia Detail screen that is activated by the utility.</param>
     */
    public void SetUpUtility(string kind, GameObject mainScreen, GameObject infoScreen)
    {
        _getPlantData = GameObject.Find("Firebase").GetComponent<GetPlantData>(); ;
        plant = _getPlantData.GETSinglePlant(kind);
        plantpediaScreen = mainScreen;
        detailScreen = infoScreen;
    }
}
