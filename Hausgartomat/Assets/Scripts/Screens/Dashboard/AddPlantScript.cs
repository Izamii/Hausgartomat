using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPlantScript : MonoBehaviour
{
    [SerializeField] private Image plantImg;
    [SerializeField] private Dropdown plantSelection;
    [SerializeField] private string nickname;
    [SerializeField] private Sprite testSprite;
    public Image PlantImg { get => plantImg; set => plantImg = value; }
    public Dropdown PlantSelection { get => plantSelection; set => plantSelection = value; }
    public string Nickname { get => nickname; set => nickname = value; }

    public void ChangeAddPlantImage()
    {
        PlantImg.sprite = testSprite;
    }
}
