using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField flowField = null;

    public void InitFlowField()
    {
        flowField = new FlowField(cellRadius, gridSize);
        flowField.CreateGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Making Grid!");
            InitFlowField();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (flowField != null)
        {
            foreach (Cell c in flowField.grid)
            {
                Gizmos.DrawWireCube(c.worldPos, Vector3.one * flowField.cellRadius * 2);
            }

        }

    }
}
