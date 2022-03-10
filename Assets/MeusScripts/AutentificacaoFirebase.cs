using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement; // Para mudar de scenes


public class AutentificacaoFirebase : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Erro no Login";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Digite seu email";
                    break;
                case AuthError.MissingPassword:
                    message = "Digite sua senha";
                    break;
                case AuthError.WrongPassword:
                    message = "Senha invalida";
                    break;
                case AuthError.InvalidEmail:
                    message = "Email invalido";
                    break;
                case AuthError.UserNotFound:
                    message = "Conta n�o existe";
                    break;
            }
            //warningLoginText.text = message;
            SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("Usuario Logado {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            //confirmLoginText.text = "Logado";////////////////////////////
            SceneManager.LoadScene(1); // Abrir a tela de menu principal

            Debug.Log("Logado!!!");
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            //warningRegisterText.text = "Digite seu nome";
            SSTools.ShowMessage("Digite seu nome", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            //warningRegisterText.text = "As senhas n�o s�o iguais";
            SSTools.ShowMessage("As senhas n�o s�o iguais", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Erro no cadastro";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Digite seu email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Digite sua senha";
                        break;
                    case AuthError.WeakPassword:
                        message = "Senha fraca";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email j� cadastrado";
                        break;
                }
                //warningRegisterText.text = message;
                SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen

                        //UIManager.instance.LoginScreen(); !!!!!!!!!!!!!!!!!!!!

                        //GameObject tela;
                        //tela = GetComponent<Menu_Cadastro>().gameObject;
                        //tela.SetActive(false);
                        //warningRegisterText.text = "";
                        //Debug.Log("Cadastrado!!!");

                        SceneManager.LoadScene(0); // Abrir a tela de menu principal
                    }
                }
            }
        }
    }
}