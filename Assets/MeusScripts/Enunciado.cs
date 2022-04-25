using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enunciado : MonoBehaviour
{
    public GameObject workspace;
    public TMP_Text linguagemText;
    public TMP_Text quintuplaText;

    string alfabetoText = "Σ";
    string estadosText = "Q";
    string estadoInicialText = "Q?";
    string estadosFinaisText = "F";


    public void ZerarEnunciado()
    {
        linguagemText.text = workspace.GetComponent<Workspace>().GetLinguagem();
        alfabetoText = "Σ";
        estadosText = "Q";
        estadoInicialText = "Q?";
        estadosFinaisText = "F";
        AtualizarQuintupla();
    }
    public void AtulizarLinguagem()
    {
        if (workspace.GetComponent<Workspace>().GetLinguagem() != "")
        {
            linguagemText.text = workspace.GetComponent<Workspace>().GetLinguagem();
        }
        else
        {
            linguagemText.text = "Linguagem";
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
        string[] NomeDosEstadosFinais = workspace.GetComponent<Workspace>().GetNomeDosEstadosFinais();
        string estadosFinaisText = "";
        int quantosEstadosFaltam = workspace.GetComponent<Workspace>().GetQuantosEstadosFinais();
        for (int i = 0; i < NomeDosEstadosFinais.Length; i++)
        {
            if (NomeDosEstadosFinais[i] != null)
            {
                estadosFinaisText += NomeDosEstadosFinais[i];
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
        quintuplaText.text = "({" + estadosText + "}, " + "{" + alfabetoText + "}, " + estadoInicialText + ", " + "δ, " + "{" + estadosFinaisText + "})";
    }
}
