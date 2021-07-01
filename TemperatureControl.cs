using System.Collections.Generic;
using UnityEngine;

//Service that tracks heat sources in the scene and calculates temperature for an object at a given position
public class TemperatureControl : MonoBehaviour, ITemperatureControl
{
    [SerializeField, Header("Grid Settings")]
    Vector3 gridOrigin = new Vector3(-15f, 0.05f, -15f);

    [SerializeField, Min(0f)]
    float cellSize = 6f;

    [SerializeField, Min(0)]
    int numberOfCells = 5, cellCheckRange = 1;

    float ambientTemperature = 20f; //Temperature of the surrounding scene
    public float GetAmbientTemperature()
    {
        return ambientTemperature;
    }
    public void SetAmbientTemperature(float ambientTemperature)
    {
        this.ambientTemperature = ambientTemperature;
    }

    float tempRateOfChange = 0.5f; //Rate at which objects will reach thermodynamic equilibrium
    public float TempRateOfChange()
    {
        return tempRateOfChange;
    }
    public void SetTempRateOfChange(float tempRateOfChange)
    {
        this.tempRateOfChange = tempRateOfChange;
    }

    MyGrid<HeatSource> grid;

    private void Awake()
    {
        ServiceLocator.Provide(this);
        grid = new MyGrid<HeatSource>(gridOrigin, cellSize, numberOfCells);
    }

    //Called by heat sources during OnAwake(), adds them to list of active heat sources in the scene
    public void RegisterHeatSource(HeatSource source)
    {
        Vector2 gridPosition;
        if (grid.GetGridPosition(source.transform.position, out gridPosition))
        {
            List<HeatSource> heatSources = grid.GetCellContents(gridPosition);
            if (!heatSources.Contains(source))
            {
                heatSources.Add(source);
            }
        }
    }

    //Called by heat sources during OnDisable(), removes them from list of active heat sources in the scene
    public void UnregisterHeatSource(HeatSource source)
    {
        Vector2 gridPosition;
        if (grid.GetGridPosition(source.transform.position, out gridPosition))
        {
            List<HeatSource> heatSources = grid.GetCellContents(gridPosition);
            if (heatSources.Contains(source))
            {
                heatSources.Remove(source);
            }
        }
    }

    public float GetTemperature(Vector3 position, float currentTemp)
    {
        Vector2 monitorGridPosition = new Vector2();
        float temperature = ambientTemperature;

        //Check if temperature montitor is on the grid, if it is convert world position to grid position
        if (grid.GetGridPosition(position, out monitorGridPosition))
        {
            float targetTemperatureSum = ambientTemperature;
            float numberOfSources = 1f;

            //Check the cell that the monitor is in to start and the surrounding cells up to the set range
            for(int i = -cellCheckRange; i <= cellCheckRange; i++)
            {
                for (int j = -cellCheckRange; j <= cellCheckRange; j++)
                {
                    Vector2 sourceGridPosition = new Vector2(monitorGridPosition.x + i, monitorGridPosition.y + j);
                    if(sourceGridPosition.x >= 0 && sourceGridPosition.x < numberOfCells && sourceGridPosition.y >= 0 && sourceGridPosition.y < numberOfCells)
                    {
                        List<HeatSource> heatSources = grid.GetCellContents(sourceGridPosition);
                        foreach (HeatSource source in heatSources)
                        {
                            float contributionRatio = 1f;
                            float tempContribution = source.GetTemperatureContribution(position, out contributionRatio);
                            if (tempContribution != 0f)
                            {
                                targetTemperatureSum += tempContribution;
                                //numberOfSources += 1;
                                numberOfSources += contributionRatio;
                            }
                        }
                    }
                }
            }

            //Calculate final target and set temperature
            float targetTemperature = targetTemperatureSum / numberOfSources;
            temperature = Mathf.Lerp(currentTemp, targetTemperature, Time.deltaTime * tempRateOfChange);
        }
        return temperature;
    }

    private void OnDrawGizmos()
    {
        //Set grid colour
        Gizmos.color = Color.red;

        //Draw rows
        for (int i = 0; i < numberOfCells + 1; i++)
        {
            Vector3 leftEdge = new Vector3(gridOrigin.x, gridOrigin.y, gridOrigin.z + (cellSize * i));
            Vector3 rightEdge = new Vector3(gridOrigin.x + (cellSize * numberOfCells), gridOrigin.y, gridOrigin.z + (cellSize * i));
            Gizmos.DrawLine(leftEdge, rightEdge);
        }

        //Draw columns
        for (int i = 0; i < numberOfCells + 1; i++)
        {
            Vector3 bottomEdge = new Vector3(gridOrigin.x + (cellSize * i), gridOrigin.y, gridOrigin.z);
            Vector3 topEdge = new Vector3(gridOrigin.x + (cellSize * i), gridOrigin.y, gridOrigin.z + (cellSize * numberOfCells));
            Gizmos.DrawLine(bottomEdge, topEdge);
        }
    }
}
