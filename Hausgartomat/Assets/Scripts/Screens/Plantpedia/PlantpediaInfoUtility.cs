using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/**
 * <summary>Utility class to control the behavior of the Plantpedia Detail screen.</summary>
 */
public class PlantpediaInfoUtility : MonoBehaviour
{
    [SerializeField] private Image thumbnail;
    [SerializeField] private Text plantName;
    [SerializeField] private Text latName;
    [SerializeField] private Text description;
    [SerializeField] private Text growthTime;
    [SerializeField] private Text neighbours;
    [SerializeField] private Text space;
    [SerializeField] private Text sunTime;
    [SerializeField] private Text humidity;
    [SerializeField] private Text temperature;
    [SerializeField] private GameObject lastScreen;

    public GameObject LastScreen { get => lastScreen; set => lastScreen = value; }
    /**
     * <summary>Method to fill in the preset Labels and Images of the details screen by a given instance of <c>Plant</c>.</summary>
     * <param name="plant">Plant data that represents a single plant.</param>
     */
    public void FillInfoScreen(Plant plant)
    {
        plantName.text = plant.name;
        latName.text = plant.scientificname;
        description.text = plant.description;
        growthTime.text = plant.growthtime + " Wochen";
        neighbours.text = plant.potentialneigbors.Aggregate("", (current, neighbour) => current + ", " + neighbour).Substring(2);
        space.text = plant.spacerequirements + " cm";
        sunTime.text = (plant.light[2] - 2) + "-" + (plant.light[2]);
        humidity.text = (plant.humidity[0]*100) + "-" + (plant.humidity[2]*100) + "%";
        temperature.text = plant.temperature[0] + "-" + plant.temperature[2] + "°C";
    }
}
