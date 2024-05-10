using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOconnectorLogic : MonoBehaviour
{
    public static IOconnectorLogic instance;
    private string containedMaterial="";

    public void Awake()
    {
        instance=this;
    }

    public void SetMatereal(string type)
    {
        containedMaterial= type;
        Debug.Log(containedMaterial);
    }
    public void setText()
    {
        Collector.instance.addText(containedMaterial);
    }
}
