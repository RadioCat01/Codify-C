using System.Collections.Generic;
using TMPro;
using System.Text;
using UnityEngine;
using System.Collections;
public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; // Singleton access

    private List<InventoryObject> droppedObjects; // List of dropped objects
    private InventoryObject selectedObject;

    public TMP_Text drop;
    public TMP_Text connect;

    public GameObject LineGameObject;

    private string textToDisplay;
    private string output = "include<stdio.h>"+"\n"+ "public void main(){"+"\n"+ "printf ( \" hello world \" )";

    void Update()
    {
        if (connect.text != output)
        {
            if (Input.GetMouseButtonDown(0)) // Check for left mouse click
            {
                RaycastHit2D[] allHits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                foreach (RaycastHit2D hit in allHits)
                {
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("board"))
                    {
                        foreach (RaycastHit2D hit2 in allHits)
                        {
                            InventoryObject clickedObject = hit2.collider.gameObject.GetComponent<InventoryObject>();
                            if (clickedObject != null)
                            {
                                BoardManager.Instance.OnObjectClicked(clickedObject);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("WIN WIN");
        }
    }

   
    void Awake()
    {
        Instance = this;
        droppedObjects = new List<InventoryObject>();
        connect.text = null;

    }

    public void OnObjectDropped(InventoryObject objectDropped)
    {
        if (drop.text == null)
        {
            drop.text=objectDropped.text;
        }
        else
        {
            drop.text = drop.text + "\n" + objectDropped.text;
        }
       

    }

    


    // Function to handle clicks on objects for connection logic (unused in this example)
    public void OnObjectClicked(InventoryObject clickedObject)
    {

        // Implement logic to handle clicked object and create connections
        if (selectedObject == null) // Check if this is the first click
        {
            selectedObject = clickedObject; // Store the first clicked object

        }
        else if (selectedObject != clickedObject) // Check if it's a different object
        {
            // Check for connection between selectedObject and clickedObject
            CheckConnectionOnClicka(selectedObject, clickedObject);

            selectedObject = null; // Clear selection for next click
        }
    }
    private void CheckConnectionOnClicka(InventoryObject object1, InventoryObject object2)
    {
        InventoryObject notInList = CheckConnectionOnClick(object1, object2, droppedObjects);
        Debug.Log(notInList);

        if (connect.text==null)
        { 
            textToDisplay =  object2.text + "\n" + object1.text ;
            StartCoroutine(TypeText(textToDisplay));

        }
        else
        {
           
            textToDisplay = "\n" + notInList.text;
            StartCoroutine(TypeText(textToDisplay));
            
        }

      }

    private IEnumerator TypeText(string fullText)
    {
        foreach (char character in fullText)
        {
            connect.text += character;
            yield return new WaitForSeconds(0.02f);
        }
        
    }



    private InventoryObject CheckConnectionOnClick(InventoryObject object1, InventoryObject object2, List<InventoryObject> list)
    {
        InventoryObject notInList = null;

        // Check if object1 is not in the list
        if (!list.Contains(object1))
        {
            notInList = object1;
        }
        // If notInList is not found yet, check object2
        else if (!list.Contains(object2))
        {
            notInList = object2;
        }

        

        CreateConnectionLine(object1, object2);

        return notInList;
    }

    


    public void CreateConnectionLine(InventoryObject objectA, InventoryObject objectB)
    {
        ConnectionLine line = Instantiate(LineGameObject).GetComponent<ConnectionLine>();

        line.ConnectedObjectA = objectA;
        line.ConnectedObjectB = objectB;

    }

    public void ClearLevel()
    {      
        connect.text = "";
        drop.text = "";
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("line");
        foreach (GameObject toDestroy2 in toDestroy)
        {
            Destroy(toDestroy2);
        }

       InventoryObject ob1= GameObject.Find("OB1").GetComponent<InventoryObject>();
       ob1.clear();
        InventoryObject ob2 = GameObject.Find("OB2").GetComponent<InventoryObject>();
        ob2.clear();
        InventoryObject ob3 = GameObject.Find("OB3").GetComponent<InventoryObject>();
        ob3.clear();
        InventoryObject ob4 = GameObject.Find("OB4").GetComponent<InventoryObject>();
        ob4.clear();
    }

}