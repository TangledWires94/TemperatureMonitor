using UnityEngine;

//Radiates temperature ina  sphere around the GameObject. Gives full contribution of temperature within the heatRange and a reducing contribution up to the fall off range
public class RadiusHeatSource : HeatSource
{
    [SerializeField, Range(-20f, 100f), Tooltip("Temperature in Degrees Celcius")]
    float sourceTemperature = 50f; //Temperature of the heat source, maximum contribution it can offer
    public float SourceTemperature
    {
        get { return sourceTemperature; }
        set { sourceTemperature = value; }
    }

    [SerializeField, Min(0f)]
    float heatRange = 1f, heatFalloffRange = 5f; //Defines range at which temperature contribution is full, reduced or none

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

    //Used to make sure that whenever the ranges are changed in the inspector the fall off range is always at least the standard heat range
    public void OnValidate()
    {
        heatFalloffRange = Mathf.Max(heatRange, heatFalloffRange);
    }

    protected override void Awake()
    {
        OnValidate();
        base.Awake();
    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, heatRange);
        if (heatFalloffRange > heatRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center, heatFalloffRange);
        }
    }

    //When temperature monitor is within the heat range it recieves full contribution from the heat source, when its within the falloff range contribution changes by 1/x^2
    //until temperature monitor is outside of the falloff range
    public override float GetTemperatureContribution(Vector3 position)
    {
        float tempContribution = 0f;
        float distance = (transform.position - position).magnitude;
        if (distance <= heatRange)
        {
            tempContribution = sourceTemperature;
        }
        else if (distance < heatFalloffRange)
        {
            float tempProportion = Mathf.Pow(1 - ((distance - heatRange) / (heatFalloffRange - heatRange)), 2); //Reducing by 1/x^2 to give slightly more realistic dropoff
            tempContribution = sourceTemperature * tempProportion;
        }
        return tempContribution;
    }

    //Set heating parameters
    public override void InitialiseHeatSource(float sourceTemperature, float heatRange, float heatFalloffRange, float angle)
    {
        this.sourceTemperature = sourceTemperature;
        this.heatRange = heatRange;
        this.heatFalloffRange = heatFalloffRange;
    }
}
