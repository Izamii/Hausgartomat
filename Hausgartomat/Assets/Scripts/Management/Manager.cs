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
    [SerializeField] private GameObject navi;
    private GoToScreen _navi; 

    private GameObject add;
    private GameObject empty;
    private GameObject plant;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateAddPlant();
        InstatiateEmptys();
        //1 Add dummy plants, Bob and Carla over the Add and empty buttons
        int dashboardItems = dashboard.transform.GetChildCount();
        Debug.Log(dashboardItems);
        string itemName = dashboard.transform.GetChild(dashboardItems - 3).name;
        Debug.Log(itemName);
        

    }

    private void InstantiateDashboardItem(GameObject item, Color color, Sprite icon, PlantItem plantItem)
    {
        item = Instantiate(prefabDashPlant, dashboard.transform);
        item.GetComponent<Image>().color = color;
        if(icon != null)
        {
            item.transform.GetChild(1).GetComponent<Image>().sprite = icon;
            item.transform.GetChild(0).GetComponent<Text>().text = plantItem.getNickname();

        }
        else
        {
            item.transform.GetChild(1).GetComponent<Image>().color = Color.clear;
            item.transform.GetChild(0).GetComponent<Text>().text = "";
        }

    }

    private void InstatiateEmptys()
    {
        InstantiateDashboardItem(empty, Color.clear, null, null);
        InstantiateDashboardItem(empty, Color.clear, null, null);
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


    public PlantItem newPlant(Image icon, string name, string nickname, string kind)
    {
        PlantState states = new PlantState(kind);
        return new PlantItem(icon, nickname, kind, states);
    }
    //Update should be done every 10-30 seconds
    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
