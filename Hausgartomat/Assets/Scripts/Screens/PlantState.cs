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
    }

    //States provided by arduino :D 
    //Getters for States information
    /*
     Needs a Port and connection to the arduino :D*/



    // Start is called before the first frame update
    void Start()
    {
        //Get info from DB for this plant
    }

    public int[] RequestStates()
    {
        int[][] values = RequestLimitsFromDB();
        return new int[] { 1, 2, 1 };
    }

    public int[][] RequestLimitsFromDB()
    {
        //Min Max Values for each parameter. Light, Water, Temperature
        return new int[][] { new int[] { 10, 20 }, new int[] { 30, 40 }, new int[] { 50, 60 } };
    } 
}
