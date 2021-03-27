using UnityEngine;
using TMPro;

public class HeatSourcePlacement : MonoBehaviour
{
    [SerializeField]
    RadiusHeatSource placeableObject;

    [SerializeField]
    TMP_InputField tempInput, rangeInput, rangeFalloffInput;

    Camera mainCamera;
    float sourceTemperature = 50f, heatRange = 1.5f, heatFalloffRange = 3.5f;

    void Awake()
    {
        mainCamera = Camera.main;
        tempInput.onValueChanged.AddListener(delegate { TempInputChanged(); });
        rangeInput.onValueChanged.AddListener(delegate { RangeInputChanged(); });
        rangeFalloffInput.onValueChanged.AddListener(delegate { RangeFalloffInputChanged(); });
        sourceTemperature = float.Parse(tempInput.text);
        heatRange = float.Parse(rangeInput.text);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << 8;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Vector3 spawnPoint = hit.point + (hit.normal * (placeableObject.transform.localScale.y * 0.5f));
                Debug.Log(hit.point + (hit.normal * (placeableObject.transform.localScale.y * 0.5f)));
                RadiusHeatSource sourceObject = Instantiate(placeableObject, spawnPoint, spawnRotation);
                sourceObject.SourceTemperature = sourceTemperature;
                sourceObject.HeatRange = heatRange;
                sourceObject.HeatFalloffRange = heatFalloffRange;
                Debug.LogFormat("New Heat Source: Temp = {0}, Range = {1}, Falloff = {2}", sourceTemperature, heatRange, heatFalloffRange);
            }
        }
    }

    void TempInputChanged()
    {
        sourceTemperature = float.Parse(tempInput.text);
    }

    void RangeInputChanged()
    {
        heatRange = float.Parse(rangeInput.text);
    }

    void RangeFalloffInputChanged()
    {
        heatFalloffRange = float.Parse(rangeFalloffInput.text);
    }
}
