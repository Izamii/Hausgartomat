using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Assertions;
/**
 * <summary>
 * Class <c>GetPlantData</c> makes Firestore API calls to retrieve data from a Firestore database.
 * And converts it into <c>Plant</c> class instances to provide the application with a workable source of data from the
 * database.
 * </summary>
 */
public class GetPlantData : MonoBehaviour
{
    private QuerySnapshot databaseSnapshot;
    private List<Plant> plantList = new List<Plant>();
    public bool read = false;
    public void Start()
    {
        CheckDependencies();
    }
    /**
     * <summary>Asynchronously checks the needed database dependencies and tries to fix them if they are broken.</summary>
     */
    private async void CheckDependencies()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {   
            Debug.Log("Dependencies are functional.");
            StartCoroutine(Plantpedia());
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }
    }
    /**
     * <summary>Makes an asynchronous API call to retrieve a snapshot of database data and converts them into <c>Plant</c> instances
     * that are updated into <value>plantList</value>.</summary>
     */
    private IEnumerator Plantpedia()
    {
        Task<QuerySnapshot> plantpediaTask = FirebaseFirestore.DefaultInstance.Collection("Plantpedia").GetSnapshotAsync();
        yield return new WaitUntil(() => plantpediaTask.IsCompleted);

        if (plantpediaTask.Exception != null)
        {
            Debug.Log("Database Snapshot failed..");
            if (plantpediaTask.Exception.InnerException != null)
                Debug.Log(plantpediaTask.Exception.InnerException.Message);
            yield break;
        }

        databaseSnapshot = plantpediaTask.Result;
        plantList.Clear();
        foreach (DocumentSnapshot plant in databaseSnapshot.Documents)
        {
            plantList.Add(plant.ConvertTo<Plant>());
            //Debug.Log(plant.ConvertTo<Plant>().name);
        }
        Debug.Log("Done");
        read = true;
    }
    /**
     * <summary>Updates the currently saved data by calling <c>Plantpedia</c> asynchronously.</summary>
     */
    public void UpdatePlantData()
    {
        read = false;
        StartCoroutine(Plantpedia());
    }
    /**
     * <summary>Returns a list of all saved instances of <c>Plant</c>.></summary>
     */
    public List<Plant> GETAllPlants()
    {
        return plantList;
    }
    /**
     * <summary>Retrieves the data of a <c>Plant</c> by the given name, if no match is found returns null.</summary>
     *  <param name="plantName"> Plantname of wanted <c>Plant</c> data</param>
     */
    public Plant GETSinglePlant(string plantName)
    {
        Plant plantreturn = null;
        foreach (Plant plant in plantList)
        {
            if (plant.name.Equals(plantName))
            {
                plantreturn = plant;
            }
        }
        return plantreturn;
    }
}
