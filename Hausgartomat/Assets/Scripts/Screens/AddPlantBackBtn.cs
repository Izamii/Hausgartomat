using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>Navigation for the Add Plant form</summary> 
 * 
 */
public class AddPlantBackBtn : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject dashboard;
    [SerializeField] private GameObject screen1;
    [SerializeField] private GameObject screen2;
    [SerializeField] private GameObject screen3;
    private GameObject[] screens;

    /**
     * <summary>Set an array of the relevant screens for the Add Plant form</summary> 
     */
    private void OnEnable()
    {
        screens = new GameObject[4] {dashboard,screen1, screen2, screen3 };

    }
    /**
     * <summary>Backweards navigation for the Add Plant form.
     * If the  actual screen is screen1, then it goes outside from
     * the Add Plant form, back into the dashboard. </summary> 
     */
    public void BackBtn()
    {
        for(int i = 1; i < 4; i++)
        {
            if (screens[i].activeInHierarchy)
            {
                if(screens[i] == screen1)
                {
                    screen1.transform.parent.gameObject.SetActive(false);
                    break;
                }
                screens[i].SetActive(false);
                screens[i - 1].SetActive(true);
                break;
            }
        }
    }
}
