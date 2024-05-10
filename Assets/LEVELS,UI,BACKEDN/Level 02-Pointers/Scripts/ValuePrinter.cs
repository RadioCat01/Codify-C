using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ValuePrinter : MonoBehaviour
{
    public static ValuePrinter Instance;

    StringBuilder sb=new StringBuilder();
    private string update;
    private string text;

    private void Awake()
    {
        Instance = this;
    }

    public void clearSb()
    {
        sb.Clear();
    }

    public void dropOB(string str)
    {
        string connect= BoardManager2.Instance.connect.text;

        if(connect == null ) {
           sb.Append(str);
            StartCoroutine(BoardManager2.Instance.TypeText(sb.ToString()));
            return;
        }
        else
        {
            sb.Append("\n" + str);
            StartCoroutine(BoardManager2.Instance.TypeText(sb.ToString()));
        }
    }
    public void lineconnected(string retrivedbyPointer,string currentTextOPointer)
    {
        text = currentTextOPointer+"  =  " + retrivedbyPointer;
        sb.Append("\n"+text);
        StartCoroutine(BoardManager2.Instance.TypeText(sb.ToString())); text = null;
    }

    public void OnSpellWordDrop(string str, Pointer_Container o)
    {
        if (!o.isValueset)
        {
            if (o.num == 1)
            {
                sb.Replace("value1", str);
                StartCoroutine(BoardManager2.Instance.TypeText(sb.ToString()));
                o.isValueset = true;
                return;
            }
            else if (o.num == 2)
            {
                sb.Replace("value2", str);
                StartCoroutine(BoardManager2.Instance.TypeText(sb.ToString()));
                o.isValueset = true;
                return;
            }
        }
    }
    public void OnSpellLine(string str, Pointer_Container o)
    {
        if (!o.holdsValue)
        {
            Debug.Log("strcpy holds no value");
            if (o.num == 1)
            {
                Debug.Log("O = 1");
                sb.Replace("pointer1", str);
                StartCoroutine(BoardManager2.Instance.TypeText(sb.ToString()));
                o.holdsValue = true;
                return;
            }
            else if (o.num==2)
            {
                Debug.Log("O = 2");
                sb.Replace("pointer2", str);
                StartCoroutine(BoardManager2.Instance.TypeText(sb.ToString()));
                o.holdsValue = true;
                return;
            }
        }

    }

    public void Level2(string text)
    {
        clearSb();
        sb.Append("include < stdio.h >");
        sb.AppendLine("int main ( )"+"\n"+ "{");
        
        sb.Append(text);
        sb.Append('\n');
        sb.Append("}");
        StartCoroutine(BoardManager2.Instance.TypeText(sb.ToString()));
    }



}
