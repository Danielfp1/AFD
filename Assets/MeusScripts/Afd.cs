using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afd : MonoBehaviour
{
    public string[] qEstados = new string[20];
    public char[] alfabeto;
    public string linguagem;

    public void SetQEstados (string[] qEstados)
    {
        this.qEstados = qEstados;
    }

    public string[] GetqEstados()
    {
        return qEstados;
    }
    public void SetAlfabeto(char[] alfabeto)
    {
        this.alfabeto = alfabeto;
    }
    public char[] GetAlfabeto()
    {
        return alfabeto;
    }
    public void SetLinguagem(string linguagem)
    {
        this.linguagem = linguagem;
    }
    public string GetLinguagem()
    {
        return linguagem;
    }
}
