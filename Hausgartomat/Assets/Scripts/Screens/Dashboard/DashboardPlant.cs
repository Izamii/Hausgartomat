using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPlant : MonoBehaviour
{
    [SerializeField] private Text nickname;
    [SerializeField] private Text kind;
    /*[SerializeField] private Sprite icon;
    [SerializeField] private Sprite stateWater;
    [SerializeField] private Sprite stateLight;
    [SerializeField] private Sprite stateTemp;*/

    private PlantItem _plantItem;

    public void setScreen(Sprite icon, string nickname, string kind, PlantState states)
    {   
        this.nickname.text = nickname;
    }
}
