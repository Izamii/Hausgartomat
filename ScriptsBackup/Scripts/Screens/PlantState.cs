using System;
using System.Collections;
using System.IO;
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
    [SerializeField] private int lightState = 0;
    [SerializeField] private int waterState = 0;
    [SerializeField] private int tempState;
    [SerializeField] private GameObject manager;

    private GetPlantData _getPlantData;
    [SerializeField] private float[] lightVals;
    [SerializeField] private float[] humidityVals;
    [SerializeField] private float[] temperatureVals;
    private float mediocreTemp = 3;
    private float mediocreHumidity = 0.1f;
    private float mediocreLight = 50;

    private SerialPort sp;
    private string arduinoByte = "";

    private bool finishedChecking = false;

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
    public SerialPort Sp { get => sp; set => sp = value; }
    public bool FinishedChecking { get => finishedChecking; set => finishedChecking = value; }

    private void Awake()
    {
        manager = GameObject.Find("Manager");
        _getPlantData = manager.GetComponent<GetPlantData>();
    }


    public int RequestStates()
    {
        
        StartCoroutine(
            AsynchronousReadFromArduino
            (
                (string s) => ManageResponse(s),     // Callback
                () => Debug.LogError("Error!"), // Error callback
                300f                          // Timeout (milliseconds)
            )
        );
        FinishedChecking = true;
        //Sp.Close();
        return //DetermineState(lightState, waterState, tempState);
               //DetermineState(waterState);
            1;
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

    /**
    * Responses: t###, h###, l###
    * States: 0,1 = Low; 2 = good; 3,4 = High
    */
    private void ManageResponse(string response)
    {
        Debug.Log(response + " PlantState 116:   " + gameObject.name );
        string[] split = response.Split(' ');
        string type = split[0];
        float amount = 0;
        int equipmentON = 2;
        try
        {
            amount = float.Parse(split[1]);
            equipmentON = int.Parse(split[2]);
        }
        catch (FormatException)
        {
            Debug.Log("Format Error");
            return;
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("Format Error");
            return;
        }
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
                if (amount >= lightVals[0] && amount < lightVals[0] - 50)
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
                if (amount >= lightVals[2] && amount < lightVals[1] + mediocreLight)
                {
                    lightState = 3;
                }
                else
                //Too High
                if (amount >= lightVals[1] + mediocreLight)
                {
                    lightState = 4;
                }
                /* Debug.Log("Lightstate: " + lightState + ". Lightness: " + amount +
                     ". Vals: " + (lightVals[0] - mediocreLight) + ", " + lightVals[0]
                     + ", " + lightVals[1] + ", " + (lightVals[1] + mediocreLight));
                */
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

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;
        int counter = 0;

        for (int i = 0; i < 3; i++)
        {
            /*if (!Sp.IsOpen)
            {
                try
                {
                    Sp.Open();
                }
                catch (IOException)
                {

                }
            }*/

            switch (i)
            {
                case 0://Gimme light
                    //WriteToArduino("GETLIGHT");
                    WriteToArduino("GETTEMP");
                    break;
                case 1://Gimme Humidity
                    //WriteToArduino("GETHUMIDITY");
                    WriteToArduino("GETTEMP");
                    break;
                case 2://Gimme Temp
                    WriteToArduino("GETTEMP");
                    break;
                case 3:
                    yield break;
            }
            counter++;
            do
            {
                try
                {
                    dataString = Sp.ReadLine();
                }
                catch (TimeoutException)
                {
                    dataString = null;
                }
                catch (InvalidOperationException)
                {
                    Sp.Open();
                    try
                    {
                        dataString = Sp.ReadLine();
                    }
                    catch (TimeoutException)
                    {
                        dataString = null;
                    }
                }

                if (dataString != null)
                {
                    callback(dataString);
                    if(counter >= 3)
                    {
                        yield break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                    yield return null; // Wait for next frame
                //yield return new WaitUntil(() => true);

                nowTime = DateTime.Now;
                diff = nowTime - initialTime;

            } while (diff.Milliseconds < timeout);
        }

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
            //if (!Sp.IsOpen) Sp.Open();
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
                case 3:
                    yield break;
            }
            yield return new WaitForSeconds(1);
            try
            {
                arduinoByte = Sp.ReadLine();
                Debug.Log(arduinoByte + " " + gameObject.name);
                ManageResponse(arduinoByte);
            }
            catch
            {
                Debug.Log("Error:");
            }

            Sp.Close();
        }
    }

    public void SetVariables()
    {
        manager = GameObject.Find("Manager");
        _getPlantData = manager.GetComponent<GetPlantData>();
        plantDB = _getPlantData.GETSinglePlant(kind);
        this.lightVals = plantDB.light;
        this.humidityVals = plantDB.humidity;
        this.temperatureVals = plantDB.temperature;
        Sp = new SerialPort("COM11", 9600);
        Sp.ReadTimeout = 500;
    }
}

