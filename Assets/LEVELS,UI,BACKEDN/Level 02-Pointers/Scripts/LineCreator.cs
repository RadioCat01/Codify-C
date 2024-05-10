using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public Pointer_Container pointer;
    public Pointer_Container ConnectedObjectB;
    public LineRenderer LineRenderer;

    private bool moveableLines;
    private void Awake()
    {
        moveableLines = false;
    }
    
    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        UpdateLinePositions();
    }

    private void UpdateLinePositions()
    {
        if (pointer != null && ConnectedObjectB != null)
        {
            LineRenderer.SetPosition(0, pointer.transform.position);
            LineRenderer.SetPosition(1, ConnectedObjectB.transform.position);
        }
        if (ConnectedObjectB.isStrCpy)
        {
            moveableLines |= true;
        }
    }

    private void Update()
    {
        if(moveableLines)
        {

            LineRenderer.SetPosition(0, pointer.transform.position);
            LineRenderer.SetPosition(1, ConnectedObjectB.transform.position);
        }
    }
}