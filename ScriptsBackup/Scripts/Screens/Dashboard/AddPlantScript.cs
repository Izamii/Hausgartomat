using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class AddPlantScript : MonoBehaviour
{
    [SerializeField] private Image plantImg;
    [SerializeField] private Dropdown plantSelection;
    [SerializeField] private Text nickname;
    [SerializeField] private InputField nameField;
    [SerializeField] private Sprite plantSprite;
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject dashboard;
    [SerializeField] private Button confirmationBtn;
    public Image PlantImg { get => plantImg; set => plantImg = value; }
    public Dropdown PlantSelection { get => plantSelection; set => plantSelection = value; }
    public Text Nickname { get => nickname; set => nickname = value; }

    private void Start()
    {
        confirmationBtn.interactable = false;
    }
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


    public void CreateNewPlant()
    {
        string selected = plantSelection.transform.GetChild(0).GetComponent<Text>().text;
        if (selected.Length < 1) return;
        PlantState state = new PlantState(selected);
        PlantItem newPlantItem = 
            new PlantItem(plantImg.sprite, nickname.text, selected , state);
        manager.InstantiateNewPlantItem(newPlantItem);
        manager.GoTo.GoBack(dashboard);
    }

    public void TurnAddButtonOn()
    {
        if (nameField.text.Length > 0)
        {
            confirmationBtn.interactable = true;
            //Debug.Log(nameField.text);
        }
        else
        {
            confirmationBtn.interactable = false;
        }
    }
}
