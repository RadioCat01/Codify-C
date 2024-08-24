using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class FBAuthManager : MonoBehaviour
{
    public FirebaseAuth auth;
    public TMP_Text getNickname;
    public TMP_Text scoreOut;
    public DatabaseReference DBreference;


    void Awake()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        if (!FB.IsInitialized)
        {
            
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
           
            FB.ActivateApp();
           
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
      
            Time.timeScale = 0;
        }
        else
        {
          
            Time.timeScale = 1;
        }
    }

    public void FBLogin()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

            var aTokenSt = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
            Debug.Log(aToken.UserId);
           
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
            FBauth(aTokenSt);

            Landing_UIManager.instance.Menupage();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public void FBauth(string accessToken)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            string uid = result.User.UserId;

            checkIfUserExists(uid, user =>
            {
                if (!user.Exists) // User doesn't exist, write new user data
                {
                    CentralUserManager.instance.writeNewUser(uid, result.User.DisplayName,result.User.Email);
                    CentralUserManager.instance.GetUserDataF(uid);
                }
            });
        });
    }
    private void checkIfUserExists(string uid, Action<DataSnapshot> callback)
    {
        var userRef = FirebaseDatabase.DefaultInstance.GetReference("users/" + uid);

        userRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error checking for user: " + task.Exception);
                callback(null); 
            }
            DataSnapshot snapshot = task.Result;
            callback(snapshot);
        });
      
    }
}