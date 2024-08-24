using System.Text;
using UnityEngine;


public class Pointer_Container : MonoBehaviour
{
    public static Pointer_Container Instance;
    public string OBJName;
    [Space]
    public bool isPointer;
    public bool isAContainer;
    public bool isTemp;
    public bool isStrCpy;
    public string strcpyString;
    public int num;
    [Space]
    public string value;
    public string address;
    public bool isAddress;
    public string attachedText;
    public string currentText;
    public string retrievedText;
    [Space]
    public Vector3 currentPos;
    public bool isDragging;
    public bool iscreatingLine;
    [Space]
    [Space]
    private bool isDropped;
    private Vector3 offset;
    public bool isUsed;
    public bool isStrcpyDropped;
    public bool holdsValue;
    public string tempValue;
    public bool isHeatChanged;
    public bool isSwaped;
    public bool isValueset;
    public bool currentposChanged;


   
    void Start()
    {
        currentposChanged = false;
        isValueset = false;
        isSwaped=false;
        isHeatChanged = false;
        holdsValue = false;
        isAddress = true;
        isStrcpyDropped = false;
        iscreatingLine = false;
        isDragging = false;
        isDropped = false;
        isUsed = false;
        currentPos = transform.position;

    }
    public void ChangeHeat()
    {
        isHeatChanged = true;
    }

    public void setUse()
    {
        isUsed = false;
    }

    public void creatingLine()
    {
      
            iscreatingLine = true;
        
    }
    public void noCreatingLine()
    {
       
            iscreatingLine = false;
        
    }
    void OnMouseDown()
    {
        if (!isUsed)
        {
            isDragging = true;
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            BoardManager2.Instance.drop.text = OBJName;
        }
        else if(isUsed && !isStrCpy)
        {
            if (isAddress && !iscreatingLine)
            {
                
                currentText = address;
                BoardManager2.Instance.drop.text=null;
                BoardManager2.Instance.drop.text = currentText;
                isAddress = false;
                return;
            }
            
            else if(!isAddress && !iscreatingLine)
            {
               
                currentText = value;

                BoardManager2.Instance.drop.text = null;
                BoardManager2.Instance.drop.text = currentText;
                isAddress =true;
                return;
            }

        }
    }


    void Update()
    {
        if (isDragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
        else if (isDropped)
        {
           
        }

    }
    void OnMouseUp()
    {
      
        isDragging = false;
        if (!isDropped && !isUsed)
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
                    BoardManager2.Instance.drop.text = null;
                    isUsed = true;
                    currentText = value;
                    if (isStrCpy) {
                        if(!isStrcpyDropped) {
                            ValuePrinter.Instance.dropOB(strcpyString);
                            isStrcpyDropped = true;
                            currentposChanged = true;
                        }
                        isUsed = false;
                        return;
                    }
                    ValuePrinter.Instance.dropOB(attachedText);
                 
                    return;
                }
                else
                {
                    transform.position = currentPos;
                }
            }
        }
    }
}
