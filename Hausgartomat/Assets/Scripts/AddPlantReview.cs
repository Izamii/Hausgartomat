using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * <summary>
 * Class responsible for the navigation from the last page of the "add plant" form.
 * </summary>
 */
public class AddPlantReview : MonoBehaviour
{
    [SerializeField] private GameObject plantpedia;
    public void SetUpPlantpediaUtility(Text kind)
    {
        this.transform.GetComponent<PlantpediaDetailUtility>().SetUpUtility(kind.text, this.gameObject, plantpedia);
    }
}
