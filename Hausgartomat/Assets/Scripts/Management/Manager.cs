using System;
using System.Collections;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class organizes how the application works with the DB and Arduino.
 * Makes each plantItem in the Dashboard check it´s state with arduino periodically
 * It adds,deletes,modifies Plants to the Dashboard.
 * 
 **/
public class Manager : MonoBehaviour
{

    [SerializeField] private GameObject dashboard;
    [SerializeField] private GameObject prefabDashPlant;
    [SerializeField] private GameObject prefabDashAddBtn;
    [SerializeField] private Sprite testSprite;
    [SerializeField] private GameObject goToScreen;
    private GoToScreen _goToScreen;
    [Space]
    [Header("Test Area")]
    [SerializeField] private GameObject testPlant;
    private Plant plantDBTest;
    private SerialPort sp;
    private string arduinoByte = "";

    [SerializeField] private float light = 0;
    [SerializeField] private float temp = 0;
    [SerializeField] private float humid = 0;
    private string messageType = "";
    private bool equipmentONt = false;
    private bool equipmentONl = false;
    private bool equipmentONh = false;

    public GoToScreen GoTo { get => _goToScreen; set => _goToScreen = value; }
    public GameObject Dashboard { get => dashboard; set => dashboard = value; }

    public SerialPort Sp { get => sp; set => sp = value; }

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        InstantiateBottom();
        Sp = new SerialPort("COM11", 9600);
        Sp.ReadTimeout = 500;
        Sp.Open();
        StartCoroutine(CheckStates());
    }
    //Create
    public void InstantiateNewPlantItem(PlantItem plant)
    {
        int dashboardItems = Dashboard.transform.GetChildCount();
        GameObject item = Instantiate(prefabDashPlant, Dashboard.transform);

        item.AddComponent<PlantState>();
        item.GetComponent<PlantState>().Kind = plant.Kind;
        item.GetComponent<PlantState>().SetVariables();

        item.GetComponent<PlantItem>().Nickname = plant.Nickname;
        item.GetComponent<PlantItem>().Kind = plant.Kind;
        //item.GetComponent<PlantItem>().PlantState = plant.PlantState;
        item.GetComponent<PlantItem>().Manager = plant.Manager;
        item.GetComponent<PlantItem>().Icon = plant.Icon;
        item.transform.GetChild(0).GetComponent<Text>().text = plant.Nickname;
        item.transform.GetChild(1).GetComponent<Image>().sprite = plant.Icon;
        item.name = plant.Nickname;
        //Reorganize
        for (int i = 1; i < 4; i++)
        {
            Dashboard.transform.GetChild(dashboardItems - 3).SetAsLastSibling();
        }


    }
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
    private void InstantiateAddPlant()
    {
        GoTo = goToScreen.GetComponent<GoToScreen>();
        GameObject item = Instantiate(prefabDashAddBtn, Dashboard.transform);
        item.GetComponent<Button>().onClick.AddListener(GoTo.GoToAddPlant);
    }
    private void InstantiateBottom()
    {
        InstantiateAddPlant();
        InstatiateEmptys();
    }
    //Delete
    public void DeletePlant(Text nickname)
    {
        Navigation _navi = GoTo.Navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(GoTo.Navi.GetComponent<Navigation>().Panels[0]);
        Destroy(dashboard.transform.Find(nickname.text).gameObject);
    }

    public IEnumerator CheckStates()
    {/*AsynchronousReadFromArduino(Action<string> callback, Action fail = null, 
                            float timeout = float.PositiveInfinity, string requisite = "GETLIGHT", float value = 0f)*/
        //Pedir los tres numeros aqui, con una separacion de 1 segundo entre cada una y usar esos pa comparar todos
        int j = 0;
        while (j < 25)
        {
            StartCoroutine(
                AsynchronousReadFromArduino(
                    (string msg) => ParseMessage(msg),
                    () => Debug.Log("Error!"),
                    3000f
                    )
                );
            //Debug.Log("Checking States");
            for (int i = 0; i < dashboard.transform.childCount - 3; i++)
            {
                int test = dashboard.transform.GetChild(i).GetComponent<PlantState>()
                    .RequestStates(temp, equipmentONt,
                                    light, equipmentONl,
                                    humid, equipmentONh);
                switch (test)
                {
                    case 0:
                        dashboard.transform.GetChild(i).GetComponent<Image>().color = Color.green;
                        break;
                    case 1:
                        dashboard.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
                        break;
                    case 2:
                        dashboard.transform.GetChild(i).GetComponent<Image>().color = Color.red;
                        break;
                }
            }
            //Debug.Log("States checked " + j++ + " times.");
            j++;
            yield return new WaitForSeconds(4);
        }
    }

    //Run recursively 3 Times to avoid collisions
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

        //Write to arduino
        WriteToArduino(requisite);
        yield return new WaitForSeconds(1);
        string dataString = null;
        do
        {
            try
            {
                dataString = Sp.ReadLine();
                //Debug.Log(dataString);
            }
            catch (TimeoutException)
            {
                Debug.Log("In corutine: Got nothing 175");
                dataString = null;
            }

            if (dataString != null)
            {
                //Debug.Log("182 In corutine: " + dataString);
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
                yield return null; // Wait for next frame

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);


        if (fail != null)
            fail();
        yield return null;
    }


    //Send Signal to Arduino to turn on/off an equipment.
    public void SwitchArduinoEquipment(int i)
    {
    }

    private void WriteToArduino(string message)
    {
        Sp.WriteLine(message);
        Sp.BaseStream.Flush();
    }

    private void ParseMessage(string message)
    {
        Debug.Log(message + "Parsing...");
        string[] parts = message.Split(' ');
        if (parts.Length != 3) return;
        messageType = parts[0];
        float value = float.Parse(parts[1]);
        switch (messageType)
        {
            case "t":
                equipmentONt = parts[2] == "1";
                temp = value;
                break;
            case "l":
                equipmentONl = parts[2] == "1";
                light = value;
                break;
            case "h":
                equipmentONh = parts[2] == "1";
                humid = value;
                break;
            default:
                return;
        }
    }

    public IEnumerable DashboardPlantUpdate()
    {
        //On Enable start, on disable stop
        //get this plant´s calculated state, level of values and equipment state and adapt the screen
        //On change of equipment... wait? yeaaaah

        yield return new WaitForSeconds(3);
    }
}
