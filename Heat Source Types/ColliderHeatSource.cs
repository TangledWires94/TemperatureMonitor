using UnityEngine;

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
    Mesh mesh = null;

    void OnValidate()
    {
        col = GetComponent<Collider>();
        MeshCollider mc = GetComponent<MeshCollider>();
        if (mc != null)
        {
            mesh = mc.sharedMesh;
        }
    }

    protected override void Awake()
    {
        OnValidate();
        base.Awake();
    }


    //Currently only draw rectangular bounds but will look into changing for the future to more accurately draw other collider shapes
    void OnDrawGizmos()
    {
        Bounds bounds = col.bounds;
        Vector3 center = bounds.center;
        Quaternion rotation = transform.rotation;
        Gizmos.color = Color.green;
        /*
        if(mesh != null)
        {
            Gizmos.DrawWireMesh(mesh, center, rotation);
        }
        else
        {*/
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            //Bottom rectangle
            Gizmos.DrawLine(min, new Vector3(min.x, min.y, max.z));
            Gizmos.DrawLine(min, new Vector3(max.x, min.y, min.z));
            Gizmos.DrawLine(new Vector3(max.x, min.y, max.z), new Vector3(min.x, min.y, max.z));
            Gizmos.DrawLine(new Vector3(max.x, min.y, max.z), new Vector3(max.x, min.y, min.z));

            //Top rectangle
            Gizmos.DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(min.x, max.y, max.z));
            Gizmos.DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(max.x, max.y, min.z));
            Gizmos.DrawLine(new Vector3(max.x, max.y, max.z), new Vector3(min.x, max.y, max.z));
            Gizmos.DrawLine(new Vector3(max.x, max.y, max.z), new Vector3(max.x, max.y, min.z));

            //Vertical lines
            Gizmos.DrawLine(min, new Vector3(min.x, max.y, min.z));
            Gizmos.DrawLine(new Vector3(max.x, min.y, max.z), new Vector3(max.x, max.y, max.z));
            Gizmos.DrawLine(new Vector3(min.x, min.y, max.z), new Vector3(min.x, max.y, max.z));
            Gizmos.DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z));
        //}

    }


    //If the point is within the bounds of the collider it recieves the full contribution, otherwise no contibution
    public override float GetTemperatureContribution(Vector3 position)
    {
        if (col.bounds.Contains(position))
        {
            return sourceTemperature;
        } else
        {
            return 0f;
        }
    }
}
