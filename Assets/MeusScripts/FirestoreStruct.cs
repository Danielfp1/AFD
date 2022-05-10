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
    public List<float> estadosPos { get; set; }
}
