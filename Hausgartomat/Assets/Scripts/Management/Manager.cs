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
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite plus;

    private GameObject add;
    private GameObject empty;

    // Start is called before the first frame update
    void Start()
    {
        //add = prefab;
        //add.GetComponent<Image>().color = Color.white;
        //add.GetComponent<Image>().sprite = plus;

        //1 Add dummy plants, Bob and Carla over the Add and empty buttons
        int dashboardItems = dashboard.transform.GetChildCount();
        Debug.Log(dashboardItems);
        string itemName = dashboard.transform.GetChild(dashboardItems - 3).name;
        Debug.Log(itemName);
        

    }

    //Update should be done every 10-30 seconds
    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
