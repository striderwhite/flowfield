using System;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{

    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }

    private float cellDiameter;

    public FlowField(float cellRadius, Vector2Int gridSize)
    {
        this.cellRadius = cellRadius;
        cellDiameter = cellRadius * 2f;
        this.gridSize = gridSize;
    }

    public void CreateGrid()
    {
        grid = new Cell[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                float offset = cellRadius;

                Vector3 wPos = new Vector3(
                    cellDiameter * x + offset,
                    0,
                    cellDiameter * y + offset);

                Vector2Int gridPos = new Vector2Int(x, y);

                grid[x, y] = new Cell(wPos, gridPos);
            }
        }

    }

    public void CalcCostField()
    {
        int layerMask = LayerMask.GetMask("Impassable", "Rough");
        Vector3 overlayBox = Vector3.one * cellRadius;

        //  Probably better in class definition
        //byte normalCost = 1;
        byte roughCost = 4;
        byte impassableCost = byte.MaxValue;

        //  layers
        int impassableLayer = 8;
        int roughLayer = 9;

        foreach (Cell cell in grid)
        {
            //  Perform Phsyics.OverlapBox check and determine if the box is overlaying with any marked objects
            Collider[] obstacles = Physics.OverlapBox(cell.worldPos, overlayBox, Quaternion.identity, layerMask);

            //  Iterate over any collisions, and mark the current cell to have a corrisponding value
            foreach (Collider collider in obstacles)
            {
                if (cell.costWasCalced) { continue; }

                //  Determine what kind of "surface" this touched
                //  and increase the value based on the cost of the "surface"
                if (collider.gameObject.layer == impassableLayer)
                {
                    cell.IncreaseCost(impassableCost);
                    cell.costWasCalced = true;

                }
                else if (collider.gameObject.layer == roughLayer)
                {
                    cell.IncreaseCost(roughCost);
                    cell.costWasCalced = true;
                }
            }
        }
    }


    public void CalculateIntegrationField(Cell target)
    {
        Queue<Cell> searchQueue = new Queue<Cell>();

        //  1. set the integrationCost of each cell to "highest number" or "infinity" in this case Byte.MaxValue 
        //  and set the target cell to have a cost of 0
        foreach (Cell c in grid)
        {
            c.integrationCost = int.MaxValue;
        }

        target.integrationCost = 0;

        //  2. add the target cell to the current queue

        searchQueue.Enqueue(target);

        Cell currentCell = null;

        while (searchQueue.Count > 0)
        {
            currentCell = searchQueue.Dequeue();

            //  3. get a list of all its neighbours 
            List<Cell> neighbours = GetCellNeighbours(currentCell);

            //  4. now calculate a new integration cost based on the current cell cost plus the cost of the cell in the flow field
            foreach (Cell neighbour in neighbours)
            {
                //  Ignore if its "impassable" terrain
                if (grid[neighbour.gridPos.x, neighbour.gridPos.y].cost == byte.MaxValue) { continue; }

                //  Determine a new integration cost
                int newIntegrationCost = currentCell.integrationCost + grid[neighbour.gridPos.x, neighbour.gridPos.y].cost;

                //  If this newly calculated cost is less than its current cost
                //  set that as the new cost, then add the neighbour to the queue
                if (newIntegrationCost < grid[neighbour.gridPos.x, neighbour.gridPos.y].integrationCost)
                {
                    grid[neighbour.gridPos.x, neighbour.gridPos.y].integrationCost = newIntegrationCost;

                    if (!searchQueue.Contains(grid[neighbour.gridPos.x, neighbour.gridPos.y]))
                    {
                        searchQueue.Enqueue(grid[neighbour.gridPos.x, neighbour.gridPos.y]);
                    }

                }

            }
        }



    }

    public List<Cell> GetCellNeighbours(Cell target)
    {
        List<Cell> neighbours = new List<Cell>();

        int targetX = target.gridPos.x;
        int targetY = target.gridPos.y;

        Cell left = GetCellByGridPosition(
            new Vector2Int(targetX - 1, targetY)
        );

        Cell right = GetCellByGridPosition(
            new Vector2Int(targetX + 1, targetY)
        );

        Cell up = GetCellByGridPosition(
             new Vector2Int(targetX, targetY + 1)
        );

        Cell down = GetCellByGridPosition(
             new Vector2Int(targetX, targetY - 1)
        );

        if (left != null) { neighbours.Add(left); }
        if (right != null) { neighbours.Add(right); }
        if (up != null) { neighbours.Add(up); }
        if (down != null) { neighbours.Add(down); }

        return neighbours;
    }

    public Cell GetCellByGridPosition(Vector2Int gridPos)
    {
        try
        {
            return grid[gridPos.x, gridPos.y];
        }
        catch (Exception)
        {
            return null;
        }

    }

}