using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /*private void OnEnable()
    {
        
    }*/
   public void FinishProcess()
    {
        firstScreen.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

}
