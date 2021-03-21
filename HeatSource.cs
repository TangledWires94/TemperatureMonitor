using UnityEngine;

public class HeatSource : MonoBehaviour
{
    [SerializeField, Range(-20f, 100f), Tooltip("Temperature in Degrees Celcius")]
    float sourceTemperature = 50f;

    public float SourceTemperature
    {
        get { return sourceTemperature; }
        set { sourceTemperature = value; }
    }

    [SerializeField, Min(0f)]
    float heatRange = 1f, heatFalloffRange = 5f;

    public float HeatRange
    {
        get { return heatRange; }
        set { heatRange = value; }
    }
    public float HeatFalloffRange
    {
        get { return heatFalloffRange; }
        set { heatFalloffRange = value; }
    }

    void OnValidate()
    {
        heatFalloffRange = Mathf.Max(heatRange, heatFalloffRange);
    }

    void Awake()
    {
        OnValidate();
        TemperatureControl.RegisterHeatSource(this);
    }

    void OnDisable()
    {
        TemperatureControl.UnregisterHeatSource(this);
    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, heatRange);
        if(heatFalloffRange > heatRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center, heatFalloffRange);
        }
    }
}
