using Firebase.Auth;
using System.Collections;
using TMPro;
using UnityEngine;
using Firebase.Database;
using Firebase;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour
{
    
    private FirebaseAuth auth;
    public DatabaseReference DBreference;
    public TMP_Text scoreOut;

    [Header("Login")]
    public TMP_InputField email_LoginField;
    public TMP_InputField password_LoginField;
    public TMP_Text LoginFieldsStatus;

    [Header("Register")]
    public TMP_InputField NickNameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordConfirmRegister;
    public TMP_Text RegisterFieldStatus;

    public void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }
  
    public void RegisterButton()
    {
        StartCoroutine(OnregisterButton(NickNameRegisterField.text,emailRegisterField.text, passwordRegisterField.text,passwordConfirmRegister.text));
    }
    public void LoginButton()
    {
        StartCoroutine(OnLoginButton(email_LoginField.text, password_LoginField.text));
    }
    private IEnumerator OnregisterButton(string Nickname, string email, string password, string confirmPassword)
    {
        if (Nickname == "")
        {
            //If the username field is blank show a warning
            RegisterFieldStatus.text = "Missing Username";
        }
        else if (emailRegisterField.text == "")
        {
            RegisterFieldStatus.text = "Enter Email";
        }
        else if(passwordRegisterField.text=="" || passwordConfirmRegister.text == "")
        {
            RegisterFieldStatus.text = "Password Fields Empty";
        }
        else if (passwordRegisterField.text != passwordConfirmRegister.text)
        {
            //If the password does not match show a warning
            RegisterFieldStatus.text = "Password Does Not Match!";
        }
        else
        {

            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            }

            Firebase.Auth.AuthResult result = registerTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            Firebase.Auth.FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                string name = Nickname;
                string Email = user.Email;
                string uid = user.UserId;
                
                
                CentralUserManager.instance.writeNewUser(uid, name, Email);
                Landing_UIManager.instance.LoginWindow();

            }
        }
    }
 
    private IEnumerator OnLoginButton(string email, string password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            LoginFieldsStatus.text = message;
        }
        else
        {

            Firebase.Auth.AuthResult result = LoginTask.Result;
            Debug.LogFormat("User signed in successfully");
            Debug.Log(result.User.UserId);
            CentralUserManager.instance.GetUserDataF(result.User.UserId);
            Landing_UIManager.instance.Menupage();

        }

    }
}