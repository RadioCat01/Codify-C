
using UnityEngine;

public class InventoryObject : MonoBehaviour
{
    public static InventoryObject instance;
    private bool isDropped;
    public bool isDragging;
    private Vector3 offset; 
    private Vector3 currentPos;
    public Vector3 starterPos;
    private InventoryObject targetOB;

    

    public string text;
    public string material;
    public bool isAContainer;
    public bool isUsed=false;
    void Start()
    {
        isDragging = false;
        isDropped = false;
        currentPos = transform.position;
        starterPos = transform.position;
        
    }

    void OnMouseDown()
    {    
         isDragging = true;
         offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    void OnMouseUp()
    {
        isDragging = false;
        if (!isDropped) 
        {
            GameObject board = GameObject.Find("Board");
            RaycastHit2D[] allHits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach (RaycastHit2D hit in allHits)
            {
                if (hit.collider != null && hit.collider.gameObject == board)
                {

                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = -1;
                    transform.position = mousePosition;                 
                    currentPos=transform.position;
                   BoardManager.Instance.OnObjectDropped(this);
                    return;
                }
                else if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.gameObject.CompareTag("InventoryObject"))
                {
                    if (!isAContainer)
                    {
                        InventoryObject hitO = hit.collider.gameObject.GetComponent<InventoryObject>();
                        BoardManager.Instance.OnObjectDroppedd(this, hitO);
                        IOconnectorLogic setM = hit.collider.gameObject.GetComponent<IOconnectorLogic>();
                        setM.SetMatereal(this.material);
                        isDropped = true;
                        targetOB = hitO;
                        BoxCollider2D boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
                        boxCollider.enabled = false;
                        return;
                    }
                    else if(isAContainer)
                    {
                        transform.position = currentPos;
                        return;
                    }
                }
                else if(hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.gameObject.CompareTag("final"))
                {
                    if (isAContainer)
                    {
                        IOconnectorLogic thisLogic = this.gameObject.GetComponent<IOconnectorLogic>();
                        thisLogic.setText();
                        Debug.Log("set called");
                        return;
                    }

                }
                 transform.position = currentPos;             
            }
        }
    }

    public void clear()
    {
        isDropped = false;
        transform.position = starterPos;
        BoxCollider2D boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;

    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
        else if (isDropped)
        {
            transform.position=targetOB.transform.position;
        }
      
    }
}