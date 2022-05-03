using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ListElement : MonoBehaviour
{
    public TMP_Text nome;
    public TMP_Text enunciado;
    public string idProject;
    public string idUser;

    public Button buttonClick;

    public void CriarElemento (string idProject,string idUser, string enunciado)
    {
        this.idUser = idUser;
        this.enunciado.text = enunciado;
        this.idProject = idProject;
    }
}
