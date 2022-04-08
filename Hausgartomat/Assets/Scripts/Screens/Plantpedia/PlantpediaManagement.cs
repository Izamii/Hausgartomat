using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
/**
 * <summary>Management class for the Plantpedia screen, used to populate the list of plants in the Plantpedia view</summary>
 */
public class PlantpediaManagement : MonoBehaviour
{
    [SerializeField] private GameObject drawer;

    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private GameObject plantpediaMainScreen;
    
    [SerializeField] private GameObject planpediaDetailScreen;

    private GetPlantData database;
    
    void Start()
    {
        database = GameObject.Find("Firebase").GetComponent<GetPlantData>();
        StartCoroutine(GETData());
    }
    /**
     * <summary>Populates the view of the Plantpedia screen with GameObjects that represent a single plant each.
     * By using the read database information, and references to the Plantpedia detail screen and main Plantpedia view.</summary>
     */
    private void Populate()
    {
        foreach (var plant in database.GETAllPlants())
        {
            GameObject button = Instantiate(buttonPrefab, drawer.transform, false);
            button.GetComponent<PlantpediaButtonUtility>().SetPlantpediaScreen(plantpediaMainScreen);
            button.GetComponent<PlantpediaButtonUtility>().SetDetailScreen(planpediaDetailScreen);
            button.SetActive(true);
            button.GetComponent<PlantpediaButtonUtility>().SetFromPlantData(plant);
        }    
    }

    /**
     * <summary>Method that waits for the database to be read to then populate the view in order to avoid NullPointerExceptions</summary>
     */
    private IEnumerator GETData()
    {
        yield return new WaitUntil(() => database.read);
        Populate();
    }
}
