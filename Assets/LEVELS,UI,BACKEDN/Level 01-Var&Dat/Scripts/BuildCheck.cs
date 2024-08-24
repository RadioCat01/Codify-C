using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class BuildCheck : MonoBehaviour
{

    public static BuildCheck instance;
    public GameObject Uimanager;
    public UIManager UIManager;
    public TMP_Text errortext;
    private void Awake()
    {
        instance = this;
        UIManager = Uimanager.GetComponent<UIManager>();
    }
    public void checkLevel1(string buildText)
    {
        Debug.Log("in checklevel1");
        HashSet<string> expectedType1 = new HashSet<string>() { "int Silver = 20 ;", "int Gold = 5 ;", "bool moon_Stone = true ;", "float iron = 10.25 ;" };
        HashSet<string> expectedType2 = new HashSet<string>() { "float Silver = 20 ;", "float iron = 10.25 ;", "int Gold = 5 ;", "bool moon_Stone = true ;" };
        HashSet<string> expectedType3 = new HashSet<string>() { "bool moon_Stone = true ;", "float iron = 10.25 ;", "int Silver = 20 ;", "float Gold = 5 ;" };
        HashSet<string> expectedType4 = new HashSet<string>() { "bool moon_Stone = true ;", "float iron = 10.25 ;", "float Silver = 20 ;", "float Gold = 5 ;" };

        string[] lines = buildText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        HashSet<string> actual = new HashSet<string>(lines);

        bool type1 = expectedType1.SetEquals(actual);
        bool type2 = expectedType1.SetEquals(actual);
        bool type3 = expectedType1.SetEquals(actual);
        bool type4 = expectedType1.SetEquals(actual);

        if (type1 || type2 || type3 || type4)
        {
            Debug.Log("successfull build");
            StartCoroutine(level1Build());
           
        }
        else 
        {
            StartCoroutine(wrongText());
        }
    }

    public IEnumerator level1Build()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.lv1complete();
    }


    public void checkLevel2(string buildText)
    {
        Debug.Log("in checklevel2");
        HashSet<string> expectedType1 = new HashSet<string>() {

            "include<stdio.h>",
            "int main( )",
            " { ",
            "floatGold = 20.12 ;",
            "double Silver = 6.626e-34 ;",
            "char Scar  = 'G' ;",
            "int Emerald = 1 ;",
            "printf( \" 1st element : %.2f \", Gold );",
            "printf( \" 2nd element : %e \", Silver );",
            "printf( \" 3rd element : %c \", Scar );",
            "printf( \" 4th element : %d \", Emerald );",
            "}"

        };
      

        string[] lines = buildText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        HashSet<string> lineSt = new HashSet<string>(lines);
        bool allPartsFound = expectedType1.SetEquals(lineSt);


        if (allPartsFound)
        {
            Debug.Log("successful build");
            StartCoroutine (level2Build());
        }
        else
        {
            StartCoroutine(wrongText());
        }
    }
    public IEnumerator level2Build()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.lv2complete();
    }

    public void checkLevel3(string buildText)
    {
        Debug.Log("in checklevel3");
        HashSet<string> expectedType1 = new HashSet<string>() { 
            "include<stdio.h>",
            "int main( )",
            " { ",
            "int Ruby = 1 ;",
            "float Brine = 3.1 ;",
            "float Cerise = 10.25 ;",
            "bool Gold = true ;",
            "char Scar = ' D ' ;",
            "printf( \" 1st element : %d \", Ruby )",
            "printf( \" 2nd element : %c \", Scar )",
            "printf( \" 3rd element : %.2f \", Cerise )",
            "printf( \" 4th element : %d \", Gold )",
            "printf( \" 5th element : %.1f \", Brine )",
            "}"
        };

        string[] lines = buildText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        HashSet<string> lineSt = new HashSet<string>(lines);

        bool allPartsFound = expectedType1.SetEquals(lineSt);


        if (allPartsFound)
        {
            Debug.Log("successful build");

            GameObject lv3 = GameObject.Find("LevelManager");
            NextLevelButton sc=lv3.AddComponent<NextLevelButton>();
            sc.updateUserLevel();

            StartCoroutine(level3Build());
        }
        else
        {
            StartCoroutine(wrongText());
        }

    }

    public IEnumerator wrongText()
    {
        errortext.text = "W r o n g  b u i l d !";
        yield return new WaitForSeconds(2f);
        errortext.text = null;
    }

    public IEnumerator level3Build()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.lv3complete();
        
    }
}
