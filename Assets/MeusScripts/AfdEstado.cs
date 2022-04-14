using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfdEstado : MonoBehaviour
{
    private string nome;
    private bool inicial;
    private bool final;

    void Awake()
    {
        inicial = false;
        final = false;
    }

    public string GetNome()
    {
        return nome;
    }
    public void SetNome(string nome)
    {
        this.nome = nome;
    }
    public void SetInicial(bool inicial)
    {
        this.inicial = inicial;
    }
    public bool GetInicial()
    {
        return inicial;
    }
    public void SetFinal(bool final)
    {
        this.final = final;
    }
    public bool GetFinal()
    {
        return final;
    }
}