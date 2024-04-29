using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing_UIManager : MonoBehaviour
{
    public static Landing_UIManager instance;

    public GameObject GuestWindow;
    public GameObject LandingPage;
    public GameObject loginWindow;
    public GameObject RegisterWindow;
    public GameObject MenuWindow;
    public GameObject LevelSelect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    public void Guestwindow()
    {
        LandingPage.SetActive(false);
        GuestWindow.SetActive(true);
        
    }
    public void Landingpage()
    {
        GuestWindow.SetActive(false);
        LandingPage.SetActive(true);
        MenuWindow.SetActive(false);
    }

    public void Menupage()
    {
        GuestWindow.SetActive(false);
        LandingPage.SetActive(false);
        MenuWindow.SetActive(true);
    }

    public void LoginWindow()
    {
        loginWindow.SetActive(true);
        RegisterWindow.SetActive(false);
    }
    public void Registerwindow()
    {
        loginWindow.SetActive(false);
        RegisterWindow.SetActive(true);
    }
    public void LevelSelectWindow()
    {
        LandingPage.SetActive(false);
        GuestWindow.SetActive(false);
        MenuWindow.SetActive(false);
        LevelSelect.SetActive(true);
       
    }
}
