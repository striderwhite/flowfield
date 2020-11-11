using UnityEngine;

public class Cell
{
    public Vector3 worldPos { get; private set; }
    public Vector2Int gridPos { get; private set; }
    public byte cost;
    public int integrationCost;
    public bool costWasCalced = false;

    public Cell(Vector3 worldPos, Vector2Int gridPos)
    {
        this.worldPos = worldPos;
        this.gridPos = gridPos;
        cost = 1;
        integrationCost = int.MaxValue;
    }

    public void IncreaseCost(int amount)
    {
        if (cost == byte.MaxValue) { return; }
        if (cost + amount > byte.MaxValue) { cost = byte.MaxValue; }
        else { cost += (byte)amount; }
    }

}