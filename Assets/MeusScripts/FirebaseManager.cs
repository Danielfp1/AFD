using System;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement; // Para mudar de scenes


public class FirebaseManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    public DatabaseReference DBreference; // Banco de dados

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    //public TMP_Text warningLoginText;
    //public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Dropdown userTypeRegisterVerifyField;
    //public TMP_Text warningRegisterText;

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
        DBreference = FirebaseDatabase.DefaultInstance.RootReference; // Banco de dados!!!!!!!!!!!!!
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
        bool userTypebool = Convert.ToBoolean(userTypeRegisterVerifyField.value);
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text, userTypebool));
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
                    message = "Conta não existe";
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

            //Get the currently logged in user data
            var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                Debug.LogWarning(message: $"Não foi possivel acessar o banco de dados {DBTask.Exception}");
            }
            else
            {
                //Data has been retrieved
                DataSnapshot snapshot = DBTask.Result;

                //SSTools.ShowMessage(snapshot.Child("userType").Value.ToString(), SSTools.Position.bottom, SSTools.Time.threeSecond);

                if (snapshot.Child("userType").Value.ToString() == "True")
                {

                    SceneManager.LoadScene("MainMenu_Professor"); // Abrir a tela de menu principal aluno
                    //SSTools.ShowMessage("Professor Logado", SSTools.Position.bottom, SSTools.Time.oneSecond);
                }
                else
                {
                    SceneManager.LoadScene("MainMenu_Aluno"); // Abrir a tela de menu principal professor
                    //SSTools.ShowMessage("Aluno Logado", SSTools.Position.bottom, SSTools.Time.oneSecond);
                }

                Debug.Log("Logado!!!");
                //StateNameController
                StateNameController.IdUser = User.UserId;
            }
        }
    }

    private IEnumerator Register(string _email, string _password, string _username, bool _userType)
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
            //warningRegisterText.text = "As senhas não são iguais";
            SSTools.ShowMessage("As senhas não são iguais", SSTools.Position.bottom, SSTools.Time.threeSecond);
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

                string message = "Email inválido";
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
                        message = "Email já cadastrado";
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
                        //warningRegisterText.text = "Username Set Failed!";
                        SSTools.ShowMessage("Falha ao registrar nome", SSTools.Position.bottom, SSTools.Time.threeSecond);
                    }
                    else
                    {
                        
                        var DBTask = DBreference.Child("users").Child(User.UserId).Child("userType").SetValueAsync(_userType);
                        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
                        if (DBTask.Exception != null)
                        {
                            Debug.LogWarning(message: $"Erro no registro da tarefa com o: {DBTask.Exception}");
                        }
                        else
                        {
                            if (_userType)
                            {
                                SSTools.ShowMessage("Cadastrado realizado", SSTools.Position.bottom, SSTools.Time.threeSecond);
                                SceneManager.LoadScene(0);
                            }
                            else
                            {
                                SSTools.ShowMessage("Cadastrado realizado", SSTools.Position.bottom, SSTools.Time.threeSecond);
                                SceneManager.LoadScene(0);
                            }
                            //username is now set

                            //now return to login screen

                            //UIManager.instance.LoginScreen(); //!!!!!!!!!!!!!!!!!!!!



                            //gameobject tela;
                            //tela = getcomponent<menu_cadastro>().gameobject;
                            //tela.setactive(false);
                            //warningregistertext.text = "";
                            //debug.log("cadastrado!!!");
                            //SceneManager.LoadScene(1); // abrir a tela de menu principal
                        };
                    }
                }
            }
        }
    }
}