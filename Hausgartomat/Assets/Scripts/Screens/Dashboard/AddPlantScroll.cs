using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *<summary>
 * Class to manage the list of plant kinds in the Add Plant´s first screen.
 *</summary>
 *
 */
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
    /**
     * On Start, get the actual list of plant kinds in the database.
     */
    private void Start()
    {
        database = GameObject.Find("Firebase").GetComponent<GetPlantData>();
        StartCoroutine(WaitForDatabase());
    }

    /**
     * <summary>
     * Navigate forward in the list of plant kinds.
     * </summary>
     */
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

    /**
     * <summary>
     * Navigate backwards in the list of plant kinds.
     * </summary>
     */
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
    
    /**
     * <summary>
     * Corrutine that waits for the Databse to be ready to be read
     * and, when ready, fills the plant list in this class with a list
     * of all available kinds of plant.
     * If the Database is empty, the plant navigation buttons
     * are disabled.
     * </summary>
     */
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

    /**
     * <summary>
     * Checks if there is still a next or previous plant on the list.
     * If there isn´t, block the corresponding button.
     * </summary>
     */
    private void UpdateButtons()
    {
        previousPlantButton.interactable = currentPlant != 0;
        nextPlantButton.interactable = currentPlant != plantList.Count-1;
    }
}
