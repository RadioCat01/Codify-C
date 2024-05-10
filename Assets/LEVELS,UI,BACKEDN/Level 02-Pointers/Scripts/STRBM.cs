using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class STRBM : MonoBehaviour
{
    public static STRBM instance;
    bool partsfound = false;
    public BoardManager2 BM2;
    private void Awake()
    {
        instance= this;
    }
    public void reOrder()
    {
        string[] prioritizedLines = new string[] { "struct Spell black_magic ;", "struct Spell *writer ;", "writer  =  &spell ;" };
        string[] lines = BoardManager2.Instance.connect.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder sb = new StringBuilder();
        foreach (string line in prioritizedLines)
        {
            int index = Array.IndexOf(lines, line);
            if (index >= 0)
            {
                sb.AppendLine(line);
            }
        }

        foreach (string line in lines)
        {
            if (line != null)
            {
                sb.AppendLine(line);
            }
        }
        BoardManager2.Instance.connect.text = sb.ToString();
        partsfound = false;
  
    }

    public void checkST() {
        HashSet<string> expectedType1=new HashSet<string>{ "struct Spell black_magic ;", "struct Spell *writer ;", "writer  =  &spell ;" };
       
        string text = BM2.connect.text;
        if (text!=null)
        {
            string[] lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            bool allPartsFound = true;
            foreach (string expectedPart in lines)
            {
                Debug.Log(expectedPart);
                if (!expectedType1.Contains(expectedPart))
                {
                    allPartsFound = false;
                    break;
                }
            }
            if (allPartsFound)
            {
                partsfound = true;
            }
        }
   
    }

    private void Update()
    {
        if (partsfound)
        {
            reOrder();
        }
    }
}
