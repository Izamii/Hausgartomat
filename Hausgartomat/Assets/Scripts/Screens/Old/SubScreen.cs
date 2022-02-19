using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubScreen : Screen
{
    [SerializeField] private SubScreen next;

    public void GoToNext()
    {
        this.GoAway();
        next.ShowUp();
    }
}
