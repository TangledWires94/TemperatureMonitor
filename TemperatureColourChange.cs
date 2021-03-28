using UnityEngine;
using TMPro;

//Changes the material colour based on the temperature of the TemperatureMonitorClass and updates UI
[RequireComponent(typeof(TemperatureMonitor)), RequireComponent(typeof(Renderer))]
public class TemperatureColourChange : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI displayText = default;

    [SerializeField]
    float minTemp = 0f, maxTemp = 75f, hotHue = 0f, coldHue = 240f;

    Renderer rend;
    TemperatureMonitor tempMonitor;

    void Awake()
    {
        //Don't need to check if it has either of this components because attributes require them
        rend = GetComponent<Renderer>();
        tempMonitor = GetComponent<TemperatureMonitor>();
    }

    //Ensures that if values are changed in the inspector that max values are always greater than or equal to the min values
    void OnValidate()
    {
        maxTemp = Mathf.Max(maxTemp, minTemp);
        coldHue = Mathf.Max(coldHue, hotHue);
    }

    void Update()
    {
        float temperature = tempMonitor.Temperature;
        displayText.text = $"Cube Temp = {temperature.ToString("0.0")} C";
        rend.material.color = TemperatureColor(temperature);
    }

    //Determines new colour based on the current temperature relative the min and max temp, closer temp is to the min temp closer the colour is the min hue
    Color TemperatureColor(float temp)
    {
        float t = temp / (maxTemp - minTemp);
        float hue = Mathf.Lerp(coldHue, hotHue, t) / 360f;
        return Color.HSVToRGB(hue, 1f, 1f);
    }
}
