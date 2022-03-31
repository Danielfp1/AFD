using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Workspace : MonoBehaviour
{
    public GameObject menuEstadoObj;
    public int quantosEstados = 2;
    public GameObject[] estados = new GameObject[20]; // limite de estados � 20
    public GameObject estadoAtual;
    public bool novaTransFlag;
    public GameObject estadoAlvo;


    public int getQuantosEstados()
    {
        return quantosEstados;
    }

    public void setQuantosEstados(int quantidade)
    {
        quantosEstados = quantidade;
    }
    
    public void addEstado(GameObject novoEstado)
    {
        estados[quantosEstados] = novoEstado;
        setQuantosEstados(quantosEstados+1);
    }
    public void abrirMenuEstado(GameObject estadoAtual) //Passar estado como parametro!!! e pegar outro para trasis��o
    {
        menuEstadoObj.transform.position = getPosicaoMouse();
        menuEstadoObj.SetActive(true);
        Debug.Log(estadoAtual.name);
        SetEstadoAtual(estadoAtual);
        
    }
    public void FecharMenuEstado()
    {
        menuEstadoObj.SetActive(false);
    }
    Vector3 getPosicaoMouse()
    {
        var posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0;
        return posicaoMouse;
    }

    private void SetEstadoAtual(GameObject estado)
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
