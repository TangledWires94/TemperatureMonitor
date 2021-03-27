using UnityEngine;

//Base class that other heat sources will derive from, contains the 3 minimum functions that all heat sources must have to work. All methods are virtual to allow children to 
//override with their own implementation if needed.
public class HeatSource : MonoBehaviour
{
    //If child classes override Awake they should always call the base implementation as well
    protected virtual void Awake()
    {
        TemperatureControl.RegisterHeatSource(this);
    }

    protected virtual void OnDisable()
    {
        TemperatureControl.UnregisterHeatSource(this);
    }

    public virtual float GetTemperatureContribution(Vector3 position)
    {
        return 0f;
    }
}
