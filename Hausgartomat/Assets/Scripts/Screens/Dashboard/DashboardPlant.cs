using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * <summary>
 * Class that manages the Plant Details Screen,
 * managing it´s corresponding equipments in the green house,
 * showing the state of the displayed plant and
 * allowing update and delition of the plant.
 * </summary>
 */
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
    [SerializeField] private Dropdown plantKinds;
    private GameObject plantItem;
    private GetPlantData _database;
    private List<Plant> plants;
    /**
     * <summary>
     * Sets the UI components of the Plant Details Screen to the information
     * of the selected plant.
     * </summary>
     * <param name="nickname"> Nickname of the plant</param>
     * <param name="plantItem"> Game object containing more details of the selected plant</param>
     * <param name="icon"> Image that represents the selected plant</param>
     * <param name="kind"> Type of plant </param>
     * <param name="states"> State of the selected plant </param>
     */
    public void SetScreen(Sprite icon, string nickname, string kind, PlantState states, GameObject plantItem)
    {
        this.nickname.text = nickname;
        this.kind.text = kind;
        this.icon.sprite = icon;
        this.state = states;
        manager.GetComponent<Manager>().SetActivePlant(nickname, states);
        //Use state to determine color, faces and icons
        SetState(states);
        this.plantItem = plantItem;
        plantpediaButton.GetComponent<PlantpediaDetailUtility>().SetUpUtility(state.PlantDB, thisScreen, plantpediaDetailScreen);

    }

    /**
     * <summary> On Awake, wait for the Database to be readable </summary>
     */
    private void Awake()
    {
        _database = GameObject.Find("Firebase").GetComponent<GetPlantData>();
        StartCoroutine(WaitForDatabase());
    }
    /**
     *<summary>
     * Corutine that pauses a process until the Database is readable.
     * When the database becomes readable, the list of available plant kinds
     * gets filled with the most actual values.
     *</summary>
     */
    private IEnumerator WaitForDatabase()
    {
        yield return new WaitUntil(() => _database.read);
        plants = _database.GETAllPlants();
        plantKinds.options.Add(new Dropdown.OptionData("Pflanze auswählen"));
        foreach (Plant plant in plants)
        {
            plantKinds.options.Add(new Dropdown.OptionData(plant.name));
        }

    }

    /**
     * <summary>
     * Set the UI components accordingly to the states of the
     * displayed plant.
     * </summary>
     */
    public void SetState(PlantState states)
    {
        SelectFaceAndLevel(states.WaterState, faceW, levelW);
        SelectFaceAndLevel(states.TempState, faceT, levelT);
        SelectFaceAndLevel(states.LightState, faceL, levelL);

        if (CheckWaterpump) MayInteract(waterPump, states.WaterState, true);
        MayInteract(ledLamp, states.LightState, true);
        MayInteract(fan, states.TempState, false);
    }

    /**
     * <summary>
     * Initiate the corrutine to temporarily lock the waterpump.
     * </summary>
     */
    public void StartWaterPumpLock()
    {
        StartCoroutine(LockWaterPump());
    }

    /**
     * <summary>
     * Corutine that locks the waterpump switch, to give the water pump time to
     * push water into the green house and the moisture sensor to update the
     * humidity level, as this takes a little while.
     * This is done to prevent the user from excesively watering the plant by error.
     * </summary>
     * 
     */
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

    /**
     * <summary>
     * Sets a switch as interactable or not, according to the
     * state of the plant in the respective value.
     * </summary>
     * 
     */
    private void MayInteract(Slider slider, int state, bool whenLow)
    {
        int[] mayTurnOnStates = { 0, 1 };
        if (!whenLow)
        {
            mayTurnOnStates[0] += 3;
            mayTurnOnStates[1] += 3;
        }
        if ((state == mayTurnOnStates[0] || state == mayTurnOnStates[1]) ||
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
    /**
     * <summary>
     * Update the nickname of the displayed plant.
     * </summary>
     */
    public void ChangePlantNickname()
    {
        if (nameField.text.Length != 0)
        {
            nickname.text = nameField.text;
            confirmBtn.interactable = true;
            plantItem.GetComponent<PlantItem>().Nickname = nameField.text;

            plantItem.GetComponentInChildren<Text>().text = nameField.text;
        }
        else
        {
            Debug.Log("Muss mind. eine Buchstabe haben");
            confirmBtn.interactable = false;
        }
        nameField.text = "";
    }

    /**
     * <summary>
     * Update the kind of the displayed plant.
     * </summary>
     */
    public void ChangePlantKind()
    {
        string kindChanged = kindDropdown.transform.GetChild(0).GetComponent<Text>().text;
        //Icon = icon from DB for new Plant kind
        if (kindChanged.Equals("Pflanze auswählen")) return;
        kind.text = kindChanged;
        confirmBtn.interactable = true;
        state.UpdateKind(kindChanged);
        plantItem.GetComponent<PlantItem>().Kind = kindChanged;
        plantpediaButton.GetComponent<PlantpediaDetailUtility>().SetUpUtility(kindChanged, thisScreen, plantpediaDetailScreen);
        kindDropdown.value = 0;
    }

    /**
     * <summary>
     * Depending on which property is being updated, call
     * the corresponding method.
     * </summary>
     */
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

    /**
     * <summary>
     * Sets active the corresponding fields to 
     * be able to update the selected option.
     * </summary>
     */
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

    /**
     * <summary>
     * Activate the confirmation Panel.
     * </summary>
     */
    public void ConfirmationPanelPopUp(bool on)
    {
        optionsPanel.SetActive(false);
        confirmationPanel.SetActive(on);
    }
    /**
     * <summary>
     * Activate the Options Panel.
     * </summary>
     */
    public void OptionsPanelPopUp(bool on)
    {
        optionsPanel.SetActive(on);
    }
    /**
     * <summary>
     * Deactivates the confirmation and options panels.
     * Gets the manager to delete the displayed plant.
     * </summary>
     */
    public void DeletePlant()
    {
        confirmationPanel.SetActive(false);
        optionsPanel.SetActive(false);
        manager.GetComponent<Manager>().DeletePlant(plantItem);
    }

    /**
     * <summary>
     * Enables/Disables the panel for editing the displayed plant´s information.
     * </summary>
     */
    public void OpenInfoEditor(bool on)
    {
        updatePanel.SetActive(on);
    }

    /**
     * <summary>
     * Start the corutine to update the information displayed on the screen
     * </summary>
     */
    void OnEnable()
    {
        manager.GetComponent<Manager>().StartCoroutine(manager.GetComponent<Manager>().DashboardPlantUpdate());
    }

    /**
     * <summary>
     * Stop the corutine to update the information displayed on the screen
     * </summary>
     */
    void OnDisable()
    {
        manager.GetComponent<Manager>().StopCoroutine(manager.GetComponent<Manager>().DashboardPlantUpdate());
    }

    /**
     * <summary>
     * Sets the corresponding smiley and color of a condition of this plant.
     * </summary>
     * <param name="i"> State of this condition 
     * 0: Too low
     * 1: Low
     * 2: Good
     * 3: High
     * 4: Too High
     * </param>
     * <param name="image">Image component of the placeholder for the smiley</param>
     * <exception> If the case does not fit the switch, the color is set to purple and 
     * the arduino is considered dissconnected</exception>
     */
    private void SelectFaceAndLevel(int i, Image image, GameObject level)
    {
        Image stateSprite = level.GetComponent<Image>();
        if (manager.GetComponent<Manager>().Sp.IsOpen)
        {
            switch (i)
            {
                case 0: //Low
                    image.sprite = face2;
                    stateSprite.color = new Color32(0xb3, 0x52, 0x52, 0xFF);;//RED
                    break;
                case 1:
                    image.sprite = face1;
                    stateSprite.color = new Color32(0xDD, 0xCA, 0x8B, 0xFF);//YELLOW
                    break;
                case 2:
                    image.sprite = face0;
                    stateSprite.color = new Color32(0x97, 0xBB, 0x8F, 0xFF); //GREEN
                    break;
                case 3:
                    image.sprite = face1;
                    stateSprite.color = new Color32(0xDD, 0xCA, 0x8B, 0xFF); //YELLOW
                    break;
                case 4: //High
                    image.sprite = face2;
                    stateSprite.color = new Color32(0xb3, 0x52, 0x52, 0xFF); //RED
                    break;
            }
        }
        else
        {
            image.sprite = disconnected;
            stateSprite.color = new Color32(0xDB, 0x8B, 0xDD, 0xFF);
        }

    }
}
