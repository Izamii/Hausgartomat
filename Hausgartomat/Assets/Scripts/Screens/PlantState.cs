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
    [SerializeField] private int lightState;
    [SerializeField] private int waterState;
    [SerializeField] private int tempState;
    //States provided by arduino :D 
    //Getters for States information

    // Start is called before the first frame update
    void Start()
    {
        //Get info from DB for this plant
    }

    
}
