using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int limit = 60;

    public float seconds, minutes, hours;

    void FixedUpdate()
    {
        seconds += Time.deltaTime;
        if(seconds >= limit)
        {
            seconds = 0;
            minutes++;
        }
        if(minutes >= limit)
        {
            minutes = 0;
            hours++;
        }        
    }
}
