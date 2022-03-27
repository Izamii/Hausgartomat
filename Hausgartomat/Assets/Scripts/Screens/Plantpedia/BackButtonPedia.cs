using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonPedia : MonoBehaviour
{

    [SerializeField] private GameObject lastScreen;
    public void GoToLastScreen()
    {
        lastScreen = gameObject.transform.parent.gameObject.GetComponentInParent<PlantpediaInfoUtility>().LastScreen;
        gameObject.GetComponent<GoToScreen>().GoBack(lastScreen);
    }
}
