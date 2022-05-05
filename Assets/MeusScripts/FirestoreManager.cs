using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Database;
using TMPro;
using static System.Guid;


public class FirestoreManager : MonoBehaviour
{
    public GameObject workspace;
    public GameObject enunciadoObj;


    [SerializeField] TMP_Text enunciadoText;
    [SerializeField] Button salvar;
    FirebaseFirestore db;

    //Uptade Automatico
    //ListenerRegistration listenerRegistration;


    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        salvar.onClick.AddListener(OnHandleClick);
        //listenerRegistration = db.Collection("Exercicies").Document("exercicie").Listen(snapshot =>
        //{
        //    FirestoreStruct firestoreStruct = snapshot.ConvertTo<FirestoreStruct>();
        //    enunciadoText.text = firestoreStruct.ToString();
        //});

    }
    public void AbrirWorkspace()
    {
        StartCoroutine(GetData());
    }
    void OnHandleClick()
    {
        // Struct
        FirestoreStruct firestoreStruct = new FirestoreStruct
        {
            IdUser = StateNameController.IdUser,
            Enunciado = workspace.GetComponent<Workspace>().GetEnunciado()
        };

        if (StateNameController.IdProject == "")
        {
            GenerateId();
        }

        DocumentReference exercisesRef = db.Collection("exercises").Document(StateNameController.IdProject);
        exercisesRef.SetAsync(firestoreStruct).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Exerc�cio salvo");
            SSTools.ShowMessage("Exerc�cio salvo", SSTools.Position.bottom, SSTools.Time.threeSecond);
            Debug.Log(StateNameController.IdProject);
        });

    }
    public IEnumerator GetData()
    {
        var DBTask2 = db.Collection("exercises").Document(StateNameController.IdProject).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            FirestoreStruct firestoreStruct = task.Result.ConvertTo<FirestoreStruct>();
            workspace.GetComponent<Workspace>().SetEnunciado(firestoreStruct.Enunciado);
            enunciadoObj.GetComponent<Enunciado>().AtualizarQuintupla();
        });
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
    }

    public void GenerateId()
    {
        var newId = NewGuid();
        StateNameController.IdProject = newId.ToString();
    }

}