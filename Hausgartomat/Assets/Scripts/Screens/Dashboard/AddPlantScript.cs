using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

/**
 * <summary> Class to manage the addition of new Plants </summary>
 * */
public class AddPlantScript : MonoBehaviour
{
    [SerializeField] private Image plantImg;
    [SerializeField] private Text plantKind;
    [SerializeField] private Text nickname;
    [SerializeField] private Text nicknameConfirmScreen;
    [SerializeField] private InputField nameField;
    [SerializeField] private Sprite plantSprite;
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject dashboard;
    [SerializeField] private Button confirmationBtn;
    [SerializeField] private GameObject backBtn;

    [Header("Screens")]
    [SerializeField] private GameObject screen1;
    [SerializeField] private GameObject screen2;
    [SerializeField] private GameObject screen3;
    [SerializeField] private GameObject lastScreen;

    public Image PlantImg { get => plantImg; set => plantImg = value; }
    public Text Nickname { get => nickname; set => nickname = value; }
    public GameObject LastScreen { get => lastScreen; set => lastScreen = value; }

    private void Start()
    {
        confirmationBtn.interactable = false;
    }

    /**
     * Update the sprite for the selected plant kind 
     */
    public void ChangeAddPlantImage(Sprite sprite)
    {
        PlantImg.sprite = sprite;
    }

    public void UpdateNickname(Text input)
    {
        nickname = input;
        TurnAddButtonOn();
    }

    public void UpdateSelection(string kind)
    {
        //Get Plant Info and Image from DB
        //Sprite plantSprite = ;//from DB
        //ChangeAddPlantImage(plantSprite);
    }

    /**
     * <summary>
     * Calls the manager to instatiate a new plant.
     * </summary>
     */
    public void CreateNewPlant()
    {
        string selected = plantKind.text;
        if (selected.Length < 1) return;
        PlantState state = new PlantState(selected);
        PlantItem newPlantItem = 
            new PlantItem(plantImg.sprite, nicknameConfirmScreen.text, selected , state);
        manager.InstantiateNewPlantItem(newPlantItem);
        //manager.GoTo.GoBack(dashboard);
    }

    /**
     * <summary>
     * Enable/Disable the confirmation button.
     * </summary>
     */
    public void TurnAddButtonOn()
    {
        if (nameField.text.Length > 0)
        {
            confirmationBtn.interactable = true;
            nicknameConfirmScreen.text = nameField.text;
            //Debug.Log(nameField.text);
        }
        else
        {
            confirmationBtn.interactable = false;
        }
    }

    /**
     * <summary>
     * Update what the last screen of the form is.
     * If the plant has been instatiated, the back button is deactivated.
     * </summary>
     */
    public void SetLastScreen()
    {
        if (screen1.activeSelf)
        {
            LastScreen = dashboard;
        }
        if (screen2.activeSelf)
        {
            LastScreen = screen1;
        }
        if (screen3.activeSelf)
        {
            LastScreen = screen2;
            backBtn.SetActive(false);
        }
    }
    /**
     * <summary>
     * After leaving the form, the initial state of the form is reset.
     * </summary>
     */
    public void ResetLastScreen()
    {
        LastScreen = dashboard;
        screen1.SetActive(true);
        screen3.SetActive(false);
        backBtn.SetActive(true);
    }
    /**
     * <summary>
     * Empty the field where the name of the plant is set.
     * </summary>
     */
    public void EmptyField()
    {
        nameField.text = "";
    }
}
