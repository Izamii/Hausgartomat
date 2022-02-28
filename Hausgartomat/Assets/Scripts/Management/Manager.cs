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
    [SerializeField] private Sprite testSprite;
    [SerializeField] private GameObject navi;
    private GoToScreen _navi;

    public GoToScreen Navi { get => _navi; set => _navi = value; }

    // Start is called before the first frame update
    void Start()
    {
        PlantState s = new PlantState("Mota");
        PlantItem plantyThePlant = new PlantItem(testSprite, "Sandra", "Mota", s);

        InstantiateBottom();
        InstantiateNewPlantItem(plantyThePlant);
    }
    public void InstantiateNewPlantItem(PlantItem plant)
    {
        int dashboardItems = dashboard.transform.GetChildCount();
        Navi = navi.GetComponent<GoToScreen>();
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
        Debug.Log(Navi);
        Navi = navi.GetComponent<GoToScreen>();
        Debug.Log(Navi);
        GameObject item = Instantiate(prefabDashAddBtn, dashboard.transform);
        item.GetComponent<Button>().onClick.AddListener(Navi.GoToAddPlant);
    }
    private void InstantiateBottom()
    {
        InstantiateAddPlant();
        InstatiateEmptys();
    }


}
