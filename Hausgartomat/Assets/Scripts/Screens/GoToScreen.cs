using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * <summary>
 * Class with multiple navigation methods to open
 * specific screens and provide them with required parameters
 * when necessary.
 * </summary>
 */
public class GoToScreen : MonoBehaviour
{
    private DashboardPlant _dashboardPlant;
    [SerializeField] private GameObject navi;
    [SerializeField] private GameObject[] planes;

    public GameObject Navi { get => navi; set => navi = value; }


    /**
     * <summary>Go to Plant Detail Screen of the specified plant. </summary>
     * <param name="icon"> Image that represents this plant. </param>
     * <param name="kind"> Type of plant to gather data from Database. </param>
     * <param name="nickname"> Name given by the user for this plant. </param>
     * <param name="plantItem"> GameObject that contains more information about this plant. </param>
     * <param name="state"> Component that contains the information about this plant´s state. </param>
     */
    public void GoToPlantScreen(Sprite icon, string nickname, string kind, PlantState state, GameObject plantItem)
    {
        Navigation _navi = Navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(planes[1]);
        _dashboardPlant = planes[1].GetComponent<DashboardPlant>();
        _dashboardPlant.SetScreen(icon, nickname, kind, state, plantItem);
    }

    /**
     * <summary> Open the Add Plant Screen.</summary>
     */
    public void GoToAddPlant()
    {
        Navigation _navi = Navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(planes[2]);
    }

    /**
     * <summary> Open the last screen to the actual one.</summary>
     * <param name="lastScreen">Specific screen that went before the actual one.</param>
     */
    public void GoBack(GameObject lastScreen)
    {
        Navigation _navi = Navi.GetComponent<Navigation>();
        _navi.NavigationBarClick(lastScreen);
    }

    /**
     * <summary> Inner backwards navigation for the Add Plant form. </summary>
     * <param name="lastScreen">Last visited screen from the form</param>
     * <param name="thisScreen">The actual open screen</param>
     * */
    public void GoBackAddPlant(GameObject lastScreen, GameObject thisScreen)
    {
        thisScreen.SetActive(false);
        lastScreen.SetActive(true);
    }
}
