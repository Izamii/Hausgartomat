using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Plant plantDB;
    [SerializeField] private string kind;
    [SerializeField] private int lightState;
    [SerializeField] private int waterState;
    [SerializeField] private int tempState;

    public PlantState(string kind)
    {
        this.kind = kind;
        //1 Get Values from DB about this kind of plant
        //2 Provide Arduino with values
        //3 Recive initial state
    }

    //States provided by arduino :D 
    //Getters for States information
    /*
     Needs a Port and connection to the arduino :D*/

    public void SendLimitsToArduino()
    {
        //Send limits for this plant to be tested on Arduino
    }
    public float[] RequestStates()
    {
        float[][] values = RequestLimitsFromDB();
        return new float[] { 1, 2, 1 };
    }

    public float[][] RequestLimitsFromDB()
    {
        SendLimitsToArduino();
        //Min Max Values for each parameter. Light, Water, Temperature
        return new float[][] { new float[] { 10, 20 }, new float[] { 30, 40 }, new float[] { 50, 60 } };
    } 

    //Send Signal to Arduino to turn on/off an equipment.
    public void SwitchArduinoEquipment(int i)
    {
        switch (i)
        {
            case 1: //Water
                break;
            case 2: //Light
                break;
            case 3: //Fan
                break;
        }

    }
}
