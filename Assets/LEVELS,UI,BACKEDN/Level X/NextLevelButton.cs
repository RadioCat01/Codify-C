using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public int levelNum;
    DatabaseReference DBreference;
    FirebaseUser user;
    private void Awake()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;


    }
    public void nextLevel()
    {
        string uId = user.UserId;
        UpdateUserLevel(uId, levelNum);
       
    }
    public void UpdateUserLevel(string userId, int level)
    {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        userRef.Child("users").Child(userId).Child("level").SetValueAsync(level);
    }

    public void back()
    {
        SceneManager.LoadScene(0);
    }
}
