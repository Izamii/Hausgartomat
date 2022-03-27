using UnityEngine;

/**
 * This class gets the current sensor values from the manager
 * to give back the state of a plant.
 */
public class PlantState : MonoBehaviour
{
    private Plant plantDB;

    [SerializeField] private string kind;
    [SerializeField] private int lightState = 0;
    [SerializeField] private int waterState = 0;
    [SerializeField] private int tempState = 0;

    private GetPlantData _getPlantData;
    [SerializeField] private float[] lightVals;
    [SerializeField] private float[] humidityVals;
    [SerializeField] private float[] temperatureVals;
    private float mediocreTemp = 3;
    private float mediocreHumidity = 0.1f;
    private float mediocreLight = 50;


    /**
     * Constructor for PlantState.
     * When created, the value ranges for the specified kind of plant
     * are retrieved from the database.
     * */
    public PlantState(string kind)
    {
        this.Kind = kind;
        _getPlantData = GameObject.Find("Firebase").GetComponent<GetPlantData>();;
        plantDB = _getPlantData.GETSinglePlant(kind);
        this.lightVals = PlantDB.light;
        this.humidityVals = PlantDB.humidity;
        this.temperatureVals = PlantDB.temperature;
    }

    public float[] LightVals { get => lightVals; set => lightVals = value; }
    public int LightState { get => lightState; set => lightState = value; }
    public int WaterState { get => waterState; set => waterState = value; }
    public int TempState { get => tempState; set => tempState = value; }
    public string Kind { get => kind; set => kind = value; }
    public Plant PlantDB { get => plantDB; }

    private void Awake()
    {
        _getPlantData = GameObject.Find("Firebase").GetComponent<GetPlantData>();;
    }

    /**
     * When called, it compares the given values to the plant�s range of values
     * and gives back the individual state of each of the three conditions.
     * */
    public int RequestStates(float valueT, float valueL , float valueH)
    {
        ManageResponse("t", valueT);
        ManageResponse("l", valueL);
        ManageResponse("h", valueH);
        return DetermineState(lightState, waterState, tempState);
    }


    /**
     * Based on the type of message, this method compares the given value with the corresponding
     * paramter of this plant and sets it�s corresponding state to one of the following:
     * Possible states: 0,1 = to low; 2 = good; 3,4 = to high
     * 
     * Type of Information: t,h,l
     * Value read by the sensor: float
    */
    private void ManageResponse(string type, float value)
    {
        //Debug.Log(" PlantState 116:   " +type +  " " + value + "       On: " + gameObject.name);
        float amount = value;
        mediocreLight = 50;
        mediocreHumidity = 0.1f;
        mediocreLight = 50;
        //Debug.Log(type);
        switch (type)
        {
            case "t":
                //Too low, risky
                if (amount < (temperatureVals[0] - mediocreTemp))
                {
                    tempState = 0;
                }
                else
                //Mediocre low
                if (amount >= (temperatureVals[0] - mediocreTemp) && amount < temperatureVals[0])
                {
                    tempState = 1;
                }
                else
                //Good
                if (amount >= temperatureVals[0] && amount < temperatureVals[2])
                {
                    tempState = 2;
                }
                else
                //Mediocre high
                if (amount >= temperatureVals[2] && amount < (temperatureVals[2] + mediocreTemp))
                {
                    tempState = 3;
                }
                else
                //Too High
                if (amount >= (temperatureVals[2] + mediocreTemp))
                {
                    tempState = 4;
                }
                /*Debug.Log("Tempstate: "+ tempState + ". Temp: " + amount + 
                    ". Vals: " + (temperatureVals[0] - mediocreTemp)  + ", " + temperatureVals[0]
                      + ", " + temperatureVals[2] + ", "  + (temperatureVals[2] + mediocreTemp));*/
                break;
            case "h"://150 = 100% 20==0%
                amount *= 0.01f;
                //Too low, risky
                if (amount < humidityVals[0] - mediocreHumidity)
                {
                    waterState = 0;
                }
                else
                //Mediocre low
                if (amount >= humidityVals[0] - mediocreHumidity && amount < humidityVals[0])
                {
                    waterState = 1;
                }
                else
                //Good
                if (amount >= humidityVals[0] && amount < humidityVals[2])
                {
                    waterState = 2;
                }
                else
                //Mediocre high
                if (amount >= humidityVals[2] && amount < humidityVals[2] + mediocreHumidity)
                {
                    waterState = 3;
                }
                else
                //Too High
                if (amount >= humidityVals[2] + mediocreHumidity)
                {
                    waterState = 4;
                }
                /*Debug.Log("Humidstate: " + lightState + ". Wetness: " + amount +
                    ". Vals: " + (humidityVals[0] - mediocreHumidity) + ", " + humidityVals[0]
                    + ", " + humidityVals[2] + ", " + (humidityVals[2] + mediocreHumidity));
               */
                break;
            case "l":
                //Too low, risky
                if (amount < lightVals[0] - mediocreLight)
                {
                    lightState = 0;
                }
                else
                //Mediocre low
                if (amount >= lightVals[0] - mediocreLight && amount < lightVals[0])
                {
                    lightState = 1;
                }
                else
                //Good
                if (amount >= lightVals[0] && amount < lightVals[1])
                {
                    lightState = 2;
                }
                else
                //Mediocre high
                if (amount >= lightVals[1] && amount < lightVals[1] + mediocreLight)
                {
                    lightState = 3;
                }
                else
                //Too High
                if (amount >= lightVals[1] + mediocreLight)
                {
                    lightState = 4;
                }
                 Debug.Log("Lightstate: " + lightState + ". Lightness: " + amount +
                     ". Vals: " + (lightVals[0] - mediocreLight) + ", " + lightVals[0]
                     + ", " + lightVals[1] + ", " + (lightVals[1] + mediocreLight));
                
                break;
        }


    }

    /**
     * The general state of the plant is determined by the worst state.
     * This method weights the state of each condition and returns the general
     * state of the plant.
     * 
     */
    private int DetermineState(int state1, int state2, int state3)
    {
        if (state1 == 0 || state1 == 4 || state2 == 0 || state2 == 4 || state3 == 0 || state3 == 4)
        {
            return 2; //Red, Risky
        }
        else
        if (state1 == 1 || state1 == 3 || state2 == 1 || state2 == 3 || state3 == 1 || state3 == 3)
        {
            return 1; //Yellow, Mediocre
        }
        else
            return 0; //Green, good
    }

    /**
     * This method sets the appropiate value ranges for this plant, 
     * according to the data retrieved from the database for this kind
     * of plant.
     * */
    public void SetVariables()
    {
        _getPlantData = GameObject.Find("Firebase").GetComponent<GetPlantData>();;
        plantDB = _getPlantData.GETSinglePlant(kind);
        this.lightVals = PlantDB.light;
        this.humidityVals = PlantDB.humidity;
        this.temperatureVals = PlantDB.temperature;

    }
}

