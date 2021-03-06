using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class organizes how the application works with the DB and Arduino.
 * Makes each plantItem in the Dashboard check it?s state with arduino periodically
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

    public GoToScreen GoTo { get => _goToScreen; set => _goToScreen = value; }
    public GameObject Dashboard { get => dashboard; set => dashboard = value; }

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        InstantiateBottom();

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
    {

        //Pedir los tres numeros aqui, con una separacion de 1 segundo entre cada una y usar esos pa comparar todos
        int j = 0;
        // Check States every X seconds for all plant items  
        while (j < 25)
        {
            //Debug.Log("Checking States");
            for (int i = 0; i < dashboard.transform.childCount - 3; i++)
            {
                //yield return new wait until child is finished checking all its values
                if (!dashboard.transform.GetChild(i).GetComponent<PlantState>().Sp.IsOpen)
                {
                    try
                    {
                        dashboard.transform.GetChild(i).GetComponent<PlantState>().Sp.Open();
                    }
                    catch (IOException)
                    {
                        
                    }
                }
                //Debug.Log("Port is open: " + dashboard.transform.GetChild(i).GetComponent<PlantState>().Sp.IsOpen);

                int test = dashboard.transform.GetChild(i).GetComponent<PlantState>().RequestStates();
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
                dashboard.transform.GetChild(i).GetComponent<PlantState>().Sp.Close();
                yield return new WaitUntil(() => dashboard.transform.GetChild(i).GetComponent<PlantState>().FinishedChecking);
                //yield return new WaitForSeconds(2);
                dashboard.transform.GetChild(i).GetComponent<PlantState>().FinishedChecking = false;
            }
            //Debug.Log("States checked " + j++ + " times.");
            j++;
            yield return new WaitForSeconds(5);
        }
    }
}
