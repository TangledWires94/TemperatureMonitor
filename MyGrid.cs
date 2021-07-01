using UnityEngine;
using System.Collections.Generic;

//Class to create a 2D grid of elements of type T, 2D array of lists with each list defining the elements in the cell at the given XY co-ordinate
public class MyGrid<T>
{
    Vector3 gridOrigin; //Position of bottom left corner of the grid
    float cellSize;     //Width and height of each cell in units
    int numberOfCells;  //Number of total cells
    List<T>[,] gridObjects = new List<T>[0, 0]; //2D array of lists containing the elements in each cell

    //Constructor function
    public MyGrid(Vector3 gridOrigin = new Vector3(), float cellSize = 1f, int numberOfCells = 1)
    {
        this.gridOrigin = gridOrigin;

        //Grid must have at least one cell that has a size greater than zero to exist
        if(cellSize <= 0)
        {
            this.cellSize = 1f;
            Debug.Log("Cell size too small, must be greater than 0!");
        }
        else
        {
            this.cellSize = cellSize;
        }

        if (numberOfCells <= 0)
        {
            this.numberOfCells = 1;
            Debug.Log("Number of cells too small, must be greater than 0!");
        }
        else
        {
            this.numberOfCells = numberOfCells;
        }

        //Initialise grid elements as array of defined dimensions with each element being an empty list of type T
        gridObjects = new List<T>[numberOfCells, numberOfCells];
        for(int i = 0; i < numberOfCells; i++)
        {
            for(int j = 0; j < numberOfCells; j++)
            {
                gridObjects[i, j] = new List<T>();
            }
        }
    }

    //Get the cell contents at the requested grid co-ordinate, if the co-ordinates are outside of the bounds of the grid return an empty list
    public List<T> GetCellContents(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < gridObjects.GetLength(0) && y < gridObjects.GetLength(1))
        {
            return gridObjects[x, y];
        } else
        {
            return new List<T>();
        }
    }

    //Override for GetCellContents that allows the use of a vector2 value instead of individual x and y integers
    public List<T> GetCellContents(Vector2 gridIndex)
    {
        int x = (int)gridIndex.x;
        int y = (int)gridIndex.y;
        return GetCellContents(x, y);
    }

    //Calculate the grid position of a given world position, returns true if the position is within the bounds of the grid
    public bool GetGridPosition(Vector3 position, out Vector2 gridIndex)
    {
        int xIndex = 0, yIndex = 0;
        bool validPosition = false;

        if(position.x < gridOrigin.x | position.z < gridOrigin.z)
        {
            validPosition = false;
        } else
        {
            validPosition = true;
            xIndex = (int)((position.x - gridOrigin.x) / cellSize);
            yIndex = (int)((position.z - gridOrigin.z)/ cellSize);
        }

        gridIndex = new Vector2(xIndex, yIndex);
        return validPosition;
    }
}
