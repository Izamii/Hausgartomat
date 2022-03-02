using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class AddPlantScript : MonoBehaviour
{
    [SerializeField] private Image plantImg;
    [SerializeField] private Dropdown plantSelection;
    [SerializeField] private string nickname;
    [SerializeField] private Sprite plantSprite;
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject dashboard;
    public Image PlantImg { get => plantImg; set => plantImg = value; }
    public Dropdown PlantSelection { get => plantSelection; set => plantSelection = value; }
    public string Nickname { get => nickname; set => nickname = value; }

    public void ChangeAddPlantImage(Sprite sprite)
    {
        PlantImg.sprite = sprite;
    }

    public void UpdateNickname(Text input)
    {
        nickname = input.text;
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
        PlantState state = new PlantState(selected);
        PlantItem newPlantItem = 
            new PlantItem(plantImg.sprite, nickname, selected , state);
        manager.InstantiateNewPlantItem(newPlantItem);
        manager.GoTo.GoBack(dashboard);
    }

}
