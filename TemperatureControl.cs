using System.Collections.Generic;
using UnityEngine;

//Static class that tracks heat sources in the scene and calculates temperature for an object at a given position
public static class TemperatureControl
{
    static List<HeatSource> activeHeatSources = new List<HeatSource>();
    public static float AmbientTemperature //Temperature of the surrounding scene
    {
        get { return ambientTemperature; }
        set { ambientTemperature = value; }
    }
    static float ambientTemperature = 20f;

    public static float TempRateOfChange //Rate at which objects will reach thermodynamic equilibrium
    {
        get { return tempRateOfChange; }
        set { tempRateOfChange = value; }
    }
    static float tempRateOfChange = 0.5f;

    //Called by heat sources during OnAwake(), adds them to list of active heat sources in the scene
    public static void RegisterHeatSource(HeatSource source)
    {
        if (!activeHeatSources.Contains(source))
        {
            activeHeatSources.Add(source);
        }
    }

    //Called by heat sources during OnDisable(), removes them from list of active heat sources in the scene
    public static void UnregisterHeatSource(HeatSource source)
    {
        if (activeHeatSources.Contains(source))
        {
            activeHeatSources.Remove(source);
        }
    }

    //Gets temperature contribution from ambient temperature and all heat sources in the scene and finds new temperature at that point in world space based on current temperature
    public static float GetTemperature(Vector3 position, float currentTemp)
    {
        float targetTemperatureSum = ambientTemperature;
        float numberOfSources = 1f;
        for (int i = 0; i < activeHeatSources.Count; i++)
        {
            HeatSource source = activeHeatSources[i];
            float tempContribution = source.GetTemperatureContribution(position);
            if(tempContribution != 0f)
            {
                targetTemperatureSum += tempContribution;
                numberOfSources += 1;
            }
        }
        float targetTemperature = targetTemperatureSum / numberOfSources;
        float temperature = Mathf.Lerp(currentTemp, targetTemperature, Time.deltaTime * tempRateOfChange);
        return temperature;
    }
}
