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
    public void AbrirWorkspace() //Abrir banco de dados
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
            transistionStates1 = workspace.GetComponent<Workspace>().GetListaTransistionStates1(),
            transistionStates2 = workspace.GetComponent<Workspace>().GetListaTransistionStates2(),
            transistionSymbols = workspace.GetComponent<Workspace>().GetListaSymbols()
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
        var DBTask2 = db.Collection("projectsProfessores").Document(StateNameController.IdUser).Collection("projects").Document(StateNameController.IdProject).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            FirestoreStruct firestoreStruct = task.Result.ConvertTo<FirestoreStruct>();
            workspace.GetComponent<Workspace>().SetEnunciado(firestoreStruct.Enunciado);
            enunciadoObj.GetComponent<Enunciado>().AtulizarEnunciado();
            workspace.GetComponent<Workspace>().SetAlfabeto(firestoreStruct.alfabeto);
            workspace.GetComponent<Workspace>().SetQuantosEstados(firestoreStruct.quantosEstados);
            workspace.GetComponent<Workspace>().SetEstadosBd(firestoreStruct.estados);
            workspace.GetComponent<Workspace>().SetEstadosPos(firestoreStruct.estadosPos);
            workspace.GetComponent<Workspace>().SetListaTransistionStates1(firestoreStruct.transistionStates1);
            workspace.GetComponent<Workspace>().SetListaTransistionStates2(firestoreStruct.transistionStates2);
            workspace.GetComponent<Workspace>().SetListaSymbols(firestoreStruct.transistionSymbols);
        });
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        workspace.GetComponent<Workspace>().InstanciarEstados();
        enunciadoObj.GetComponent<Enunciado>().AtulizarAlfabeto();
        workspace.GetComponent<Workspace>().InstanciarTransitions();
    }

    public void GenerateId()
    {
        var newId = NewGuid();
        StateNameController.IdProject = newId.ToString();
    }

}
