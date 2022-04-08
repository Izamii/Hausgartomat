using System;
using Firebase.Firestore;
using UnityEngine;
/**
 * <summary>A dataclass per Firebase convention in order to convert Firebase data into actual usable C# data.</summary>
 */
[FirestoreData]
public class Plant
{
    [FirestoreProperty]
    public string name { get; set; }
    
    [FirestoreProperty]
    public string scientificname { get; set; }
    
    [FirestoreProperty]
    public string description { get; set; }
    
    [FirestoreProperty]
    public int spacerequirements { get; set; }
    
    [FirestoreProperty]
    public int growthtime { get; set; }
    
    [FirestoreProperty]
    public float[] temperature { get; set; }
    
    [FirestoreProperty]
    public float[] humidity { get; set; }
    
    [FirestoreProperty]
    public float[] light { get; set; }
    
    [FirestoreProperty]
    public string[] potentialneigbors { get; set; }
    
    [FirestoreProperty]
    public string[] generalhints { get; set; }
    
    [FirestoreProperty]
    public string[] harvesthints { get; set; }

    public void printData()
    {
        Debug.Log(String.Format("{0} plant contains following properties:\n1. scientific name: {1}\n2. description: {2}\n3. spacerequirements: {3}\n4. growthtime: {4}\n5. temperature {5}\n6. humidity: {6}\n7. light: {7}\n8. potential neigbors: {8}\n9. general hints: {9}\n10. harvest hints: {10} ", name, scientificname, description,
            spacerequirements, growthtime, string.Join(", ", temperature), string.Join(", ", humidity), string.Join(", ", light), string.Join(", ", potentialneigbors), string.Join(", ", generalhints), string.Join(", ", harvesthints)));
        
    }
}
