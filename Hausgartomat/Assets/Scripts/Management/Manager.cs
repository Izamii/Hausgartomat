using System;
using System.Collections;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class organizes how the application works with the DB and Arduino.
 * Makes each plantItem in the Dashboard check its state with arduino periodically
 * It adds, deletes, modifies Plants to the Dashboard.
 * 
 **/
public class Manager : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField] private GameObject dashboard;
    [SerializeField] private GameObject prefabDashPlant;
    [SerializeField] private GameObject prefabDashAddBtn;
    [SerializeField] private Sprite testSprite;
    [SerializeField] private GameObject goToScreen;
    [SerializeField] private GameObject dashboardPlantScreen;
    private GoToScreen _goToScreen;
    private GetPlantData database;

    [Space]
    [Header("State information")]
    [SerializeField] private float light = 0;
    [SerializeField] private float temp = 0;
    [SerializeField] private float humid = 0;
    private string messageType = "";


    [Space]
    [Header("Open Dashboard Plant")]
    [SerializeField] private string activePlant = "";
    [SerializeField] private Slider waterPump;
    [SerializeField] private Slider fan;
    [SerializeField] private Slider ledLamp;
    private bool equipmentONt = false;
    private bool equipmentONl = false;
    private bool equipmentONh = false;
    private PlantState activePlantObject;
    private int activeWaterState = 4;
    private int activeLightState = 4;
    private int activeTempState = 4;

    [Space]
    [Header("Test Area")]
    [SerializeField] private GameObject testPlant;
    private Plant plantDBTest;

    [Space]
    [Header("Serial Port to Arduino")]
    private SerialPort sp;
    private string arduinoByte = "";

    public GoToScreen GoTo { get => _goToScreen; set => _goToScreen = value; }
    public GameObject Dashboard { get => dashboard; set => dashboard = value; }

    public SerialPort Sp { get => sp; set => sp = value; }

    private void Awake()
    {
        database = GameObject.Find("Firebase").GetComponent<GetPlantData>();
        try
        {
            InstantiateBottom();
            Sp = new SerialPort("COM11", 9600);
            Sp.ReadTimeout = 500;
            Sp.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        try
        {
            StartCoroutine(WaitForDatabase());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /**
     * Method that adds a new plant to the system, instatiating a new item in the dashboard
     */
    public void InstantiateNewPlantItem(PlantItem plant)
    {
        int dashboardItems = Dashboard.transform.childCount;
        GameObject item = Instantiate(prefabDashPlant, Dashboard.transform);

        item.AddComponent<PlantState>();
        item.GetComponent<PlantState>().Kind = plant.Kind;
        item.GetComponent<PlantState>().SetVariables();

        item.GetComponent<PlantItem>().Nickname = plant.Nickname;
        item.GetComponent<PlantItem>().Kind = plant.Kind;
        item.GetComponent<PlantItem>().Manager = plant.Manager;
        item.GetComponent<PlantItem>().Icon = plant.Icon;
        item.transform.GetChild(0).GetComponent<Text>().text = plant.Nickname;
        item.transform.GetChild(1).GetComponent<Image>().sprite = plant.Icon;
        item.name = plant.Nickname;
        //Reorganize the items in Dashboard
        for (int i = 1; i < 4; i++)
        {
            Dashboard.transform.GetChild(dashboardItems - 3).SetAsLastSibling();
        }


    }

    private IEnumerator WaitForDatabase()
    {
        yield return new WaitUntil(() => database.read);
        StartCoroutine(CheckStates());
    }

    /**
     * Creates two empty Items on the dashboard.
     * They are meant to be placed at the end of the dashboard to create more space.
     * */
    private void InstatiateEmptys()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject item = Instantiate(prefabDashPlant, Dashboard.transform);
            item.transform.GetChild(1).GetComponent<Image>().color = Color.clear;
            item.transform.GetChild(0).GetComponent<Text>().text = "";
            item.GetComponent<Image>().color = Color.clear;
        }
    }

    /**
     * Method to add an item to the dashboard that, when pressed, takes
     * the user to the AddPlant screen.
     * */
    private void InstantiateAddPlant()
    {
        GoTo = goToScreen.GetComponent<GoToScreen>();
        GameObject item = Instantiate(prefabDashAddBtn, Dashboard.transform);
        item.GetComponent<Button>().onClick.AddListener(GoTo.GoToAddPlant);
    }

    /**
     * Method that puts the "Add Plant button" and the empty spaces on the dashboard.
     * */
    private void InstantiateBottom()
    {
        InstantiateAddPlant();
        InstatiateEmptys();
    }

    /**
     * Method to eliminate a Plant from the System.
     * When called, it loads the updated dashboard screen.
     * */
    public void DeletePlant(Text nickname)
    {
        Navigation _navi = GoTo.Navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(GoTo.Navi.GetComponent<Navigation>().Panels[0]);
        Destroy(dashboard.transform.Find(nickname.text).gameObject);
    }

    /**
     * Corutine to periodically request the state of the greenhouse conditions
     * and compare it to the Database information of each saved Plant 
     * to determine their appropiate state (Shown by the color).
     */
    public IEnumerator CheckStates()
    {
        int j = 0;
        while (true)
        {
            StartCoroutine(
                AsynchronousReadFromArduino(
                    (string msg) => ParseMessage(msg),
                    () => Debug.Log("Error!"),
                    2000f
                    )
                );
            for (int i = 0; i < dashboard.transform.childCount - 3; i++)
            {
                if (Sp.IsOpen)
                {
                    int test = dashboard.transform.GetChild(i).GetComponent<PlantState>()
                        .RequestStates(temp, light, humid);
                    switch (test)
                    {
                        case 0:
                            dashboard.transform.GetChild(i).GetComponent<Image>().color = new Color32(0x97, 0xBB, 0x8F, 0xFF);
                            break;
                        case 1:
                            dashboard.transform.GetChild(i).GetComponent<Image>().color =
                                new Color32(0xDD, 0xCA, 0x8B, 0xFF);
                            break;
                        case 2:
                            dashboard.transform.GetChild(i).GetComponent<Image>().color =
                                new Color32(0xDE, 0x88, 0x88, 0xFF);
                            break;
                    }
                }
                else
                {
                    dashboard.transform.GetChild(i).GetComponent<Image>().color = new Color32(0xDB, 0x8B, 0xDD, 0xFF);
                }
            }
            //Debug.Log("States checked " + j + " times.");
            //j++;
            yield return new WaitForSeconds(5);
        }
    }

    /**
     * Recursive Corutine to request sensor information from the Arduino.
     * Runs 3 times, one for each kind of sensor.
     */
    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null,
                            float timeout = float.PositiveInfinity,
                            string requisite = "GETLIGHT", string next = "GETTEMP", string last = "GETHUMIDITY")
    {
        //Debug.Log("154 " + requisite + "  Corutine Started");
        if (requisite == null)
        {
            //Debug.Log("Corutine finished ");
            yield break;
        }
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        WriteToArduino(requisite);
        yield return new WaitForSeconds(1);

        //Read From Arduino
        string dataString = null;
        do
        {
            try
            {
                dataString = Sp.ReadLine();
                //Debug.Log(dataString);
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException || ex is InvalidOperationException)
                {
                    Debug.Log("In corutine: Got nothing 175");
                    yield break;
                    //dataString = null;
                }
            }
            
            if (dataString != null)
            {
                //Parse and start next corutine
                callback(dataString);
                StartCoroutine(
                    AsynchronousReadFromArduino(
                        (string msg) => ParseMessage(msg),
                        () => Debug.Log("Error!"),
                        3000f, next, last, null
                        )
                );
                yield break;
            }
            else
                yield return null;
            
            
            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);


        if (fail != null)
            fail();
        yield return null;
    }


    /**
     * Method to send commands to Arduino to turn on/off an equipment.
     * */
    public void SwitchArduinoEquipment(int i)
    {
        //Debug.Log(waterPump.value);

        switch (i)
        {
            case 0:
                if (waterPump.value == 1)
                {
                    WriteToArduino("PUMPUP");
                }
                else
                {
                    WriteToArduino("PUMPDOWN");
                }
                break;
            case 1:
                if (fan.value == 1)
                {
                    WriteToArduino("FANUP");
                }
                else
                {
                    WriteToArduino("FANDOWN");
                }
                break;
            case 2:
                if (ledLamp.value == 1)
                {
                    WriteToArduino("LEDUP");
                }
                else
                {
                    WriteToArduino("LEDDOWN");
                }
                break;
        }
    }

    /**
     * Method to write a message to the Arduino, through the Serial Port
     * */
    private void WriteToArduino(string message)
    {
        try
        {
            Sp.WriteLine(message);
            Sp.BaseStream.Flush();
        }

        catch (Exception e)
        {
            if (e is InvalidOperationException)
            {
                Console.WriteLine(e);
                //More Descriptive Error Message
            }
        }
    }

    /**
     * Method to process a message from arduino. 
     *
     * Format from arduino message: (t,h,l) (###) (0,1)
     * t,h,l: Type of Information
     * ###: Value read by the sensor
     * 0,1: State of the corresponding equipment
     *      0: Off, 1: On
    */
    private void ParseMessage(string message)
    {

        string[] parts = message.Split(' ');
        if (parts.Length != 3) return;
        messageType = parts[0];
        float value = float.Parse(parts[1]);
        switch (messageType)
        {
            case "t":
                Debug.Log(message + "Parsing...");
                equipmentONt = parts[2] == "1";
                temp = value;
                break;
            case "l":
                Debug.Log(message + "Parsing...");
                equipmentONl = parts[2] == "1";
                light = value;
                break;
            case "h":
                Debug.Log(parts[0] + value * .01 + "Parsing...");
                equipmentONh = parts[2] == "1";
                humid = value;
                break;
            default:
                return;
        }
    }

    /**
     * Corutine to Update the state of each of the conditions on a Dashboard_Plant Screen
     * (The screen that details the state of a Plant)
     * Also called periodically, when Dashboar_Plant Screen is enabled.
     */
    public IEnumerator DashboardPlantUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            Debug.Log("Dashboard Plant Update started");
            dashboardPlantScreen.GetComponent<DashboardPlant>().SetState(activePlant);
        }
    }

    /**
     * Sets the activePlant as the nickname of the Plant whose Dashboard_Plant Screen
     * has been open.
     * */
    public void SetActivePlant(string nickname)
    {
        activePlant = nickname;
        activePlantObject = GameObject.Find(nickname).GetComponent<PlantState>();
        activeWaterState = activePlantObject.WaterState;
        activeTempState = activePlantObject.TempState;
        activeLightState = activePlantObject.LightState;
    }

}
