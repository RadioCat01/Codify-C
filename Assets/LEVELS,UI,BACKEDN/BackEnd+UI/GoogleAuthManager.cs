using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using System.Collections;
public class GoogleAuth : MonoBehaviour
{
   
    public string webClientId = "";
   

    public static GoogleAuth instance;

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
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


    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    
                }
                else
                {
                    
                }
            }
        }
        else if (task.IsCanceled)
        {
            
        }
        else
        {
        
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }
    private void checkIfUserExists(string uid, Action<DataSnapshot> callback)
    {
        // Get a reference to the user node
        var userRef = FirebaseDatabase.DefaultInstance.GetReference("users/" + uid);

        userRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error checking for user: " + task.Exception);
                callback(null); // Handle error or provide default value
                return;
            }
            DataSnapshot snapshot = task.Result;
            callback(snapshot);
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
      

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

      

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

  


    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0)) ;
                    
            }
            else
            {

            }
            FirebaseUser user = task.Result;
            string uid = user.UserId;
            string name = user.DisplayName;
            string email = user.Email;


            checkIfUserExists(uid, user =>
            {
                if (!user.Exists) // User doesn't exist, write new user data
                {
                    CentralUserManager.instance.writeNewUser(uid,name,email);
                }
                else
                {
                   CentralUserManager.instance.GetUserDataF(uid);
                }
            });
        });
    }

}