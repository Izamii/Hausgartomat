using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * <summary>
 * This class helps with the navigation inside the Form to add a new plant.
 * </summary>
 */
public class AddPlantStepBtn : MonoBehaviour
{
    [SerializeField] private GameObject nextScreen;
    [SerializeField] private GameObject firstScreen;

    public void GoToNextStep()
    {
        gameObject.transform.parent.gameObject.GetComponentInParent<AddPlantScript>().LastScreen = this.gameObject.transform.parent.gameObject;
        nextScreen.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

   public void FinishProcess()
    {
        firstScreen.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

}
