using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubScreen : Screen
{
    public SubScreen next;

    public void GoToNext()
    {
        this.GoAway();
        next.ShowUp();
    }
}
