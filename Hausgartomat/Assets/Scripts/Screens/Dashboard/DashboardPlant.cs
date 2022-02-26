using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPlant : MonoBehaviour
{
    [SerializeField] private Text nickname;
    [SerializeField] private Text kind;
    [SerializeField] private Image icon;
    [SerializeField] private PlantState state;

    private PlantItem _plantItem;

    public void SetScreen(Image icon, string nickname, string kind, PlantState states)
    {   
        this.nickname.text = nickname;
        this.kind.text = kind;
        this.icon.sprite = icon.sprite;
    }
}
