using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlantpediaManagement : MonoBehaviour
{
    [SerializeField] private GameObject Drawer;

    [SerializeField] private GameObject ButtonPrefab;

    [SerializeField] private int numberToCreate;

    private GetPlantData database;

    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("Firebase").GetComponent<GetPlantData>();
        StartCoroutine(GETData());
    }

    void Populate()
    {
        foreach (var plant in database.GETAllPlants())
        {
            GameObject button = Instantiate(ButtonPrefab, Drawer.transform, false);
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
