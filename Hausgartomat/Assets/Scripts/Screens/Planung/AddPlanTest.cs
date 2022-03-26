using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPlanTest : MonoBehaviour
{
    [SerializeField] private GameObject planItemPrefab;
    [SerializeField] private GameObject itemContainer;
    [SerializeField] private GameObject screen;

    public void AddPlanedItem()
    {
        GameObject button = Instantiate(planItemPrefab, itemContainer.transform, false);
        button.SetActive(true);
        screen.SetActive(false);
        screen.SetActive(true);
    }
}
