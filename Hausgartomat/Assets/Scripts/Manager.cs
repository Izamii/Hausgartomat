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

    public float speed = 0.3F;
    private float startTime;
    private float journeyLength;

    private bool lerpStarted = false;

    void Start()
    {
        _planing_Script = planing_Screen.GetComponent<Screen>();
        _addPlant_Script = addPlant_Screen.GetComponent<Screen>();
        _plantpedia_Script = plantPedia_Screen.GetComponent<Screen>();
        _dashboard_Script = dashboard_Screen.GetComponent<Screen>();
        _plantInfo_Script = plantInfo_Screen.GetComponent<Screen>();

    }

    // Move to the target end position.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _planing_Script.ShowUp();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _addPlant_Script.ShowUp();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _plantpedia_Script.ShowUp();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _dashboard_Script.ShowUp();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _plantInfo_Script.ShowUp();
        }

    }
}
