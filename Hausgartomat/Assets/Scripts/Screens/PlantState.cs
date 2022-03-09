using UnityEngine;

/**
 * This class connects the arduino to the
 * database to determine the state of a plant
 * It can also give instructions to the arduino, 
 * to modify the state of a plant.
 * 
 * DB
 * 1. Get DB Information for this plant
 * 
 * A. Each x Seconds 
 * 1. Get arduino info
 * 2. Compare with DB info
 * 3. Determine State
 * 
 * Equipment instructions.
 * 1. Deliver instruction to Arduino
 * 2. Go to A
 */
public class PlantState : MonoBehaviour
{
    private Plant plantDB;

    [SerializeField] private string kind;
    [SerializeField] private int lightState = 0;
    [SerializeField] private int waterState = 0;
    [SerializeField] private int tempState = 0;
    [SerializeField] private GameObject manager;

    private GetPlantData _getPlantData;
    [SerializeField] private float[] lightVals;
    [SerializeField] private float[] humidityVals;
    [SerializeField] private float[] temperatureVals;
    private float mediocreTemp = 3;
    private float mediocreHumidity = 0.1f;
    private float mediocreLight = 50;


    public PlantState(string kind)
    {
        this.Kind = kind;
        manager = GameObject.Find("Manager");
        _getPlantData = manager.GetComponent<GetPlantData>();
        plantDB = _getPlantData.GETSinglePlant(kind);
        this.lightVals = plantDB.light;
        this.humidityVals = plantDB.humidity;
        this.temperatureVals = plantDB.temperature;
    }

    public float[] LightVals { get => lightVals; set => lightVals = value; }
    public int LightState { get => lightState; set => lightState = value; }
    public int WaterState { get => waterState; set => waterState = value; }
    public int TempState { get => tempState; set => tempState = value; }
    public string Kind { get => kind; set => kind = value; }

    private void Awake()
    {
        manager = GameObject.Find("Manager");
        _getPlantData = manager.GetComponent<GetPlantData>();
    }


    public int RequestStates(float valueT, bool equimpentONT
        , float valueL, bool equimpentONL
        , float valueH, bool equimpentONH)
    {
        ManageResponse("t", valueT, equimpentONT);
        ManageResponse("l", valueL, equimpentONL);
        ManageResponse("h", valueH, equimpentONH);
        return DetermineState(lightState, waterState, tempState);
               //DetermineState(waterState);
            //1;
    }





    /**
    * Responses: t### x, h### x, l### x
    * States: 0,1 = Low; 2 = good; 3,4 = High
    */
    private void ManageResponse(string type, float value, bool equimpentON)
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
            case "h":
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

    private int DetermineState(int state)
    {
        if (state == 0 || state == 4) { return 2; }
        if (state == 1 || state == 3) { return 1; }
        if (state == 2) { return 0; }
        return 4;
    }


    public void SetVariables()
    {
        manager = GameObject.Find("Manager");
        _getPlantData = manager.GetComponent<GetPlantData>();
        plantDB = _getPlantData.GETSinglePlant(kind);
        this.lightVals = plantDB.light;
        this.humidityVals = plantDB.humidity;
        this.temperatureVals = plantDB.temperature;

    }
}

