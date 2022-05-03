using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Database;
using TMPro;

public class Menu_ListaDeExercicios : MonoBehaviour
{
    [Header("Lista de Exercícios")]
    public Transform listaContent;
    public GameObject listElementPrefab;
    FirebaseFirestore db;

    List<string> listaDeId = new List<string>();
    int quantidade = 0;

    private void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
        StartCoroutine(GetListaDeExercicios());
        //Listar2();


    }
    public void VoltarMenuPrincipal()
    {
        SceneManager.LoadScene(1);
    }
    public IEnumerator GetListaDeExercicios()
    {
        var DBTask2 = db.Collection("exercises").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            var DBTask = task.Result;
            quantidade = task.Result.Count;
            for (int i = 0; i < quantidade; i++)
            {
                DocumentSnapshot snapshot = DBTask[i];
                listaDeId.Add(snapshot.Id);
                Debug.Log(snapshot.Id);
            };
            Debug.Log("Acessando Dados");
        });
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        StartCoroutine(Listar());
    }
    public IEnumerator Listar()
    {
        for (int i = 0; i < quantidade; i++)
        {
            var DBTask2 = db.Collection("exercises").Document(listaDeId[i]).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                FirestoreStruct firestoreStruct = task.Result.ConvertTo<FirestoreStruct>();
                GameObject listElementObj = Instantiate(listElementPrefab, listaContent);
                listElementObj.GetComponent<ListElement>().CriarElemento(listaDeId[i], firestoreStruct.IdUser, firestoreStruct.Enunciado);
            });
            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        }
    }
}