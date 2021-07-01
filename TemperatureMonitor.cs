using UnityEngine;

//Component attached to GameObjects to allow them to monitor their temperature based on the heat sources in the scene
public class TemperatureMonitor : MonoBehaviour
{
    [SerializeField]
    float temperature = 20f;

    public float Temperature
    {
        get { return temperature; }
    }

    [SerializeField]
    Grid grid;

    //When the scene begins assume all objects start at ambient temperature
    void Start()
    {
        try
        {
            temperature = ServiceLocator.GetTemperatureControl().GetAmbientTemperature();
        }
        catch (System.NullReferenceException e)
        {
            //Service was not available when the initial temperature was requested so can't set the starting temp
            string message = this.name + " could not intitalse temperature, Temperature Control reference not available in time";
            throw new ServiceLocator.ServiceNotFoundException(message, e);
        }
    }

    void Update()
    {
        try
        {
            temperature = ServiceLocator.GetTemperatureControl().GetTemperature(transform.position, temperature);
        }
        catch (System.NullReferenceException e)
        {
            //Service isn't available for some reason
            string message = this.name + " cannot find Temperature Control reference.";
            throw new ServiceLocator.ServiceNotFoundException(message, e);
        }
    }
}
