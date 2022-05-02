using Firebase.Firestore;

[FirestoreData]
public struct FirestoreStruct
{
    [FirestoreProperty]
    public string IdUser { get; set; }
    [FirestoreProperty]
    public string Enunciado { get; set; }

}
