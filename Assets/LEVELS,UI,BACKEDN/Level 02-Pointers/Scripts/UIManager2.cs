using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager2:MonoBehaviour
{
    public static UIManager2 instance;

    public GameObject level;
    public GameObject intro;
    public GameObject outro;
    public GameObject ins1;
    public GameObject ins2;
    public GameObject ins3;
    public GameObject ins4;
    public GameObject ins5;
    public GameObject ins6;

    public void clearallins()
    {
        intro.SetActive(false);
        ins1.SetActive(false);
        ins2.SetActive(false);
        ins3.SetActive(false);
        ins4.SetActive(false);
        ins5.SetActive(false);
        ins6.SetActive(false);
    }
    public void Openins6()
    {
        ins6.SetActive(true);
    }

    public void openins1() { clearallins(); ins1.SetActive(true); }
    public void openins2() { clearallins(); ins2.SetActive(true); }
    public void openins3() { clearallins(); ins3.SetActive(true); }
    public void openins4() { clearallins(); ins4.SetActive(true); }
    public void openins5() { clearallins(); ins5.SetActive(true); }

    public void Outro()
    {
        clearallins();
        level.SetActive(false);
        outro.SetActive(true);
    }
}
