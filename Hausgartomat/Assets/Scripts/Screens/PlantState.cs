using System;
using System.Collections;
using System.IO.Ports;
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
    [SerializeField] private int lightState=0;
    [SerializeField] private int waterState=0;
    [SerializeField] private int tempState;
    [SerializeField] private GameObject manager;

    private GetPlantData _getPlantData;
    private float[] lightVals;
    private float[] humidityVals;
    private float[] temperatureVals;
    private float mediocreTemp = 3;
    private float mediocreHumidity = 0.1f;
    private float mediocreLight = 50;

    private SerialPort sp = new SerialPort("COM11", 9600);
    private string arduinoByte = "";

    public PlantState(string kind)
    {
        this.kind = kind;
        manager = GameObject.Find("Manager");
        _getPlantData = manager.GetComponent<GetPlantData>();
        plantDB = _getPlantData.GETSinglePlant(kind);
        this.lightVals = plantDB.light;
        this.humidityVals = plantDB.humidity;
        this.temperatureVals = plantDB.temperature;
        Debug.Log("---------------------------"+ lightVals[2]);
        //Temporal, until value in Tomaten DB is fixed
        if (plantDB.name.Equals("Tomate")) lightVals[2] *= 100;
    }

    public float[] LightVals { get => lightVals; set => lightVals = value; }
    public int LightState { get => lightState; set => lightState = value; }
    public int WaterState { get => waterState; set => waterState = value; }
    public int TempState { get => tempState; set => tempState = value; }

    private void Awake()
    {
        manager = GameObject.Find("Manager");
        _getPlantData = manager.GetComponent<GetPlantData>();
    }

    //States provided by arduino :D 
    //Getters for States information
    /*
     Needs a Port and connection to the arduino :D*/

    public int RequestStates()
    {
        manager = GameObject.Find("Manager");
        _getPlantData = manager.GetComponent<GetPlantData>();
        //Open coms
        sp.Open();
        sp.ReadTimeout = 50;
        //Get values from arduino
        StartCoroutine(CheckStates()); //GOT A NULL REFERENCE EXCEPTION
        //Close coms
        sp.Close();
        return DetermineState(lightState, waterState, tempState);
            //DetermineState(waterState);
    }



    //Send Signal to Arduino to turn on/off an equipment.
    public void SwitchArduinoEquipment(int i)
    {
    }

    private void WriteToArduino(string message)
    {
        sp.WriteLine(message);
        sp.BaseStream.Flush();
    }

    /**
    * Responses: t###, h###, l###
    * States: 0,1 = Low; 2 = good; 3,4 = High
    */
    private void ManageResponse(string response)
    {
        string[] split = response.Split(' ');
        string type = split[0];
        float amount = float.Parse(split[1]);
        int equipmentON = int.Parse(split[2]);
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
                Debug.Log("Tempstate: "+ tempState + ". Temp: " + amount + 
                    ". Vals: " + (temperatureVals[0] - mediocreTemp)  + ", " + temperatureVals[0]
                    + ", " + temperatureVals[2] + ", "  + (temperatureVals[2] + mediocreTemp));
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
                Debug.Log("Humidstate: " + lightState + ". Wetness: " + amount +
                    ". Vals: " + (humidityVals[0] - mediocreHumidity) + ", " + humidityVals[0]
                    + ", " + humidityVals[2] + ", " + (humidityVals[2] + mediocreHumidity));
                break;
            case "l":
                //Too low, risky
                if (amount < lightVals[0] - mediocreLight)
                {
                    lightState = 0;
                }
                else
                //Mediocre low
                if (amount >= lightVals[0] && amount < lightVals[0] - 50)
                {
                    lightState = 1;
                }
                else
                //Good
                if (amount >= lightVals[0] && amount < lightVals[2])
                {
                    lightState = 2;
                }
                else
                //Mediocre high
                if (amount >= lightVals[2] && amount < lightVals[2] + mediocreLight)
                {
                    lightState = 3;
                }
                else
                //Too High
                if (amount >= lightVals[2] + mediocreLight)
                {
                    lightState = 4;
                }
                Debug.Log("Lightstate: " + lightState + ". Lightness: " + amount +
                    ". Vals: " + (lightVals[0] - mediocreLight) + ", " + lightVals[0]
                    + ", " + lightVals[2] + ", " + (lightVals[2] + mediocreLight));
                break;
        }


    }

    /**
     * The general state of the plant is determined by the worst state.
     */
    private int DetermineState( int state1, int state2, int state3)
    {
        if(state1 == 0 || state1 == 4 || state2 == 0 || state2 == 4 || state3 == 0 || state3 == 4)
        {
            return 2; //Red, Risky
        }else
        if (state1 == 1 || state1 == 3 || state2 == 1 || state2 == 3 || state3 == 1 || state3 == 3)
        {
            return 1; //Yellow, Mediocre
        }else
        return 0; //Green, good
    }

    private int DetermineState(int state)
    {
        if(state == 0 || state == 4) { return 2; }
        if(state == 1 || state == 3) { return 1; }
        if(state == 2) { return 0; }
        return 4;
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = sp.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield break; // Terminates the Coroutine
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

    public int testState()
    {
        return 1;
    }

    IEnumerator CheckStates()
    {
        for (int i = 0; i < 3; i++)
        {
            
            switch (i)
            {
                case 0://Gimme light
                    WriteToArduino("GETTEMP");
                    break;
                case 1://Gimme Humidity
                    WriteToArduino("GETHUMIDITY");
                    break;
                case 2://Gimme Temp
                    WriteToArduino("GETTEMP");
                    break;
                case 4:
                    yield break;
            }
            //Debug.Log("Written to Arduino");
            yield return new WaitForSeconds(0.5f);
            try
            {
                arduinoByte = sp.ReadLine();
                Debug.Log(arduinoByte);
                ManageResponse(arduinoByte);
            }
            catch
            {
                Debug.Log("Error");
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

