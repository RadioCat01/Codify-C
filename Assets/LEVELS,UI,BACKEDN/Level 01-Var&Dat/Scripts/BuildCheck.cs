using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BuildCheck : MonoBehaviour
{

    public static BuildCheck instance;
    public GameObject Uimanager;
    public UIManager UIManager;
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

        bool allPartsFound = true;
        foreach (string expectedPart in lines)
        {
            Debug.Log(expectedPart);
            if (!expectedType1.Contains(expectedPart) && !expectedType2.Contains(expectedPart) && !expectedType3.Contains(expectedPart) && !expectedType4.Contains(expectedPart))
            {               
               allPartsFound = false;
               break;
            }
        }
        if (allPartsFound)
        {
            Debug.Log("successfull build");
            //UIManager.Level2intro();
           
        }
        else 
        {
            Debug.Log("wrongBuild");
        }
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
            //UIManager.LevelComplete();
        }
        else
        {
            Debug.Log("wrongBuild");
        }




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
            //UIManager.LevelComplete();
        }
        else
        {
            Debug.Log("wrongBuild");
        }

    }
}
/*IEnumerator<string> enumerator = lineSt.GetEnumerator();

        Debug.Log("Elements in the HashSet:");

        // Loop until there are no more elements
        while (enumerator.MoveNext())
        {
            Debug.Log(enumerator.Current);
        }*/