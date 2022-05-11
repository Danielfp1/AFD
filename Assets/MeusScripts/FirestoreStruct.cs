using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public struct FirestoreStruct
{
    [FirestoreProperty]
    public string IdUser { get; set; }
    [FirestoreProperty]
    public string Enunciado { get; set; }
    [FirestoreProperty]
    public int quantosEstados { get; set; }
    [FirestoreProperty]
    public string[] alfabeto { get; set; }
    [FirestoreProperty]
    public string[] estados { get; set; }
    [FirestoreProperty]
    public List<float> estadosPosX { get; set; }
    [FirestoreProperty]
    public List<float> estadosPosY { get; set; }
    [FirestoreProperty]
    public string estadoInicial { get; set; }
    [FirestoreProperty]
    public List<string> estadosFinais { get; set; }
    [FirestoreProperty]
    public List<string> transistionStates1 { get; set; }
    [FirestoreProperty]
    public List<string> transistionStates2 { get; set; }
    [FirestoreProperty]
    public List<string> transistionSymbols { get; set; }
}
