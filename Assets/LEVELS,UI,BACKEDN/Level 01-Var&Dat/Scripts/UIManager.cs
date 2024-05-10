using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject level1;
    public GameObject level2;
    public GameObject level3;

    public GameObject levelPopups;
    public GameObject level01intro;
    public GameObject level02intro;
    public GameObject level03intro;
    public GameObject end;

    private void Awake()
    {
        Instance = this;
    }

    public void nextlevel(int level)
    {
        if(level == 1) {
            Level2intro();
        }
        else if(level == 2)
        {
            Level3intro();
        }
        else if (level == 3)
        {
            LevelComplete();
        }
    }

    public void closeIntros()
    {
        levelPopups.SetActive(false);
        level01intro.SetActive(false);
        level02intro.SetActive(false);
        level03intro.SetActive(false);
    }

    public void closelevels() 
    {
        level1.SetActive(false);
        level2.SetActive(false);
        level3.SetActive(false);
    }

    public void goLevel1()
    {
        closeIntros();
        level1.SetActive(true);
        level2.SetActive(false);
        level3.SetActive(false);
    }

    public void Level2intro()
    {
        level1.SetActive(false);
        closeIntros();
        levelPopups.SetActive(true);
        level02intro.SetActive(true);
    }

    public void goLevel2()
    {
        closeIntros() ;
        level1.SetActive(false);
        level2.SetActive(true);
        level3.SetActive(false);
    }

    public void Level3intro()
    {
        level2.SetActive(false);
        closeIntros();
        levelPopups.SetActive(true);
        level03intro.SetActive(true);
    }

    public void goLevel3()
    {
        closeIntros();
        level1.SetActive(false);
        level2.SetActive(false);
        level3.SetActive(true);
    }

    public void LevelComplete()
    {
        closelevels();
        closeIntros();
        levelPopups.SetActive(true);
        end.SetActive(true);
    }
}
