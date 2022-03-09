using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPlant : MonoBehaviour
{
    [Header("Plant Item Information")]
    [SerializeField] private Text nickname;
    [SerializeField] private Text kind;
    [SerializeField] private Image icon;
    [SerializeField] private PlantState state;
    [Space]
    [Header("Panel UI Parts")]
    /*[Space]
    [Header("   Options Menu")]*/
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject optionsPanel;
    //[Header("   Information Editor")]
    [Space]
    [SerializeField] private GameObject updatePanel;
    [SerializeField] private Dropdown kindDropdown;
    [SerializeField] private InputField nameField;
    [SerializeField] private Button confirmBtn;
    [Space]
    [Header("General")]
    [SerializeField] private GameObject manager;

    public void SetScreen(Sprite icon, string nickname, string kind, PlantState states)
    {   
        this.nickname.text = nickname;
        this.kind.text = kind;
        this.icon.sprite = icon;
        //Use state to determine color, faces and icons
    }

    //Update

    public void ChangePlantNickname()
    {
        if (nameField.text.Length != 0)
        {
            nickname.text = nameField.text;
            confirmBtn.interactable = true;

        }
        else
        {
            Debug.Log("Muss mind. eine Buchstabe haben");
            confirmBtn.interactable = false;
        }
    }
    public void ChangePlantKind()
    {
        string kindChanged = kindDropdown.transform.GetChild(0).GetComponent<Text>().text;
        //Icon = icon from DB for new Plant kind
        if (!kindChanged.Equals("Pflanze Auswählen"))
        {
            kind.text = kindChanged;
            confirmBtn.interactable = true;

        }
        else
        {
            confirmBtn.interactable = false;

        }

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
    public void UpdatePlantOption(int option)
    {
        switch (option)
        {
            case 1:
                nameField.gameObject.SetActive(true);
                kindDropdown.gameObject.SetActive(false);
                break;
            case 2:
                nameField.gameObject.SetActive(false);
                kindDropdown.gameObject.SetActive(true);

                break;
            default:

                break;
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

    public void OpenInfoEditor(bool on)
    {
        updatePanel.SetActive(on);
    }
}
