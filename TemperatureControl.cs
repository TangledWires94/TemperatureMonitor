using System.Collections.Generic;
using UnityEngine;

public static class TemperatureControl
{
    static List<HeatSource> activeHeatSources = new List<HeatSource>();
    public static float AmbientTemperature
    {
        get { return ambientTemperature; }
        set { ambientTemperature = value; }
    }
    static float ambientTemperature = 20f;

    public static void RegisterHeatSource(HeatSource source)
    {
        if (!activeHeatSources.Contains(source))
        {
            activeHeatSources.Add(source);
        }
    }

    public static void UnregisterHeatSource(HeatSource source)
    {
        if (activeHeatSources.Contains(source))
        {
            activeHeatSources.Remove(source);
        }
    }

    public static float GetTemperature(Vector3 position, float currentTemp)
    {
        float targetTemperatureSum = ambientTemperature;
        float numberOfSources = 1f;
        for (int i = 0; i < activeHeatSources.Count; i++)
        {
            HeatSource source = activeHeatSources[i];
            float tempContribution = 0f;
            float distance = (source.transform.position - position).magnitude;
            if(distance <= source.HeatRange)
            {
                tempContribution = source.SourceTemperature;
                numberOfSources += 1f;
            } else if (distance < source.HeatFalloffRange)
            {
                float tempProportion = 1 - ((distance - source.HeatRange) / (source.HeatFalloffRange - source.HeatRange));
                tempContribution = source.SourceTemperature * tempProportion;
                numberOfSources += 1;
            }
            targetTemperatureSum += tempContribution;
        }
        float targetTemperature = targetTemperatureSum / numberOfSources;
        float temperature = Mathf.Lerp(currentTemp, targetTemperature, Time.deltaTime);
        return temperature;
    }
}
