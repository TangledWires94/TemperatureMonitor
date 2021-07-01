using UnityEngine;

//Child class of HeatSource, contributes to the temperature of any temperature monitor components within a space defined by a collider
[RequireComponent(typeof(Collider))]
public class ColliderHeatSource : HeatSource
{
    [SerializeField, Range(-100f, 100f), Tooltip("Temperature in Degrees Celcius")]
    float sourceTemperature = 50f; //Temperature of the heat source
    public float SourceTemperature
    {
        get { return sourceTemperature; }
        set { sourceTemperature = value; }
    }

    Collider col = null;

    void OnValidate()
    {
        col = GetComponent<Collider>();
    }

    protected override void Awake()
    {
        OnValidate();
        base.Awake();
    }

    //If the point is within the bounds of the collider it recieves the full contribution, otherwise no contibution
    public override float GetTemperatureContribution(Vector3 position, out float contributionRatio)
    {
        if (col.bounds.Contains(position))
        {
            contributionRatio = 1f;
            return sourceTemperature;
        } 
        else
        {
            contributionRatio = 0f;
            return 0f;
        }
    }

    public override void InitialiseHeatSource(float sourceTemperature, float heatRange, float heatFalloffRange, float angle)
    {
        this.sourceTemperature = sourceTemperature;
        Debug.Log(transform.rotation);
        transform.rotation = Quaternion.Euler(-90f, angle, transform.rotation.z);
    }
}
