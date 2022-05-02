using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;


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
    void OnHandleClick()
    {
        // Struct
        FirestoreStruct firestoreStruct = new FirestoreStruct
        {
            IdUser = StateNameController.IdUser,
            Enunciado = workspace.GetComponent<Workspace>().GetEnunciado()
        };

        DocumentReference StructRef = db.Collection("exercises").Document("exercise");
        StructRef.SetAsync(firestoreStruct).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Atualizando Dados");
        });

    }
    void GetData()
    {
        db.Collection("exercises").Document("exercise").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            FirestoreStruct firestoreStruct = task.Result.ConvertTo<FirestoreStruct>();
            workspace.GetComponent<Workspace>().SetEnunciado(enunciadoText.ToString());
            enunciadoObj.GetComponent<Enunciado>().AtualizarQuintupla();
        });
    }

}
