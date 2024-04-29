using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateListener : MonoBehaviour
{
    private Firebase.Auth.FirebaseAuth auth;
     void Awake()
    {
        DontDestroyOnLoad(gameObject);
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    private void Start()
    {
        auth.StateChanged += AuthStateChanged;
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            Debug.Log("User logged in");
            Landing_UIManager.instance.Menupage();
        }
        else
        {
            Debug.Log("No user");
            Landing_UIManager.instance.Landingpage();
        }
    }

    public void Logout()
    {
        auth.SignOut();
        Landing_UIManager.instance.Landingpage();
    }
}
