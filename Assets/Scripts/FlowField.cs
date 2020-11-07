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

                grid[x, y] = new Cell(wPos, new Vector2(x, y));
            }
        }

    }
}