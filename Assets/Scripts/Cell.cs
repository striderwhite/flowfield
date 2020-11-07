using UnityEngine;

public class Cell
{
    public Vector3 worldPos { get; private set; }
    public Vector2 gridPos { get; private set; }

    public Cell(Vector3 worldPos, Vector2 gridPos)
    {
        this.worldPos = worldPos;
        this.gridPos = gridPos;
    }
}