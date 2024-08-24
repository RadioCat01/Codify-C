using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class SpellPhraseLV3 : MonoBehaviour
{
    private bool isUsed;
    public bool isDragging;
    private Vector3 offset;
    public Vector3 currentPos;
    private bool isDropped;
    private Vector3 startingPos;
    [Space]
    public string spellPhrase;
    private List<GameObject> curruntPC;
    private void Awake()
    {
        curruntPC = new List<GameObject>();
        isUsed = false;
        isDragging = false;
        isDropped = false;
        startingPos = transform.position;
    }

    void OnMouseDown()
    {
        if (!isUsed)
        {
            isDragging = true;
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            BoardManager2.Instance.drop.text= spellPhrase;
        }
    }

    void Update()
    {
        if (isDragging && !isUsed)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }
    public void clear() { 
        transform.parent = null;
        transform.position = startingPos;
        isUsed = false;
        curruntPC.Clear();
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    void OnMouseUp()
    {

        isDragging = false;
        if (!isDropped && !isUsed)
        {
            getStrcpy();
            GameObject board = GameObject.Find("Board");
            RaycastHit2D[] allHits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach (RaycastHit2D hit in allHits)
            { 
                foreach (GameObject str in curruntPC)
                    {
                       Pointer_Container pc = str.GetComponent<Pointer_Container>();
                        if (str == hit.collider.gameObject && pc.currentposChanged )
                        {
                            Debug.Log("hiton strcpy");
                            BoxCollider2D strC= str.GetComponent<BoxCollider2D>();
                            Vector3 dropPos = strC.bounds.center;
                            transform.position = dropPos;
                           
                            currentPos = transform.position;
                            transform.parent = str.transform;
                            ValuePrinter.Instance.OnSpellWordDrop(spellPhrase, str.GetComponent<Pointer_Container>());
                            gameObject.GetComponent<BoxCollider2D>().enabled = false;
                            isUsed = true;
                            BoardManager2.Instance.drop.text = null;
                        return;
                        }                   
                }

                if (hit.collider != null && hit.collider.gameObject == board)
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = -1;
                    transform.position = mousePosition;
                    currentPos = transform.position;
                   
                    return;
                }
                else
                {
                    transform.position = startingPos;
                    
                }
            }
        }
    }

    public void onBox(Vector3 boxpos)
    {
        transform.Translate(boxpos);
    }

    public void getStrcpy()
    {
        GameObject[] li = GameObject.FindGameObjectsWithTag("InventoryObject");
        foreach (GameObject go in li)
        {
            if (go.GetComponent<Pointer_Container>().isStrCpy)
            {
                curruntPC.Add(go);
            }
        }
    }
}
