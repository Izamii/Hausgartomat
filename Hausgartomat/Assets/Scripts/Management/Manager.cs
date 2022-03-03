using System.Collections;
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

    public GoToScreen GoTo { get => _goToScreen; set => _goToScreen = value; }
    public GameObject Dashboard { get => dashboard; set => dashboard = value; }

    // Start is called before the first frame update
    void Start()
    {
        

        
        
    }
    private void Awake()
    {
        InstantiateBottom();
        PlantState s = new PlantState("Mota");
        PlantItem plantyThePlant = new PlantItem(testSprite, "Sandra", "Mota", s);
        InstantiateNewPlantItem(plantyThePlant);
        StartCoroutine(CheckStates());
    }
    //Create
    public void InstantiateNewPlantItem(PlantItem plant)
    {
        int dashboardItems = Dashboard.transform.GetChildCount();
        GameObject item = Instantiate(prefabDashPlant, Dashboard.transform);
        
        item.GetComponent<PlantItem>().Nickname = plant.Nickname;
        item.GetComponent<PlantItem>().Kind = plant.Kind;
        item.GetComponent<PlantItem>().PlantState = plant.PlantState;
        item.GetComponent<PlantItem>().Manager = plant.Manager;
        item.GetComponent<PlantItem>().Icon = plant.Icon;
        item.transform.GetChild(0).GetComponent<Text>().text = plant.Nickname;
        item.transform.GetChild(1).GetComponent<Image>().sprite = plant.Icon;
        item.name = plant.Nickname;
        //Reorganize
        for (int i = 1; i<4 ; i++)
        {
            Dashboard.transform.GetChild(dashboardItems-3).SetAsLastSibling();
        }
    }
    private void InstatiateEmptys()
    {
        for(int i = 0; i < 2; i++)
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

    IEnumerator CheckStates()
    {
        while (true)
        {
            for(int i = 0; i < dashboard.transform.childCount - 3; i++)
            {
                Debug.Log("Checking States");
                Debug.Log(dashboard.transform.GetChild(i).name);
                dashboard.transform.GetChild(i).GetComponent<Image>().color = Random.ColorHSV();
                // Check States every X seconds for all plant items
                Debug.Log("States checked");
            }
            yield return new WaitForSeconds(3);
        }
    }
}
