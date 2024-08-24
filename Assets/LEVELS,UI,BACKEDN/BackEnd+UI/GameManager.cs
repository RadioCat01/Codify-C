using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreOut;
    public int currentScore;
    DatabaseReference DBreference;
    void Awake()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toLevel()
    {
        SceneManager.LoadScene("Level X");
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
            int score = int.Parse(userDict["score"].ToString());


            scoreOut.text = $"{score}";
        }
        else
        {
            Debug.Log("User data does not exist.");
        }
    }

    public void IncreaseScoreButton()
    {
        int currentScore = int.Parse(scoreOut.text);
        Debug.Log(currentScore);
        currentScore++;
        Debug.Log(currentScore);
      
        scoreOut.text = currentScore.ToString();

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        
        UpdateUserScore(auth.CurrentUser.UserId, currentScore);
    }
    public void UpdateUserScore(string userId, int newScore)
    {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        userRef.Child("users").Child(userId).Child("score").SetValueAsync(newScore);
    }
    public class User
    {
        public string username;
        public int score;

        public User()
        {
        }

        public User(string username, int score)
        {
            this.username = username;
            this.score = score;
        }
    }
}
