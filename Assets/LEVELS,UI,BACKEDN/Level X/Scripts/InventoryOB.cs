using UnityEngine;

public class InventoryObject : MonoBehaviour
{
    public int objectID; // Unique identifier for the object

    public static InventoryObject instance;

    public bool isDragging;
    private Vector3 offset;
    private bool isDropped; // Flag to indicate if object is dropped on the board
    public string text;
    private Vector3 currentPos;
    void Start()
    {
        isDragging = false;
        isDropped = false;
        currentPos = transform.position;
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        
        
        isDragging = false;
        if (!isDropped) // Check if dropped on board collider (implemented in Update)
        {
            GameObject board = GameObject.Find("Board");
            RaycastHit2D[] allHits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach (RaycastHit2D hit in allHits)
            {
                if (hit.collider != null && hit.collider.gameObject==board)
                {
                    Debug.Log("hit on board");
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = -1;
                    transform.position = mousePosition;
                    isDropped = true;
                          
                    BoardManager.Instance.OnObjectDropped(this);
                }
                else
                {
                    Debug.Log("hit away");
                    transform.position = currentPos;                   
                }
            }
        }
    }

    public void clear()
    {
        isDropped = false;
        transform.position = currentPos;
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
      
    }
}