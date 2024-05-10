using System.Collections.Generic;
using TMPro;
using System.Text;
using UnityEngine;
using System.Collections;
public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; 

    private List<InventoryObject> droppedObjects; 
    private InventoryObject selectedObject;
    public TMP_Text drop;
    public TMP_Text connect;
    public GameObject LineGameObject;
    public GameObject Collector;

    
    public bool IsLinesActive;
    public bool isCollectorActive;
    private string textToDisplay;

    public GameObject collector;
    public GameObject bench;
    public List<InventoryObject> inventoryObjects;


    private string output = "";
    public int levelNumber;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsLinesActive) 
        {
            RaycastHit2D[] allHits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach (RaycastHit2D hit in allHits)
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("board"))
                {
                    foreach (RaycastHit2D hit2 in allHits)
                    {
                        InventoryObject clickedObject = hit2.collider.gameObject.GetComponent<InventoryObject>();
                        if (clickedObject != null && clickedObject.CompareTag("InventoryObject"))
                        {
                            Debug.Log("clicked");
                            BoardManager.Instance.OnObjectClicked(clickedObject);
                        }
                    }
                }

            }
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
        if (!droppedObjects.Contains(objectDropped))
        {
           

        }
    }
    public void OnObjectDroppedd(InventoryObject objectDropped, InventoryObject hitObject)
    {
        

        if (connect.text == null)
        {
            textToDisplay = hitObject.text  + objectDropped.text;
            StartCoroutine(TypeText(textToDisplay));
            droppedObjects.Add(objectDropped);
        }
        else
        {
            textToDisplay = "\n" + hitObject.text  + objectDropped.text;
            StartCoroutine(TypeText(textToDisplay));
            droppedObjects.Add(objectDropped);
        }

    }

    public void OnObjectClicked(InventoryObject clickedObject)
    {

      
        if (selectedObject == null) 
        {
            selectedObject = clickedObject; 

        }
        else if (selectedObject != clickedObject) 
        {
            CheckConnectionOnClicka(selectedObject, clickedObject);

            selectedObject = null; 
        }
        else if (selectedObject == clickedObject)
        {
            
            selectedObject = null;
        }
    }
    private void CheckConnectionOnClicka(InventoryObject object1, InventoryObject object2)
    {
        InventoryObject notInList = CheckConnectionOnClick(object1, object2, droppedObjects);
        Debug.Log(notInList);

        if (connect.text == null)
        {
            textToDisplay = object2.text + "\n" + object1.text;
            StartCoroutine(TypeText(textToDisplay));

        }
        else if (notInList != null)
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
        if (levelNumber == 2)
        {
            if (droppedObjects.Count >= 4 && isCollectorActive)
            {
                Collector col = collector.GetComponent<Collector>();
                col.createCollector();
            }
        }
        else if(levelNumber == 3)
        {
            if (droppedObjects.Count >= 5 && isCollectorActive)
            {
                Collector col = collector.GetComponent<Collector>();
                col.createCollector();
            }
        }
    }

    private InventoryObject CheckConnectionOnClick(InventoryObject object1, InventoryObject object2, List<InventoryObject> list)
    {
        InventoryObject notInList = null;


        if (!list.Contains(object1))
        {
            notInList = object1;
        }
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
        droppedObjects.Add(objectA);
        droppedObjects.Add(objectB);
        line.ConnectedObjectA = objectA;
        line.ConnectedObjectB = objectB;
    }

    public void build()
    {
        if (connect.text != null)
        {
            if (levelNumber == 1)
            {
                BuildCheck.instance.checkLevel1(connect.text);
            }
            if(levelNumber == 2)
            {
                BuildCheck.instance.checkLevel2(connect.text);
            }
            if (levelNumber == 3)
            {
                BuildCheck.instance.checkLevel3(connect.text);
            }
        } 
    }

    public void ClearLevel()
    {


        connect.text = null;
        drop.text = null;


        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("line");
        foreach (GameObject toDestroy2 in toDestroy)
        {
            Destroy(toDestroy2);
        }

        foreach (InventoryObject dropped in droppedObjects)
        {
            dropped.transform.parent = bench.transform;
        }

        Collector col = collector.GetComponent<Collector>();
        col.clear();

        foreach(InventoryObject ob in inventoryObjects)
        {
            ob.clear();
        }

        droppedObjects.Clear();
    }

}