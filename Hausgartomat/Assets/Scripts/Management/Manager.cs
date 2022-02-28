using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class organizes how the application works with the DB and Arduino.
 * Makes each plantItem in the Dashboard check it´s state with arduino periodically
 * It adds,deletes,modifies Plants to the Dashboard.
 * 
 **/
public class Manager : MonoBehaviour
{

    [SerializeField] private GameObject dashboard;
    [SerializeField] private GameObject prefabDashPlant;
    [SerializeField] private GameObject prefabDashAddBtn;
    [SerializeField] private Sprite plus;
    [SerializeField] private Sprite testSprite;
    [SerializeField] private GameObject navi;
    private GoToScreen _navi; 

    private GameObject add;
    private GameObject empty;
    private GameObject plant;

    // Start is called before the first frame update
    void Start()
    {
        PlantState s = new PlantState("Mota");
        PlantItem plantyThePlant = new PlantItem(testSprite, "Sandra", "Mota", s);
        InstantiateBottom();
        InstantiateNewPlantItem(plantyThePlant);
        //1 Add dummy plants, Bob and Carla over the Add and empty buttons
        
    }
    private void InstantiateNewPlantItem(PlantItem plant)
    {
        int dashboardItems = dashboard.transform.GetChildCount();
        _navi = navi.GetComponent<GoToScreen>();
        GameObject item = Instantiate(prefabDashPlant, dashboard.transform);
        
        item.GetComponent<PlantItem>().Nickname = plant.Nickname;
        item.GetComponent<PlantItem>().Kind = plant.Kind;
        item.GetComponent<PlantItem>().PlantState = plant.PlantState;
        item.GetComponent<PlantItem>().Manager = plant.Manager;
        item.GetComponent<PlantItem>().Icon = plant.Icon;
        item.transform.GetChild(0).GetComponent<Text>().text = plant.Nickname;
        item.transform.GetChild(1).GetComponent<Image>().sprite = plant.Icon;
        //Reorganize
        for (int i = 1; i<4 ; i++)
        {
            dashboard.transform.GetChild(dashboardItems-3).SetAsLastSibling();
        }
    }
    private void InstatiateEmptys()
    {
        for(int i = 0; i < 2; i++)
        {
            GameObject item = Instantiate(prefabDashPlant, dashboard.transform);
            item.transform.GetChild(1).GetComponent<Image>().color = Color.clear;
            item.transform.GetChild(0).GetComponent<Text>().text = "";
            item.GetComponent<Image>().color = Color.clear;
        }
    }
    private void InstantiateAddPlant()
    {
        Debug.Log(navi);
        Debug.Log(_navi);
        _navi = navi.GetComponent<GoToScreen>();
        Debug.Log(_navi);
        GameObject item = Instantiate(prefabDashAddBtn, dashboard.transform);
        item.GetComponent<Button>().onClick.AddListener(_navi.GoToAddPlant);
    }
    private void InstantiateBottom()
    {
        InstantiateAddPlant();
        InstatiateEmptys();
    }


    //Update should be done every 10-30 seconds
    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
