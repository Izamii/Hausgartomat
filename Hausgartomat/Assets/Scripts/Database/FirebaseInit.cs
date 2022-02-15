using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{
    public UnityEvent OnFirebaseInitialized = new UnityEvent();
    // Start is called before the first frame update
    private async void Start()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            var app = FirebaseApp.DefaultInstance;
            OnFirebaseInitialized.Invoke();
        }
        else
        {
            Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        }
    }
}
