using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField] private GameObject lastScreen;
    [SerializeField] private GameObject thisScreen;
    [SerializeField] private GameObject manager;
    public void GoBack()
    {
        GoToScreen _goToScreen = manager.GetComponent<GoToScreen>();
        _goToScreen.GoBack(lastScreen, thisScreen);
    }
}
