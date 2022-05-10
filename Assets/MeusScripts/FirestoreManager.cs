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
    }
    public void AbrirWorkspace()
    {
        StartCoroutine(GetData());
    }
    public void OnHandleClick()
    {
        workspace.GetComponent<Workspace>().GetEstadosPos();
        // Struct
        FirestoreStruct firestoreStruct = new FirestoreStruct
        {
            IdUser = StateNameController.IdUser,
            Enunciado = workspace.GetComponent<Workspace>().GetEnunciado(),
            alfabeto = workspace.GetComponent<Workspace>().GetAlfabetoString(),
            quantosEstados = workspace.GetComponent<Workspace>().GetQuantosEstados(),
            estados = workspace.GetComponent<Workspace>().GetEstadosBd(),
            estadosPos = workspace.GetComponent<Workspace>().GetEstadosPos(),
        };

        if (StateNameController.IdProject == "")
        {
            GenerateId();
        }

        DocumentReference exercisesRef = db.Collection("projectsProfessores").Document(StateNameController.IdUser).Collection("projects").Document(StateNameController.IdProject);
        exercisesRef.SetAsync(firestoreStruct).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Exercício salvo");
            SSTools.ShowMessage("Exercício salvo", SSTools.Position.bottom, SSTools.Time.threeSecond);
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
