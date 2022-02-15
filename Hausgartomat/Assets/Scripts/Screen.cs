using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 position;
    private Vector3 middle;
    private bool lerpStarted;
    private bool lerpCalled;

    public float speed = 1f;
    private float startTime;
    private float journeyLength;
    private bool directionOfLerp;
    private bool lerpAway;

    public Screen lastScreen;
    public List<SubScreen> innerScreens;

    void Start()
    {
        directionOfLerp = true;
        position = transform.position;
        middle = new Vector3(0, 0, 0);
        lerpStarted = false;
        lerpCalled = false;
        lerpAway = false;
        // Calculate the journey length.
        journeyLength = Vector3.Distance(position, middle);
        innerScreens = new List<SubScreen>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lerpCalled)
        {
            startTime = Time.time;
            lerpCalled = false;
        }
        if (lerpStarted)
        {
            Lerp(directionOfLerp);
        }
    }

    public void ShowUp()
    {
        lerpCalled = true;
        directionOfLerp = true;
        lerpStarted = true;
    }

    public void GoAway()
    {
        lerpCalled = true;
        directionOfLerp = false;
        lerpStarted = true;
    }

    public void Lerp( bool direction)
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered*100 / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        if (direction)
        {
            if (transform.position == middle)
            {
                lerpStarted = false;
                return;
            }
            transform.position = Vector3.Lerp(position, middle, fractionOfJourney);
            
        }
        else
        {
            if (transform.position == position)
            {
                lerpStarted = false;
                return;
            }
            transform.position = Vector3.Lerp(middle, position, fractionOfJourney);
        }
    }

    public void LastScreen()
    {
        this.GoAway();
        lastScreen.ShowUp();
    }
}
