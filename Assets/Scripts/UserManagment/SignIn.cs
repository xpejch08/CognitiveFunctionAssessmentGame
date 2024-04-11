using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SignIn : MonoBehaviour
{
    private FirebaseAuth auth;
    private string _email;
    private string _password;
    public TextMeshProUGUI ErrorText;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public GameObject editorPanel;
    

    void Start()
    {
        InitializeFirebase();
        editorPanel.SetActive(false);
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
    }

    private void Update()
    {
        if (Input.touchCount > 0)   
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                editorPanel.SetActive(false);
            }
        }
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result != DependencyStatus.Available)
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                return;
            }
            
            auth = FirebaseAuth.DefaultInstance;
        });
    }

    public void OnSignInButtonPressed()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                editorPanel.SetActive(true);
                ErrorText.text = "Error: " + task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }

            
            FirebaseUser newUser = task.Result.User;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.Email);
        });
    }

    public void OnLogInButtonPressed()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                editorPanel.SetActive(true);
                ErrorText.text = "Error: " + task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }

            FirebaseUser newUser = task.Result.User;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.Email);
        });
    }
    
    public void OnLogOutButtonPressed()
    {
        auth.SignOut();
        GameManager.isGuest = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SignIn");
    }
    
    public void PlayAsGuest()
    {
        GameManager.isGuest = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}