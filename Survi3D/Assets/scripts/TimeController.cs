using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private float secInFullDay = 600f;

    [Range(0, 1)] [SerializeField] private float currTimeDay = 0;
    private float timeMultipl = 1f;
    private float sunInitIntens;

    void Start()
    {
        sunInitIntens = sun.intensity;

    }
    void Update()
    {
        UpdateSun();

        currTimeDay += (Time.deltaTime / secInFullDay) * timeMultipl;

        if (currTimeDay >= 1 )
        {
            currTimeDay = 0;
        }
    }
    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currTimeDay * 360f) - 90, 170, 0);

        float intensMultipl = 1;

        if (currTimeDay <=0.23f || currTimeDay >= 0.75f)
        {
            intensMultipl = 0;
        }
        else if (currTimeDay<=0.25f)
        {
            intensMultipl = Mathf.Clamp01((currTimeDay - 0.23f) * (1 / 0.02f));
        }
        else if (currTimeDay>=0.73f)
        {
            intensMultipl = Mathf.Clamp01(1 - ((currTimeDay - 0.73f) * (1 / 0.02f)));
        }

        sun.intensity = sunInitIntens * intensMultipl;
    }
}
