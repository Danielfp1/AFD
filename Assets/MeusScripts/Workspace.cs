using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Workspace : MonoBehaviour
{
    public GameObject menuEstadoObj;
    public int quantosEstados = 2;
    public GameObject[] estados = new GameObject[20]; // limite de estados � 20
    

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
    public void abrirMenuEstado() //Passar estado como parametro!!! e pegar outro para trasis��o
    {
        menuEstadoObj.transform.position = getPosicaoMouse();
        menuEstadoObj.SetActive(true);
    }
    public void fecharMenuEstado()
    {
        menuEstadoObj.SetActive(true);
    }
    Vector3 getPosicaoMouse()
    {
        var posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0;
        return posicaoMouse;
    }

}
