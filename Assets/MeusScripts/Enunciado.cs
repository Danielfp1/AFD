using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enunciado : MonoBehaviour
{
    public GameObject workspace;
    public TMP_Text linguagemText;
    public TMP_Text quintuplaText;
    string alfabetoJunto = "";
    string estados = "";
    string estadoInicial = "";
    string estadosFinais = "";



    public void AtulizarAlfabeto()
    {

        char[] alfabeto = workspace.GetComponent<Workspace>().GetAlfabeto();
        for (int i = 0; i < alfabeto.Length; i++)
        {
            alfabetoJunto = alfabetoJunto + alfabeto[i].ToString();
            if (!(i == alfabeto.Length - 1))
            {
                alfabetoJunto = alfabetoJunto + ", ";
            }
        }
    }

    public void AtulizarEstados()
    {
        //GameObject[] estados = workspace.GetComponent<Workspace>().GetQuantosEstados();
    }
}
