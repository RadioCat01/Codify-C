using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CentralUserManager : MonoBehaviour
{
    public static CentralUserManager instance;
    public TMP_Text scoreOut;
    public DatabaseReference DBreference;
    public class User
    {
        public string Nickname;
        public string Email;
        public int level;

        public User()
        {
        }

        public User(string username, string email, int level)
        {
            this.Email = email;
            this.Nickname = username;
            this.level = level;
        }
    }

    public void Awake()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        instance=this;
    }

    public void writeNewUser(string userId, string name, string email)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        int level = 1;
        User user = new User(name, email, level);
        string json = JsonUtility.ToJson(user);

        reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void GetUserDataF(string UID)
    {
        StartCoroutine(GetUserData(UID));

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
            int level = int.Parse(userDict["level"].ToString());

        }
        else
        {
            Debug.Log("User data does not exist.");
        }
    }
}
