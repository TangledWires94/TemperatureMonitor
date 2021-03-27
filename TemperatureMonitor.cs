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

    //When the scene begins assume all objects start at ambient temperature
    void Awake()
    {
        temperature = TemperatureControl.AmbientTemperature;
    }

    void Update()
    {
        temperature = TemperatureControl.GetTemperature(transform.position, temperature);
    }
}
