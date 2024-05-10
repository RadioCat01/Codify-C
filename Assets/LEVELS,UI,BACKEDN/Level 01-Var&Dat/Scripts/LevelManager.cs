using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public static NextLevelButton levelInstance;

    public int levelNum;
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
        string uId = user.UserId;
        UpdateUserLevel(uId, levelNum);
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
