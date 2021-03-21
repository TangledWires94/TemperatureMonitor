using UnityEngine;
using TMPro;

public class TemperatureMonitor : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI displayText;

    [SerializeField]
    float temperature = 20f;
    public float Temperature
    {
        get { return temperature; }
    }

    [SerializeField]
    float minTemp = 0f, maxTemp = 75f, hotHue = 0f, coldHue = 240f;

    Renderer rend;

    //When the scene begins assume all objects start at ambient temperature
    void Awake()
    {
        temperature = TemperatureControl.AmbientTemperature;
        rend = GetComponent<Renderer>();
    }

    void OnValidate()
    {
        if(maxTemp < minTemp)
        {
            maxTemp = Mathf.Max(maxTemp, minTemp);
        }
        if (coldHue < hotHue)
        {
            maxTemp = Mathf.Max(coldHue, hotHue);
        }
    }

    void Update()
    {
        temperature = TemperatureControl.GetTemperature(transform.position, temperature);
        displayText.text = $"Cube Temp = {temperature.ToString("0.0")} C";
        rend.material.color = TemperatureColor(temperature);
    }

    Color TemperatureColor(float temp)
    {
        float t = temperature / (maxTemp - minTemp);
        float hue = Mathf.Lerp(coldHue, hotHue, t) / 360f;
        return Color.HSVToRGB(hue, 1f, 1f);
    }
}
