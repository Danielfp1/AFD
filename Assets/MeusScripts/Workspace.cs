using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Workspace : MonoBehaviour
{
    public GameObject menuEstadoObj;
    public int quantosEstados = 2;
    public GameObject[] estados = new GameObject[20]; // limite de estados é 20
    public GameObject estadoAtual;
    public bool novaTransFlag;
    public GameObject estadoAlvo;


    public int GetQuantosEstados()
    {
        return quantosEstados;
    }

    public void SetQuantosEstados(int quantidade)
    {
        quantosEstados = quantidade;
    }
    
    public void AddEstado(GameObject novoEstado)
    {
        estados[quantosEstados] = novoEstado;
        SetQuantosEstados(quantosEstados+1);
    }
    public void AbrirMenuEstado(GameObject estadoAtual) //Passar estado como parametro!!! e pegar outro para trasisção
    {
        menuEstadoObj.transform.position = GetPosicaoMouse();
        menuEstadoObj.SetActive(true);
        Debug.Log(estadoAtual.name);
        SetEstadoAtual(estadoAtual);
        
    }
    public void FecharMenuEstado()
    {
        menuEstadoObj.SetActive(false);
    }
    Vector3 GetPosicaoMouse()
    {
        var posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0;
        return posicaoMouse;
    }

    public void SetEstadoAtual(GameObject estado)
    {
        this.estadoAtual = estado;
    }

    public GameObject GetEstadoAtual()
    {
        return estadoAtual;
    }

    public void SetEstadoAlvo(GameObject estado)
    {
        this.estadoAlvo = estado;
    }

    public GameObject GetEstadoAlvo()
    {
        return estadoAlvo;
    }
    public void SetNovaTransFlag(bool flag)
    {
        this.novaTransFlag = flag; 
    }

    public bool GetNovaTransFlag()
    {
        return this.novaTransFlag;
    }

}
