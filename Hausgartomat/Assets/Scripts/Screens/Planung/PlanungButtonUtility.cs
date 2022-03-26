using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanungButtonUtility : MonoBehaviour
{
    [SerializeField] private Image buttonHighlight;

    [SerializeField] private GameObject[] buttonList;

    [SerializeField] private GameObject[] contentList;

    public void OnPlanMenuClick()
    {
        if (buttonList.Length != contentList.Length)
        {
            return;
        }
        
        int? index = null;
        for (var i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInChildren<Text>().color = new Color32(0x97, 0xAF, 0x8D, 0xFF);
            contentList[i].SetActive(false);
            if (buttonList[i] == gameObject)
            {
                index = i;
            }
        }

        var transform1 = buttonHighlight.transform;
        transform1.position =
            new Vector3(gameObject.transform.position.x, transform1.position.y);
        gameObject.GetComponentInChildren<Text>().color = new Color32(0x45, 0x61, 0x3B,0xFF); //45613B
        
        if (index != null)
        {
            contentList[(int) index].SetActive(true);  
        }
    }
}
