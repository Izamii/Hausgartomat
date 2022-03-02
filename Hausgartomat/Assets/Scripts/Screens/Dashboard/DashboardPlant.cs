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
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject manager;
    [SerializeField] private Dropdown kindDropdown;
    [SerializeField] private InputField nameField;
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
    public void ChangePlantNickname()
    {
        nickname.text = nameField.text;
    }
    public void ChangePlantKind()
    {
        //Icon = icon from DB for new Plant kind
        kind.text = kindDropdown.transform.GetChild(0).GetComponent<Text>().text;
    }

    public void UpdatePlantInfo()
    {
        if (kindDropdown.IsActive())
        {
            ChangePlantKind();
        }
        else
        {
            ChangePlantNickname();
        }
    }

    public void ConfirmationPanelPopUp(bool on)
    {
        confirmationPanel.SetActive(on);
    }
    public void OptionsPanelPopUp(bool on)
    {
        optionsPanel.SetActive(on);
    }
    public void DeletePlant()
    {
        confirmationPanel.SetActive(false);
        optionsPanel.SetActive(false);
        manager.GetComponent<Manager>().DeletePlant(nickname);
    }
}
