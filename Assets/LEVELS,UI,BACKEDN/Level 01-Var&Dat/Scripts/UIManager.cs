using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject level1intro;
    public GameObject level1outro;

    public GameObject ins1;
    public GameObject ins2;
    public GameObject ins3;
    public GameObject ins4;
    public GameObject ins5;
    public GameObject end;

    public GameObject level1_1;
    public GameObject level1_2;
    public GameObject level1_3;

    public GameObject moonblade;
    public GameObject guardianspride;
    public GameObject gladius;

    private void Awake()
    {
        Instance = this;
    }

    public void resetlevel()
    {
        ClearScreen();
        level1();
    }

    public void ClearScreen()
    {
        level1intro.SetActive(false);
        level1outro.SetActive(false);
        ins1.SetActive(false);
        ins2.SetActive(false);
        ins3.SetActive(false);
        ins4.SetActive(false);
        ins5.SetActive(false);
        end.SetActive(false);
        level1_1.SetActive(false);
        level1_2.SetActive(false);
        level1_3.SetActive(false);
        moonblade.SetActive(false);
        guardianspride.SetActive(false);
        gladius.SetActive(false);
    }

    public void closeAll()
    {
        
        level1_1.SetActive(false);
        level1_1.SetActive(false);
        level1_2.SetActive(false);
    }

    public void clearlvComplete()
    {
        moonblade.SetActive(false);
        guardianspride.SetActive(false);
        gladius.SetActive(false);
    }
    public void openIns1(){ins1.SetActive(true);level1intro.SetActive(false);}
    public void openIns2(){ins2.SetActive(true); ins1.SetActive(false);}
    public void openIns3(){ins3.SetActive(true); ins2.SetActive(false);}
    public void openIns4(){ins4.SetActive(true); ins3.SetActive(false);}
    public void openIns5(){ins5.SetActive(true);}
    public void closeins() {ins1.SetActive(false);ins2.SetActive(false);ins3.SetActive(false);ins4.SetActive(false);ins5.SetActive(false);moonblade.SetActive(false);guardianspride.SetActive(false);gladius.SetActive(false);}

    public void lv1complete(){ ClearScreen(); moonblade.SetActive(true);}
    public void lv2complete(){ clearlvComplete(); guardianspride.SetActive(true);}
    public void lv3complete(){ clearlvComplete(); gladius.SetActive(true);}
    public void level1(){closeAll();level1_1.SetActive(true);closeins();}
    public void level2(){closeAll();level1_2.SetActive(true); closeins();}
    public void level3(){closeAll();level1_3.SetActive(true); closeins();}

    public void levelComplete() { closeAll(); end.SetActive(true); }

}
