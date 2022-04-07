using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 *<summary>
 * The last screen of a Plantpedia entry can be variable.
 * This class contains the actual last screen of this Plantpedia entry.
 *</summary> 
 */
public class BackButtonPedia : MonoBehaviour
{

    [SerializeField] private GameObject lastScreen;
    public void GoToLastScreen()
    {
        lastScreen = gameObject.transform.parent.gameObject.GetComponentInParent<PlantpediaInfoUtility>().LastScreen;
        gameObject.GetComponent<GoToScreen>().GoBack(lastScreen);
    }
}
