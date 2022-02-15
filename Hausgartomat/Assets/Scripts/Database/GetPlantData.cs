using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Assertions;

public class GetPlantData : MonoBehaviour
{
    [SerializeField] private string plantname = "Tomate";
    
    private QuerySnapshot databaseSnapshot;
    public void Start()
    {
        StartCoroutine(Plantpedia());
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
        Debug.Log("Done");
    }

    public void updatePlantData()
    {
        StartCoroutine(Plantpedia());
    }

    public List<Plant> getAllPlants()
    {
        List<Plant> plantList = new List<Plant>();
        if (databaseSnapshot != null)
        {
            foreach (DocumentSnapshot plant in databaseSnapshot.Documents)
            {
                plantList.Add(plant.ConvertTo<Plant>());
            }
        }
        return plantList;
    }
    
    public Plant getSinglePlant(string name)
    {
        //getAllPlants();
        Plant plantreturn = null;
        foreach (DocumentSnapshot plant in  databaseSnapshot.Documents)
        {
            if (plant.Id == name)
            {
                plantreturn = plant.ConvertTo<Plant>();
            }
        }

        return plantreturn;
    }

    public void buttonClick()
    {
        Plant returnval = getSinglePlant(plantname);
        if (returnval != null)
        {
            returnval.printData();
        }
        else
        {
            Debug.Log(String.Format("Couldn't find plant named: {0}", plantname));
        }
    }
}
