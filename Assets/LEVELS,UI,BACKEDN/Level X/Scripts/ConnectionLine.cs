using UnityEngine;

public class ConnectionLine : MonoBehaviour
{
    public InventoryObject ConnectedObjectA;
    public InventoryObject ConnectedObjectB;

    public LineRenderer LineRenderer;

    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        UpdateLinePositions();
    }

    private void UpdateLinePositions()
    {
        if (ConnectedObjectA != null && ConnectedObjectB != null)
        {
            LineRenderer.SetPosition(0, ConnectedObjectA.transform.position);
            LineRenderer.SetPosition(1, ConnectedObjectB.transform.position);
        }
    }

    void Update()
    {  
          LineRenderer.SetPosition(0, ConnectedObjectA.transform.position);
          LineRenderer.SetPosition(1, ConnectedObjectB.transform.position);
                

            
    }
}
