using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enunciado : MonoBehaviour
{
    public GameObject workspace;
    public TMP_Text enunciadoText;
    public TMP_Text quintuplaText;

    string alfabetoText = "Σ";
    string estadosText = "Q";
    string estadoInicialText = "Q?";
    string estadosFinaisText = "F";


    public void ZerarEnunciado()
    {
        enunciadoText.text = workspace.GetComponent<Workspace>().GetEnunciado();
        alfabetoText = "Σ";
        estadosText = "Q";
        estadoInicialText = "Q?";
        estadosFinaisText = "F";
        AtualizarQuintupla();
    }
    public void AtulizarEnunciado()
    {
        if (workspace.GetComponent<Workspace>().GetEnunciado() != "")
        {
            enunciadoText.text = workspace.GetComponent<Workspace>().GetEnunciado();
        }
        else
        {
            enunciadoText.text = "Enunciado";
        }
    }
    public void AtulizarAlfabeto()
    {
        char[] alfabeto = workspace.GetComponent<Workspace>().GetAlfabeto();
        alfabetoText = "";
        for (int i = 0; i < alfabeto.Length; i++)
        {
            alfabetoText = alfabetoText + alfabeto[i].ToString();
            if (!(i == alfabeto.Length - 1))
            {
                alfabetoText = alfabetoText + ", ";
            }
        }
        AtualizarQuintupla();
    }

    public void AtulizarEstados()
    {
        string[] NomeDosEstados = workspace.GetComponent<Workspace>().GetNomeDosEstados();
        string estadosText = "";
        int quantosEstadosFaltam = workspace.GetComponent<Workspace>().GetQuantosEstados();
        for (int i = 0; i < NomeDosEstados.Length; i++)
        {
            if (NomeDosEstados[i] != null)
            {
                estadosText += NomeDosEstados[i];
                if (quantosEstadosFaltam != 1)
                {
                    estadosText += ", ";
                }
                quantosEstadosFaltam--;
            }
        }
        if (estadosText != "")
        {
            this.estadosText = estadosText;
        }
        else
        {
            this.estadosText = "Q";
        }
        AtualizarQuintupla();
    }

    public void AtulizarEstadoInicial()
    {
        string estadoInicial = "";
        if (workspace.GetComponent<Workspace>().GetPossuiEstadoInicial())
        {
            estadoInicial = workspace.GetComponent<Workspace>().GetEstadoInicial().GetComponent<Estado>().GetNomeDoEstado();
        }
        if (estadoInicial != "")
        {
            this.estadoInicialText = estadoInicial;
        }
        else
        {
            this.estadoInicialText = "Q?";
        }
        AtualizarQuintupla();
    }

    public void AtulizarEstadosFinais()
    {
        List<string> nomeDosEstadosFinais = workspace.GetComponent<Workspace>().GetNomeDosEstadosFinais();
        string estadosFinaisText = "";
        int quantosEstadosFaltam = nomeDosEstadosFinais.Count;
        foreach(string nomeDoEstadoFinal in nomeDosEstadosFinais)
        {
            if (nomeDoEstadoFinal != null)
            {
                estadosFinaisText += nomeDoEstadoFinal;
                if (quantosEstadosFaltam != 1)
                {
                    estadosFinaisText += ", ";
                }
                quantosEstadosFaltam--;
            }
        }
        if (estadosFinaisText != "")
        {
            this.estadosFinaisText = estadosFinaisText;
        }
        else
        {
            this.estadosFinaisText = "F";
        }
        AtualizarQuintupla();
    }

    public void AtualizarQuintupla()
    {
        quintuplaText.text = "({" + estadosText + "}, " + "{" + alfabetoText + "}, " + "δ, " + estadoInicialText + ", " + "{" + estadosFinaisText + "})";
    }
}
