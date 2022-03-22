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

public class GetPlantData : MonoBehaviour
{
    private QuerySnapshot databaseSnapshot;
    private List<Plant> plantList = new List<Plant>();
    private Manager manager;
    public bool read = false;
    public void Start()
    {
        //manager = GameObject.Find("Manager").GetComponent<Manager>();
        CheckDependencies();
    }

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
        //manager.StartCoroutine(manager.CheckStates());
    }

    public void UpdatePlantData()
    {
        read = false;
        StartCoroutine(Plantpedia());
    }

    public List<Plant> GETAllPlants()
    {
        return plantList;
    }
    
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
