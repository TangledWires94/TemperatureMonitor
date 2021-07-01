using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

//Allows users to configure and place heat source prefabs into the scene with left click
public class HeatSourcePlacement : MonoBehaviour
{
    [SerializeField]
    HeatSource placeableObject = default;

    [SerializeField]
    List<HeatSource> heatSourcePrefabs = new List<HeatSource>();

    [SerializeField]
    TMP_Dropdown heatSourceTypeInput = default;

    [SerializeField]
    TMP_InputField tempInput = default;

    [SerializeField]
    Slider rangeInput = default, rangeFalloffInput = default, angleInput = default;

    [SerializeField]
    TextMeshProUGUI rangeText = default, rangeFalloffText = default, angleText = default;

    [SerializeField]
    GameObject rangeSettingsContainer = default, angleSettingsContainer = default;

    enum HeatSourceType { Radius, Collider};
    HeatSourceType heatSourceType = HeatSourceType.Radius;
    Camera mainCamera;
    float sourceTemperature = 50f, heatRange = 1.5f, heatFalloffRange = 3.5f, angle = 0f;

    void Awake()
    {
        mainCamera = Camera.main;

        //Set the options of the dropdown menu to the values set in the enum, guarantees that when enum values change dropdown options still match
        heatSourceTypeInput.onValueChanged.AddListener(delegate { HeatSourceTypeChange(); });
        heatSourceTypeInput.ClearOptions();
        List<string> strings = new List<string>();
        foreach(string value in Enum.GetNames(typeof(HeatSourceType)))
        {
            strings.Add(value);
        }
        heatSourceTypeInput.AddOptions(strings);

        //Create delegate functions to update variables when UI values change
        tempInput.onValueChanged.AddListener(delegate 
        { 
            float.TryParse(tempInput.text, out sourceTemperature); 
        });

        rangeInput.onValueChanged.AddListener(delegate 
        {
            heatRange = rangeInput.value;
            rangeText.text = "Range = " + heatRange.ToString("0.0");
            if (heatRange > heatFalloffRange)
            {
                heatFalloffRange = heatRange;
                rangeFalloffInput.value = heatFalloffRange;
                rangeFalloffText.text = "Range Falloff = " + heatFalloffRange.ToString("0.0");
            }
        });

        rangeFalloffInput.onValueChanged.AddListener(delegate 
        { 
            heatFalloffRange = rangeFalloffInput.value;
            rangeFalloffText.text = "Range Falloff = " + heatFalloffRange.ToString("0.0");
            if (heatFalloffRange < heatRange)
            {
                heatRange = heatFalloffRange;
                rangeInput.value = heatRange;
                rangeText.text = "Range = " + heatRange.ToString("0.0");
            }
        });

        angleInput.onValueChanged.AddListener(delegate 
        { 
            angle = angleInput.value;
            angleText.text = "Angle = " + angle.ToString("0");
        });

        //Initialise values
        float.TryParse(tempInput.text, out sourceTemperature);
        heatRange = rangeInput.value;
        heatFalloffRange = rangeFalloffInput.value;
        angle = angleInput.value;

        //Initialise UI
        HeatSourceTypeChange();
    }

    void Update()
    {
        //If left mouse clicked on a surface spawn new heat source on that surface 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << 8; //Layer 8 = PlaceableSurface
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Vector3 spawnPoint = hit.point + (hit.normal * (placeableObject.transform.localScale.y * 0.5f));
                Debug.Log(hit.point + (hit.normal * (placeableObject.transform.localScale.y * 0.5f)));
                HeatSource sourceObject = Instantiate(placeableObject, spawnPoint, spawnRotation);
                sourceObject.InitialiseHeatSource(sourceTemperature, heatRange, heatFalloffRange, angle);
            }
        }
    }

    //When heat source drop down menu changes, update the selected heat source type variable and change UI to show correct settings
    void HeatSourceTypeChange()
    {
        heatSourceType = (HeatSourceType)heatSourceTypeInput.value;
        placeableObject = heatSourcePrefabs[heatSourceTypeInput.value];
        switch (heatSourceType)
        {
            case HeatSourceType.Radius:
                rangeSettingsContainer.gameObject.SetActive(true);
                angleSettingsContainer.gameObject.SetActive(false);
                rangeText.text = "Range = " + heatRange.ToString("0.0");
                rangeFalloffText.text = "Range Falloff = " + heatFalloffRange.ToString("0.0");
                break;
            case HeatSourceType.Collider:
                rangeSettingsContainer.gameObject.SetActive(false);
                angleSettingsContainer.gameObject.SetActive(true);
                angleText.text = "Angle = " + angle.ToString("0");
                break;
            default:
                break;
        }
    }
}
