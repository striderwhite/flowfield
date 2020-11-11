using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 1f;
    public FlowField flowField = null;
    public GameObject gameLand = null;
    public bool displayCube = true;
    public bool displayDirection = true;
    public bool displayCost = true;
    public bool displayGridPosition = false;
    public bool displayIntegrationCost = false;
    private Random rnd = new Random();

    public void InitFlowField()
    {

        //  Deterine the grid size based off the size of the mesh we want to draw the flow field on
        float cellDiameter = cellRadius * 2f;
        float xSize = gameLand.GetComponent<Renderer>().bounds.size.x / cellDiameter;
        float zSize = gameLand.GetComponent<Renderer>().bounds.size.z / cellDiameter;
        gridSize = new Vector2Int((int)xSize, (int)zSize);

        //  Init flow field object
        flowField = new FlowField(cellRadius, gridSize);

        //  Create the actual grid
        flowField.CreateGrid();

        //  Calculate the cost field
        flowField.CalcCostField();

    }

    private void Start()
    {
        print("Making Grid!");
        InitFlowField();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                float cellDiameter = cellRadius * 2f;
                int xPos = (int)(hit.point.x / cellDiameter);
                int yPos = (int)(hit.point.z / cellDiameter);

                Cell touched = flowField.GetCellByGridPosition(new Vector2Int(xPos, yPos));
                if (touched != null)
                {
                    flowField.CalculateIntegrationField(touched);
                    print("Calced integration field");
                }

            }
        }
    }

    private void OnDrawGizmos()
    {

        if (flowField != null)
        {
            foreach (Cell c in flowField.grid)
            {

                //  Draws cell cost
                if (displayCost) { Handles.Label(c.worldPos, c.cost.ToString()); }

                //  Draw cell "size"
                Gizmos.color = Color.red;
                if (displayCube) { Gizmos.DrawWireCube(c.worldPos, Vector3.one * flowField.cellRadius * 2); }

                //  Draw a cell grid position
                if (displayGridPosition)
                {
                    Handles.color = Color.red;
                    string text = c.gridPos.x.ToString() + " " + c.gridPos.y.ToString();
                    Handles.Label(c.worldPos, text);
                }

                //  Draw a cell's integration cost 
                if (displayIntegrationCost) { Handles.Label(c.worldPos, c.integrationCost.ToString()); }

                //  Draw flow field vector direction for this cell
                Gizmos.color = Color.blue;
                if (displayDirection) { DrawArrow.ForGizmo(c.worldPos, Vector3.right); }

            }

        }

    }
}
