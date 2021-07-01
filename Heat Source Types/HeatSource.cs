using UnityEngine;

//Base class that other heat sources will derive from, contains the 3 minimum functions that all heat sources must have to work. All methods are virtual to allow children to 
//override with their own implementation if needed.
public class HeatSource : MonoBehaviour
{
    //If child classes override Awake they should always call the base implementation as well
    protected virtual void Awake()
    {
        ServiceLocator.GetTemperatureControl().RegisterHeatSource(this);
    }

    protected virtual void OnDisable()
    {
        ServiceLocator.GetTemperatureControl().UnregisterHeatSource(this);
    }

    public virtual float GetTemperatureContribution(Vector3 position, out float contributionRatio)
    {
        contributionRatio = 1f;
        return 0f;
    }

    public virtual void InitialiseHeatSource(float sourceTemperature, float heatRange, float heatFalloffRange, float angle)
    {
        //No base implementation
    }
}
