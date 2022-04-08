using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * <summary>A test class to demonstrate the instantiation of Planung entries within the preset view.</summary>
 */
public class AddPlanTest : MonoBehaviour
{
    [SerializeField] private GameObject planItemPrefab;
    [SerializeField] private GameObject itemContainer;
    [SerializeField] private GameObject screen;
    /**
     * <summary>Instantiates a prefab entry to show that the planing screen is dynamic.</summary>
     */
    public void AddPlanedItem()
    {
        GameObject button = Instantiate(planItemPrefab, itemContainer.transform, false);
        button.SetActive(true);
        screen.SetActive(false);
        screen.SetActive(true);
    }
}
