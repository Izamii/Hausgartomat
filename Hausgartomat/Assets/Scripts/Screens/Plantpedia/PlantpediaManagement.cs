using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlantpediaManagement : MonoBehaviour
{
    [SerializeField] private GameObject drawer;

    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private GameObject plantpediaMainScreen;
    
    [SerializeField] private GameObject planpediaDetailScreen;

    private GetPlantData database;

    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("Firebase").GetComponent<GetPlantData>();
        StartCoroutine(GETData());
    }

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


    private IEnumerator GETData()
    {
        yield return new WaitUntil(() => database.read);
        Populate();
    }
}
