using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform planing_Screen;
    public Transform addPlant_Screen;
    public Transform plantPedia_Screen;
    public Transform dashboard_Screen;
    public Transform plantInfo_Screen;

    private Screen _planing_Script;
    private Screen _addPlant_Script;
    private Screen _plantpedia_Script;
    private Screen _dashboard_Script;
    private Screen _plantInfo_Script;
    private Screen[] screens;

    private Screen presentScreen;

    //public List<Plant> plants;

    //Main screen limits
    public const float maxX = 2.8f;
    public const float maxY = 4.3f;


    void Start()
    {
        _planing_Script = planing_Screen.GetComponent<Screen>();
        _addPlant_Script = addPlant_Screen.GetComponent<Screen>();
        _plantpedia_Script = plantPedia_Screen.GetComponent<Screen>();
        _dashboard_Script = dashboard_Screen.GetComponent<Screen>();
        _plantInfo_Script = plantInfo_Screen.GetComponent<Screen>();
        screens = new Screen[] {_planing_Script, _dashboard_Script, 
                                _plantInfo_Script, _addPlant_Script, 
                                _plantpedia_Script};
        presentScreen = _dashboard_Script;
    }
    void Update()
    {

    }

    public void SetActualScreen(int number)
    {
        presentScreen.GoAway();
        presentScreen = screens[number];
        screens[number].ShowUp();
        Screen temp = screens[number];
    }

    public void SetActualScreen(SubScreen subscreen)
    {
        presentScreen.GoAway();
        presentScreen = subscreen;
        subscreen.ShowUp();
    }

    public void GoToLastScreen()
    {
        presentScreen.LastScreen();
    }
}
