using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class NextLevelButton : MonoBehaviour
{
    public static NextLevelButton levelInstance;

    public bool islevelPassed=false;
    public int levelNum;
    public int DBLvnum;
    DatabaseReference DBreference;
    FirebaseUser user;
    private void Awake()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        levelInstance = this;

    }
    public void updateUserLevel()
    {
        if (!islevelPassed)
        {   
            islevelPassed = true;
            Debug.Log("backedn called");
            string uId = user.UserId;
            var reference = FirebaseDatabase.DefaultInstance.GetReference($"users/{uId}");


            reference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("task complete");
                    DataSnapshot snapshot = task.Result;
                    IDictionary userDict = (IDictionary)snapshot.Value;
                    DBLvnum = int.Parse(userDict["level"].ToString());
                    levelNum = DBLvnum + 1;
                    Debug.Log(levelNum);
                    UpdateUserLevel(uId, levelNum);
                }
            });

           
        }
    }
    public void UpdateUserLevel(string userId, int level)
    {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        userRef.Child("users").Child(userId).Child("level").SetValueAsync(level);
    }



    public void loadNextLevel()
    {

    }
   
    public void back()
    {
        SceneManager.LoadScene(0);
     
    }
}
