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
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject dashboardOrigin;
    [SerializeField] private GameObject manager;
    public void SetScreen(Sprite icon, string nickname, string kind, PlantState states)
    {   
        this.nickname.text = nickname;
        this.kind.text = kind;
        this.icon.sprite = icon;
        //Use state to determine color, faces and icons
    }

    //Update
    public void TurnChangeFieldsOn()
    {
        //Turn Inut Field and Dropdown ON
    }
    public void ChangePlantNickname(GameObject dashboardItem)
    {

    }
    public void ChangePlantKind(GameObject dashboardItem)
    {
        //Uodate Text, Image, New State with same port
    }

    public void ConfirmationPanelPopUp()
    {
        confirmationPanel.SetActive(true);
    }
    public void deletePlant()
    {
        manager.GetComponent<Manager>().DeletePlant(nickname);
    }
}
