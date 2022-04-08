using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
/**
 * <summary>Utility class that is used to control the behavior of prefab Buttons that represent Plantpedia entries.</summary>
 */
public class PlantpediaButtonUtility : MonoBehaviour
{
    [SerializeField] private Text plantName;

    [SerializeField] private Text scientificPlantName;

    [SerializeField] private Text tempValue;

    [SerializeField] private Text humValue;

    [SerializeField] private Text lightValue;

    [SerializeField] private Image thumbnail;

    [SerializeField] private GameObject detailScreen;

    [SerializeField] private GameObject plantpediaScreen;

    [SerializeField] private PlantpediaDetailUtility detailUtility;

    private Plant plant;


    public void SetPlantpediaScreen(GameObject screen)
    {
        plantpediaScreen = screen;
    }

    public void SetDetailScreen(GameObject screen)
    {
        detailScreen = screen;
    }
    
    public void SetPlantName(string nameString)
    {
        plantName.text = nameString;
    }

    public void SetScientificPlantName(string nameString)
    {
        scientificPlantName.text = nameString;
    }

    public void SetTemperature(string tempString)
    {
        tempValue.text = tempString;
    }
    
    public void SetHumidity(string humString)
    {
        humValue.text = humString;
    }
    
    public void SetLight(string lightString)
    {
        lightValue.text = lightString;
    }

    public void SetThumbnail(Sprite image)
    {
        thumbnail.sprite = image;
    }

    public void SetPlant(Plant type)
    {
        plant = type;
    }
    
    public Plant GetPlant()
    {
        return plant;
    }
    /**
     * <summary>Method to fill prefab labels and images to represent a single instance of <c>Plant</c>.</summary>
     * <param name="data"><c>Plant</c> that represents the database values of a Plant</param>
     */
    public void SetFromPlantData(Plant data)
    {
        plantName.text = data.name;
        scientificPlantName.text = data.scientificname;
        tempValue.text = data.temperature[0] + "-" + data.temperature[2] + "°C";
        humValue.text = (data.humidity[0]*100) + "-" + (data.humidity[2]*100) + "%";
        lightValue.text = (data.light[2]-2) + "-" + (data.light[2]) + "h";
        plant = data;
        detailUtility.SetUpUtility(plant, plantpediaScreen, detailScreen);
    }
}
