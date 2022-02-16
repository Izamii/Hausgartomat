using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * This class contains the information of one plant that 
 * Dashboard and Plant Information require to display it.
 * */
public class PlantItem:MonoBehaviour
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string nickname;
    [SerializeField] private PlantState plantState;


}
