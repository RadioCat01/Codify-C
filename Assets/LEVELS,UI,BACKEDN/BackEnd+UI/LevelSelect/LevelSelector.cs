using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] buttons;
    DatabaseReference DBreference;
    int unlockedLevel;
    private void Awake()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        string uId = user.UserId;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(GetUserData(uId));

    }

    private void Update()
    {

    }
    public IEnumerator GetUserData(string userId)
    {
        DatabaseReference userRef = DBreference.Child("users").Child(userId);
        var task = userRef.GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError(task.Exception.ToString());
        }
        else if (task.Result.Exists)
        {

            DataSnapshot snapshot = task.Result;
            IDictionary userDict = (IDictionary)snapshot.Value;

            string username = userDict["Nickname"].ToString();
            int score = int.Parse(userDict["level"].ToString());

            unlockedLevel = score;
            Debug.Log(unlockedLevel);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }
            for (int i = 0; i < unlockedLevel; i++)
            {
                buttons[i].interactable = true;
            }
        }
        else
        {
            Debug.Log("User data does not exist.");
        }
    }
    public void OpenLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }
}
