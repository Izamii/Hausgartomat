using System;
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

    [Header("Icons")]
    [SerializeField] private Image faceW;
    [SerializeField] private GameObject levelW;
    [SerializeField] private Image faceT;
    [SerializeField] private GameObject levelT;
    [SerializeField] private Image faceL;
    [SerializeField] private GameObject levelL;
    [Space]
    [SerializeField] private Sprite face0;
    [SerializeField] private Sprite face1;
    [SerializeField] private Sprite face2;
    [SerializeField] private Sprite disconnected;

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
    [Header("Sliders")]
    [SerializeField] private Slider waterPump;
    [SerializeField] private Slider fan;
    [SerializeField] private Slider ledLamp;
    private bool CheckWaterpump = true;
    [Space]
    [Header("General")]
    [SerializeField] private GameObject manager;

    [SerializeField] private GameObject plantpediaButton;
    [SerializeField] private GameObject thisScreen;
    [SerializeField] private GameObject plantpediaDetailScreen;
    public void SetScreen(Sprite icon, string nickname, string kind, PlantState states)
    {   
        this.nickname.text = nickname;
        this.kind.text = kind;
        this.icon.sprite = icon;
        this.state = states;
        manager.GetComponent<Manager>().SetActivePlant(nickname);
        //Use state to determine color, faces and icons
        SetState(nickname);
        plantpediaButton.GetComponent<PlantpediaDetailUtility>().SetUpUtility(state.PlantDB, thisScreen, plantpediaDetailScreen);

    }
    public void SetState(string nickname)
    {
        PlantState state = GameObject.Find(nickname).GetComponent<PlantState>();
        SelectFaceAndLevel(state.WaterState, faceW, levelW);
        SelectFaceAndLevel(state.TempState, faceT, levelT);
        SelectFaceAndLevel(state.LightState, faceL, levelL);

        if(CheckWaterpump) MayInteract(waterPump, state.WaterState, true);
        MayInteract(ledLamp, state.LightState, true);
        MayInteract(fan, state.TempState, false);
    }

    public void StartWaterPumpLock()
    {
        StartCoroutine(LockWaterPump());
    }
    private IEnumerator LockWaterPump()
    {
        CheckWaterpump = false;
        Debug.Log("Locking waterpump");
        yield return new WaitForSeconds(3);//10
        waterPump.interactable = false;
        waterPump.value = 0;
        Debug.Log("Waterpump Locked");
        yield return new WaitForSeconds(5);//20
        Debug.Log("Unlocking Waterpump");
        CheckWaterpump = true;
        yield break;
    }

    private void MayInteract(Slider slider, int state, bool whenLow)
    {
        int[] mayTurnOnStates = { 0, 1 };
        if (!whenLow)
        {
            mayTurnOnStates[0] += 3;
            mayTurnOnStates[1] += 3;
        }
        if((state == mayTurnOnStates[0] || state == mayTurnOnStates[1]) || 
            (slider.value == 1))
        {
            slider.interactable = true;
        }
        else
        {
            slider.interactable = false;
        }
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


    void OnEnable()
    {
        manager.GetComponent<Manager>().StartCoroutine(manager.GetComponent<Manager>().DashboardPlantUpdate());
        //Debug.Log("Sup?");
    }

    void OnDisable()
    {
        manager.GetComponent<Manager>().StopCoroutine(manager.GetComponent<Manager>().DashboardPlantUpdate());
        //Debug.Log("Bye");
    }

    private void SelectFaceAndLevel(int i, Image image, GameObject level)
    {
        RectTransform rt = level.GetComponent<RectTransform>();
        Image img = level.GetComponent<Image>();
        if (manager.GetComponent<Manager>().Sp.IsOpen)
        {
            switch (i)
            {
                case 0: //Low
                    image.sprite = face2;
                    rt.sizeDelta = new Vector2(73, 15);
                    img.color = new Color32(0xFF, 0x10, 0x00, 0x88);//RED
                    break;
                case 1:
                    image.sprite = face1;
                    rt.sizeDelta = new Vector2(73, 30);
                    img.color = new Color32(0xFF, 0xD3, 0x00, 0x88);//YELLOW
                    break;
                case 2:
                    image.sprite = face0;
                    rt.sizeDelta = new Vector2(73, 45);
                    img.color = new Color32(0xFF, 0x10, 0x00, 0x00); //GREEN
                    break;
                case 3:
                    image.sprite = face1;
                    rt.sizeDelta = new Vector2(73, 60);
                    img.color = new Color32(0xFF, 0xD3, 0x00, 0x88); //YELLOW
                    break;
                case 4: //High
                    image.sprite = face2;
                    rt.sizeDelta = new Vector2(73, 75);
                    img.color = new Color32(0xFF, 0x10, 0x00, 0x88); //RED
                    break;
            } 
        }
        else
        {
            image.sprite = disconnected; //Find new Sprite here for not connected
            image.rectTransform.localScale = new Vector3(0.9f, 0.9f, 1f);
            rt.sizeDelta = new Vector2(71, 75);
            img.color = new Color32(0xDB, 0x8B, 0xDD, 0xFF);
        }
        
    }
}
