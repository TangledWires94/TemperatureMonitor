using UnityEngine;

//Interface to define the service part of the Temperature Control service locator pattern
public interface ITemperatureControl
{
    void RegisterHeatSource(HeatSource source);
    void UnregisterHeatSource(HeatSource source);
    float GetTemperature(Vector3 position, float currentTemp);
    float GetAmbientTemperature();
    void SetAmbientTemperature(float ambientTemperature);
    float TempRateOfChange();
    void SetTempRateOfChange(float tempRateOfChange);
}
