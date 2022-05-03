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
        GetListaDeExercicios();
        //Listar();
        Listar2();


    }
    public void VoltarMenuPrincipal()
    {
        SceneManager.LoadScene(1);
    }
    public void GetListaDeExercicios()
    {
        db.Collection("exercises").GetSnapshotAsync().ContinueWithOnMainThread(task =>
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
    }
    public void Listar()
    {
        //for (int i = 0; i < quantidade; i++)
        //{
        db.Collection("exercises").Document(listaDeId[0]).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            FirestoreStruct firestoreStruct = task.Result.ConvertTo<FirestoreStruct>();
            GameObject listElementObj = Instantiate(listElementPrefab, listaContent);
            listElementObj.GetComponent<ListElement>().CriarElemento(listaDeId[0], firestoreStruct.IdUser, firestoreStruct.Enunciado);
        });
        //}
    }
    public void Listar2()
    {
        for (int i = 0; i < quantidade; i++)
        {
            GameObject listElementObj = Instantiate(listElementPrefab, listaContent);
            listElementObj.GetComponent<ListElement>().CriarElemento(listaDeId[i], listaDeId[i], listaDeId[i]);
        }
    }
}