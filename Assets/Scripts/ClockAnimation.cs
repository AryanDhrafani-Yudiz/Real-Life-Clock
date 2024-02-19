using UnityEngine;
using System;
using System.Collections;
// Code Tested On Mac Mini 2014 , Unity 2022.3.7f1<OpenGL 4.1> 16:9 Aspect , Avg 65 FPS. Aryan Dhrafani
public class ClockAnimation : MonoBehaviour
{
    [SerializeField] private Transform hourTransform, minuteTransform, secondTransform;
    private float degreesInHour, degreesInMinute, degreesInSecond;
    private float currentHour, currentMinute, currentSecond;
    [SerializeField] private bool runHour, runMinute, runSecond;
    [SerializeField] private bool runSmooth;
    DateTime currentDate;

    private void Start() // Start Method For Initializing Values
    {
        QualitySettings.vSyncCount = 1;
        degreesInHour = 12f;
        degreesInMinute = 60f;
        degreesInSecond = 60f;

        if (runSecond)
        {
            currentSecond = ((currentDate.Second / degreesInSecond) * 360f);
            secondTransform.Rotate(0f, 0f, -currentSecond, Space.Self);
        }
        if (runMinute)
        {
            currentMinute = ((currentDate.Minute / degreesInMinute) * 360f) + currentSecond / 60f;
            minuteTransform.Rotate(0f, 0f, -currentMinute, Space.Self);
        }
        if (runHour)
        {
            currentHour = ((currentDate.Hour / degreesInHour) * 360f) + currentMinute / 60f + currentSecond/60f;
            hourTransform.Rotate(0f, 0f, -currentHour, Space.Self);
        }
        InvokeRepeating("clockTicking", 1f, 1f); // Invoke Repeat clockTicking Method Every Second
    }
   
    private void clockTicking() // Method For Actual Ticking Of All The Hands
    {
        currentDate = DateTime.Now;
        Debug.Log(currentDate);
      
        if (runSecond)
        {
            currentSecond = ((currentDate.Second / degreesInSecond) * 360f);
            if (runSmooth)
            {
                StartCoroutine(LerpFunction(Quaternion.Euler(0f, 0f, -currentSecond), 1));
            }
            else
            { secondTransform.rotation = Quaternion.Euler(0f, 0f, -currentSecond); }
        }
        if (runMinute)
        {
            currentMinute = ((currentDate.Minute / degreesInMinute) * 360f) + currentSecond / 60f;
            minuteTransform.rotation = Quaternion.Euler(0f, 0f, -currentMinute);
        }
        if (runHour)
        {
            currentHour = ((currentDate.Hour / degreesInHour) * 360f) + currentMinute / 60f;
            hourTransform.rotation = Quaternion.Euler(0f, 0f, -currentHour);
        }
    }
    IEnumerator LerpFunction(Quaternion endValue, float duration)  // Coroutine For Smooth Movement Of Seconds Hand
    {
        float time = 0;
        Quaternion startValue = secondTransform.rotation;

        while (time < duration)
        {
            secondTransform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        secondTransform.rotation = endValue;
    }
}
