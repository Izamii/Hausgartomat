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
    private float mediocreHumidity = 10;
    private float mediocreLight = 50;

    private SerialPort sp = new SerialPort("COM12", 9600);
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
    }
    
    public void generateParameters(string kind)
    {
        this.kind = kind;
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
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0://Gimme light
                    WriteToArduino("GETLIGHT");
                    break;
                case 1://Gimme Humidity
                    WriteToArduino("GETHUMIDITY");
                    break;
                case 2://Gimme Temp
                    WriteToArduino("GETTEMP");
                    break;
            }
            //Debug.Log("Written to Arduino");
            try
            {
                arduinoByte = sp.ReadLine();
                //Debug.Log(arduinoByte);
                ManageResponse(arduinoByte);
            }
            catch
            {
                Debug.Log("Error");
            }
        }
        //Close coms
        sp.Close();
        return DetermineState(lightState, waterState, tempState);
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
        string type = response.Split(' ')[0];
        float amount = float.Parse(response.Split(' ')[1]);
        //Debug.Log(type);
        switch (type)
        {
            case "t":
                //Too low, risky
                if (amount < MediocreRange(temperatureVals, mediocreTemp)[0])
                {
                    tempState = 0;
                }
                else
                //Mediocre low
                if (amount >= MediocreRange(temperatureVals, mediocreTemp)[0] && amount < temperatureVals[0])
                {
                    tempState = 1;
                }
                else
                //Good
                if (amount >= temperatureVals[0] && amount < temperatureVals[1])
                {
                    tempState = 2;
                }
                else
                //Mediocre high
                if (amount >= temperatureVals[1] && amount < MediocreRange(temperatureVals, mediocreTemp)[1])
                {
                    tempState = 3;
                }
                else
                //Too High
                if (amount >= MediocreRange(temperatureVals, mediocreTemp)[1])
                {
                    tempState = 4;
                }
                Debug.Log("Tempstate: "+tempState + ". Temp: " + amount);
                break;
            case "h":
                //Too low, risky
                if (amount < MediocreRange(humidityVals, mediocreHumidity)[0])
                {
                    waterState = 0;
                }
                else
                //Mediocre low
                if (amount >= MediocreRange(humidityVals, mediocreHumidity)[0] && amount < humidityVals[0])
                {
                    waterState = 1;
                }
                else
                //Good
                if (amount >= humidityVals[0] && amount < humidityVals[1])
                {
                    waterState = 2;
                }
                else
                //Mediocre high
                if (amount >= humidityVals[1] && amount < MediocreRange(humidityVals, mediocreHumidity)[1])
                {
                    waterState = 3;
                }
                else
                //Too High
                if (amount >= MediocreRange(humidityVals, mediocreHumidity)[1])
                {
                    waterState = 4;
                }
                Debug.Log("Humidstate: " + tempState + ". Wetness: " + amount);
                break;
            case "l":
                //Too low, risky
                if (amount < MediocreRange(lightVals, mediocreLight)[0])
                {
                    lightState = 0;
                }
                else
                //Mediocre low
                if (amount >= MediocreRange(lightVals, mediocreLight)[0] && amount < lightVals[0])
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
                if (amount >= lightVals[1] && amount < MediocreRange(lightVals, mediocreLight)[1])
                {
                    lightState = 3;
                }
                else
                //Too High
                if (amount >= MediocreRange(lightVals, mediocreLight)[1])
                {
                    lightState = 4;
                }
                Debug.Log("Lightstate: " + tempState + ". Lightness: " + amount);
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
            return 0; //Red, Risky
        }
        if (state1 == 1 || state1 == 3 || state2 == 1 || state2 == 3 || state3 == 1 || state3 == 3)
        {
            return 1; //Yellow, Mediocre
        }
        return 2; //Green, good
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


    /**
     * Ranges of values that are still OK for the plant, but could be better
     */
    float[] MediocreRange(float[] range, float modifier)
    {
        float[] medRange = range;
        medRange[0] -= modifier;
        medRange[1] += modifier;
        return medRange;
    }

    public int testState()
    {
        return 1;
    }
}

