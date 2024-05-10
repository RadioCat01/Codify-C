using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BoardManager2 : MonoBehaviour
{
    public static BoardManager2 Instance;

    public List<Pointer_Container> BoardOBs;
    public List<SpellPhraseLV3> SpellOBs;
    public bool IsLinesActive;
    private Pointer_Container selectedObject;
    

    private Dictionary<LineCreator, KeyValuePair<Pointer_Container, Pointer_Container>> connectedLines;

    public TMP_Text drop;
    public TMP_Text connect;
    public TMP_Text starterText;
    private string starterTextString="include < stdio.h >\nstruct Spell {\nchar first [ 50 ] ;\n char address [ 100 ] ;\n} ;\nint main ( ) {";

    public GameObject LineGameObject;
    public GameObject LineGameObject2;
    private int strcpyindex = 1;

    [Space]
    public Sprite cold;
    public Sprite hot;
    public Sprite natural;

    [Space]
    public GameObject NatClick;
    public GameObject p1Click;
    public GameObject p2Click;

    private void Start()
    {
        StartCoroutine(TypeStarterText(starterTextString));
    }
    void Awake()
    {
        Instance = this;    
        connect.text = null;
        connectedLines = new Dictionary<LineCreator, KeyValuePair<Pointer_Container, Pointer_Container>>();
    }

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
                        Pointer_Container clickedObject = hit2.collider.gameObject.GetComponent<Pointer_Container>();
                        if (clickedObject != null && clickedObject.CompareTag("InventoryObject") || clickedObject != null && clickedObject.CompareTag("pointer"))
                        {
                            OnObjectClicked(clickedObject);
                            cliker(clickedObject);
                            return;
                        }
                        else
                        {
                            Debug.Log("unfreezed");
                            selectedObject = null;
                            Uncliker();
                            UnfreezeType();

                            GameObject[] pointers = GameObject.FindGameObjectsWithTag("pointer");
                            foreach (GameObject go in pointers)
                            {
                                if (go.GetComponent<Pointer_Container>().iscreatingLine)
                                {
                                    go.GetComponent<Pointer_Container>().iscreatingLine = false;
                                }
                            }


                        }
                    }
                }

            }
        }
    }
    public void cliker(Pointer_Container clicked)
    {
        if (clicked.gameObject.name == "NaturalOne")
        {
            NatClick.SetActive(true);
        }
        else if (clicked.gameObject.name == "Pointer")
        {
            p1Click.SetActive(true);
        }
        else if (clicked.gameObject.name == "Pointer2")
        {
            p2Click.SetActive(true);
        }
    }
    public void Uncliker()
    {
        GameObject[] clicked = GameObject.FindGameObjectsWithTag("Clicker");
        foreach(GameObject go in clicked)
        {
            go.SetActive(false);
        }
    }

    public void freezeType()
    {
        GameObject[] IO = GameObject.FindGameObjectsWithTag("InventoryObject");
        foreach (GameObject go in IO)
        {
            Pointer_Container pc = go.GetComponent<Pointer_Container>();
            if (pc.isAContainer && !pc.iscreatingLine)
            {              
                pc.creatingLine();
            }          
        }
    }
    public void UnfreezeType()
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("InventoryObject");
        
        foreach (GameObject go in all)
        {
            Pointer_Container pc = go.GetComponent<Pointer_Container>();
            if (pc.iscreatingLine && pc.iscreatingLine)
            {
                
                pc.noCreatingLine();
            }
        }
    }

    public void FreezePointers(Pointer_Container pointer)
    {
        GameObject[] pointers = GameObject.FindGameObjectsWithTag("pointer");
        foreach (GameObject go in pointers)
        {
            Pointer_Container pc= go.GetComponent<Pointer_Container>();
            if (pc != pointer.GetComponent<Pointer_Container>())
            {
                Debug.Log("freezing pointers called");
                pc.creatingLine();
            }
        }
    }

    public void OnObjectClicked(Pointer_Container clickedObject)
    {
        if (selectedObject == null)
        {
            if (clickedObject.CompareTag("pointer"))
            {
                selectedObject = clickedObject;
                FreezePointers(clickedObject);
                freezeType();
                return;
            }
            
        }
        else if (selectedObject != clickedObject)
        {
            Pointer_Container pc = clickedObject.GetComponent<Pointer_Container>();
            Pointer_Container pointer = selectedObject.GetComponent<Pointer_Container>();
            //&& pointer.retrievedText == "&spell"
            if (pc.isStrCpy && pointer.retrievedText== "&spell ;" && pointer.currentText== "writer")
            {
                Debug.Log("clicked on Strcpy");
                string FirstValueOfStruct = "writer->first";
                string SecondValueOfStruct = "writer->second";

                if (strcpyindex == 1 )
                {
                    Debug.Log("indexis1");
                    ValuePrinter.Instance.OnSpellLine(FirstValueOfStruct,clickedObject);
                    LineofStrcpy(selectedObject, clickedObject);
                    strcpyindex +=1;
                    return;         
                }else if(strcpyindex == 2 )
                {
                    ValuePrinter.Instance.OnSpellLine(SecondValueOfStruct,clickedObject);
                    LineofStrcpy(selectedObject, clickedObject);
                    return;
                }

                Debug.Log("second click on Strcpy");

            }
            else if (pc.isStrCpy && pointer.retrievedText == "&spell ;" && pointer.currentText == "*writer")
            {
                Debug.Log("clicked on Strcpy");
                string FirstValueOfStruct = "*writer->first";
                string SecondValueOfStruct = "*writer->second";

                if (strcpyindex == 1)
                {
                    Debug.Log("indexis1");
                    ValuePrinter.Instance.OnSpellLine(FirstValueOfStruct, clickedObject);
                    LineofStrcpy(selectedObject, clickedObject);
                    strcpyindex += 1;
                    return;
                }
                else if (strcpyindex == 2)
                {
                    ValuePrinter.Instance.OnSpellLine(SecondValueOfStruct, clickedObject);
                    LineofStrcpy(selectedObject, clickedObject);
                    return;
                }

                Debug.Log("second click on Strcpy");

            }
            else if(pc.isStrCpy && pc.retrievedText != "&spell ;")
            {
                Debug.Log("wrong");
                return;
            }
            CheckConnectionOnClick(selectedObject, clickedObject);
            selectedObject = null;
            Uncliker();
        }
    }

    public void LineofStrcpy(Pointer_Container pointer, Pointer_Container target)
    {
        
        GameObject lineObject = Instantiate(LineGameObject2);
        LineCreator line = lineObject.GetComponent<LineCreator>();
        line.pointer = pointer;
        line.ConnectedObjectB = target;
    }


    public IEnumerator TypeText(string fullText)
    {
        connect.text = null;
        foreach (char character in fullText)
        {
            
            connect.text += character;
            yield return new WaitForSeconds(0.0001f);
        }
    }
    private IEnumerator TypeStarterText(string fullText)
    {
        foreach (char character in fullText)
        {
            starterText.text += character;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void CheckConnectionOnClick(Pointer_Container pointer,Pointer_Container target)
    {     
            CreateConnectionLine(pointer, target);
    }

    public void CreateConnectionLine(Pointer_Container pointer, Pointer_Container target)
    {

            LineCreator checkPointer = FindLineByPointer(pointer);
            LineCreator checkTarget = FindLineByPointer(target);

        if (checkPointer != null && checkTarget !=null)
            {
           
                DestroyLine(checkPointer);
                DestroyLine(checkTarget);

                LineCreateValuExchange(pointer, target);
                ChangeSprite(pointer);
            return;
            }
        else if (checkPointer != null || checkTarget != null)
        {
            if(checkTarget != null) 
            {
                DestroyLine(checkTarget);
            }
            else if(checkPointer != null)
            {
                DestroyLine(checkPointer);
            }
            LineCreateValuExchange(pointer, target);
            ChangeSprite(pointer);
            return;
        }
            else
            { 
               LineCreateValuExchange(pointer, target);
               ChangeSprite(pointer);
        }
        

    }

    public void ChangeSprite(Pointer_Container pointer)
    {
        if (!pointer.isTemp)
        {
            if (pointer.retrievedText == "&Heated ;" && (pointer.currentText == "line1" || pointer.currentText == "line2"))
            {
                Debug.Log("addressed to hot");
                SpriteRenderer sr = pointer.GetComponent<SpriteRenderer>();
                sr.sprite = hot;
            }
            else if (pointer.retrievedText == "&Freezed ;" && (pointer.currentText == "line1" || pointer.currentText == "line2"))
            {
                Debug.Log("addressed to cold");
                SpriteRenderer sr = pointer.GetComponent<SpriteRenderer>();
                sr.sprite = cold;
            }
            else if(pointer.tempValue == "&Freezed ;")
            {
                SpriteRenderer sr = pointer.GetComponent<SpriteRenderer>();
                sr.sprite = cold;
                pointer.isHeatChanged = true;
             

            }
            else if(pointer.tempValue == "&Heated ;")
            {
                SpriteRenderer sr = pointer.GetComponent<SpriteRenderer>();
                sr.sprite = hot;
                pointer.isHeatChanged = true;
            }
           
        }
        else if(pointer.isTemp)
        {
            if(pointer.tempValue == "&Heated ;")
            {
                SpriteRenderer sr = pointer.GetComponent<SpriteRenderer>();
                sr.sprite = hot;
            }
            else if(pointer.tempValue == "&Freezed ;")
            {
                SpriteRenderer sr = pointer.GetComponent<SpriteRenderer>();
                sr.sprite = cold;
            }
        }
    }

    public void LineCreateValuExchange(Pointer_Container pointer, Pointer_Container target)
    {
        GameObject lineObject = Instantiate(LineGameObject);
        LineCreator line = lineObject.GetComponent<LineCreator>();
        line.pointer = pointer;
        line.ConnectedObjectB = target;
        connectedLines.Add(line, new KeyValuePair<Pointer_Container, Pointer_Container>(pointer, target));
        if (!pointer.isTemp)
        {
            if (!pointer.holdsValue)
            {
                pointer.retrievedText = target.currentText;
                pointer.holdsValue = true;
            }
            else if (pointer.holdsValue && !target.isTemp)
            {
              if (pointer.holdsValue && !target.holdsValue)
                {
                    pointer.retrievedText = target.retrievedText;
                }
                else if (pointer.holdsValue && target.holdsValue)
                {
                    pointer.retrievedText = target.currentText;
                    pointer.tempValue = target.retrievedText;
                    ChangeSprite(pointer);
                    pointer.isHeatChanged = true;

                    GameObject[] IO = GameObject.FindGameObjectsWithTag("InventoryObject");
                    if (target.retrievedText == "&Freezed ;")
                    {
                        foreach (GameObject o in IO)
                        {
                            Pointer_Container pc= o.GetComponent<Pointer_Container>();
                            if(pc.address== "&Heated ;")
                            {
                                SpriteRenderer sr=pc.gameObject.GetComponent<SpriteRenderer>();
                                sr.sprite = cold;
                            }
                        }
                        
                    }
                    if (target.retrievedText == "&Heated ;")
                    {
                        foreach (GameObject o in IO)
                        {
                            Pointer_Container pc = o.GetComponent<Pointer_Container>();
                            if (pc.address == "&Freezed ;")
                            {
                                SpriteRenderer sr = pc.gameObject.GetComponent<SpriteRenderer>();
                                sr.sprite = hot;
                            }
                        }
                    }

                }
            }
            else if (pointer.holdsValue && target.isTemp)
            {
                if(pointer.holdsValue)
                {
                    pointer.retrievedText = target.currentText +" ;";
                    pointer.tempValue = target.tempValue;
                    ChangeSprite(pointer);
                    pointer.isHeatChanged = true;

                    if (target.tempValue == "&Heated ;") {
                        GameObject[] IO = GameObject.FindGameObjectsWithTag("InventoryObject");
                        foreach(GameObject o in IO)
                        {
                            Pointer_Container pc= o.GetComponent<Pointer_Container>();
                            if(pc.address == "&Freezed ;")
                            {
                                SpriteRenderer sr = pc.gameObject.GetComponent<SpriteRenderer>();
                                sr.sprite = hot;
                            }
                        }
                        
                        ValuePrinter.Instance.lineconnected(pointer.retrievedText, pointer.currentText);
                        ValuePrinter.Instance.Level2(connect.text);
                        return;

                    }
                    else if(target.tempValue == "&Freezed ;")
                    {
                        GameObject[] IO = GameObject.FindGameObjectsWithTag("InventoryObject");
                        foreach (GameObject o in IO)
                        {
                            Pointer_Container pc = o.GetComponent<Pointer_Container>();
                            if (pc.address == "&Heated ;")
                            {
                                SpriteRenderer sr = pc.gameObject.GetComponent<SpriteRenderer>();
                                sr.sprite = cold;
                            }
                        }
                        ValuePrinter.Instance.lineconnected(pointer.retrievedText, pointer.currentText);

                        ValuePrinter.Instance.Level2(connect.text);
                        return;

                    }
                }
            }
        }
        else if (pointer.isTemp)
        {
            pointer.retrievedText = target.currentText +" ;";
            pointer.tempValue=target.retrievedText;
            ChangeSprite(pointer);
        }
            selectedObject = null;
        ValuePrinter.Instance.lineconnected(pointer.retrievedText, pointer.currentText);
        UnfreezeType();
    }


    private LineCreator FindLineByPointer(Pointer_Container pointer)
    {
        foreach (KeyValuePair<LineCreator, KeyValuePair<Pointer_Container, Pointer_Container>> entry in connectedLines)
        {
            if (entry.Value.Key == pointer || entry.Value.Value == pointer)
            {
                return entry.Key;
            }
        }
        return null;
    }
    public void DestroyLine(LineCreator line)
    {
        if (connectedLines.ContainsKey(line))
        {
            connectedLines.Remove(line);
            Destroy(line.gameObject);
            
        }
    }


    public void clearLV2()
    {
        GameObject[] IO = GameObject.FindGameObjectsWithTag("InventoryObject");
        foreach (GameObject o in IO)
        {
            Pointer_Container pc= o.GetComponent<Pointer_Container>();
            pc.isHeatChanged= false;
        }
        GameObject[] go = GameObject.FindGameObjectsWithTag("line");
        foreach (GameObject toDes in go)
        {
            Destroy(toDes);
        }
        foreach(Pointer_Container toPos in BoardOBs)
        {
            toPos.setUse();
            toPos.transform.position = toPos.currentPos;
            toPos.currentText = null;
            toPos.retrievedText = null;
            
            if (toPos.gameObject.CompareTag("pointer"))
            {
                toPos.holdsValue = false;
                SpriteRenderer sr = toPos.gameObject.GetComponent<SpriteRenderer>();
                sr.sprite = natural;
            }
        }
        connectedLines.Clear();

        ValuePrinter val= ValuePrinter.Instance.GetComponent<ValuePrinter>();
        val.clearSb();
        connect.text =null;
        drop.text = "";
    }

    public void clearLV3()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("line");
        foreach (GameObject toDes in go)
        {
            Destroy(toDes);
        }
        foreach (Pointer_Container toPos in BoardOBs)
        {
            toPos.setUse();
            toPos.isStrcpyDropped = false;
            toPos.transform.position = toPos.currentPos;
            toPos.isValueset = false;
            toPos.currentposChanged = false;
        }
        foreach (SpellPhraseLV3 toPos in SpellOBs)
        {
            toPos.clear();
        }
        connectedLines.Clear();
        ValuePrinter val = ValuePrinter.Instance.GetComponent<ValuePrinter>();
        val.clearSb();

        GameObject[] gos= GameObject.FindGameObjectsWithTag("SpellPhrase");
        foreach (GameObject toCle in gos)
        {
            SpellPhraseLV3 sc=toCle.GetComponent<SpellPhraseLV3>();
            sc.clear();
        }

        connect.text = null;
        drop.text = "";
    }

    public void buildLV2()
    {
        GameObject pointer1 = GameObject.Find("Pointer");
        GameObject pointer2 = GameObject.Find("Pointer2");
        Pointer_Container pc1 = pointer1.GetComponent<Pointer_Container>();
        Pointer_Container pc2 = pointer2.GetComponent<Pointer_Container>();

        if(pc1.isHeatChanged && pc2.isHeatChanged)
        {
            Debug.Log("successfull");
        }

    }

    public void buildV3()
    {
        string text =
            "struct Spell black_magic ;" +
            "\n"+
            "struct Spell *writer ;" +
            "\n" +
            "writer  =  &spell ;" +
            "\n" +
            "strcpy( writer->second, \"wooo\" );" +
            "\n" +
            "strcpy( writer->first, \"wooo\" );";

        if(connect.text==text)
        {
            Debug.Log("correct");
        }
        else
        {
            Debug.Log("wrong");
        }
    }
    

}



