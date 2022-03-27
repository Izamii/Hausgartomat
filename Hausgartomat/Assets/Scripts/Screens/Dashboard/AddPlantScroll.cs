using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPlantScroll : MonoBehaviour
{
    [SerializeField] private Text plantNameBox;
    [SerializeField] private Text plantNameNextScreen;
    [SerializeField] private Text plantNameConfirmScreen;
    [SerializeField] private Button nextPlantButton;
    [SerializeField] private Button previousPlantButton;
    private List<Plant> plantList;
    private GetPlantData database;
    private int currentPlant;
    private void Start()
    {
        database = GameObject.Find("Firebase").GetComponent<GetPlantData>();
        StartCoroutine(WaitForDatabase());
    }

    public void RightButtonClick()
    {
        if (plantList.Count == 0 || (plantList.Count-1) < (currentPlant+1))
        {
            return;
        }
        
        currentPlant += 1;
        plantNameBox.text = plantList[currentPlant].name;
        plantNameNextScreen.text = plantList[currentPlant].name;
        plantNameConfirmScreen.text = plantList[currentPlant].name;
        UpdateButtons(); 
    }
    
    public void LeftButtonClick()
    {
        if (plantList.Count == 0 || 0 > currentPlant-1 )
        {
            return;
        }
        
        currentPlant -= 1;
        plantNameBox.text = plantList[currentPlant].name;
        plantNameNextScreen.text = plantList[currentPlant].name;
        plantNameConfirmScreen.text = plantList[currentPlant].name;
        UpdateButtons();
    }
    
    private IEnumerator WaitForDatabase()
    {
        yield return new WaitUntil(() => database.read);
        plantList = database.GETAllPlants();
        if (plantList.Count != 0)
        {
            plantNameBox.text = plantList[currentPlant].name;
            plantNameNextScreen.text = plantList[currentPlant].name;
            plantNameConfirmScreen.text = plantList[currentPlant].name;
            UpdateButtons();
        }
        else
        {
            previousPlantButton.interactable = false;
            nextPlantButton.interactable = false;
        }
    }

    public Plant GetSelectedPlant()
    {
        return plantList[currentPlant];
    }

    private void UpdateButtons()
    {
        previousPlantButton.interactable = currentPlant != 0;
        nextPlantButton.interactable = currentPlant != plantList.Count-1;
    }
}
