using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Workspace : MonoBehaviour
{
    //Menus
    public GameObject menuEstadoObj;
    public GameObject menuNovaTransObj;
    public GameObject menuWorkspaceObj;
    public TMP_InputField alfabetoField;
    public GameObject menuEunciadoObj;

    public int quantosEstados = 2;
    public GameObject[] estados = new GameObject[20]; // limite de estados é 20
    public GameObject estadoAtual;
    public bool novaTransFlag;
    public GameObject estadoAlvo;

    //transition
    public int simboloSelecionado;


    //afd
    private char[] alfabeto;
    public TMP_Dropdown dropdownSimbolos;


    public void SetSimboloSelecionado(int simboloSelecionado)
    {
        this.simboloSelecionado = simboloSelecionado;
    }
    public int GetSimboloSelecionado()
    {
        return simboloSelecionado;
    }

    public void SetAlfabeto(char[] alfabeto)
    {
        this.alfabeto = alfabeto;
    }
    public char[] GetAlfabeto()
    {
        return alfabeto;
    }

    public void AbrirMenuEunciado()
    {

        menuEunciadoObj.SetActive(true);
        FecharMenuWokspace();

    }
    public void FecharMenuEunciado()
    {
        menuEunciadoObj.SetActive(false);
        AbrirMenuWokspace();
    }

    public void OkMenuEnunciado()
    {
        string alfabetoJunto =  alfabetoField.text;
        alfabetoJunto = alfabetoJunto.Replace(" ","");
        char[] simbolos = new char[alfabetoJunto.Length];
        simbolos = alfabetoJunto.ToCharArray();
        SetAlfabeto(simbolos);
        FecharMenuEunciado();
    }

    public void AbrirMenuNovaTrans()
    {

        if (alfabeto != null) // se o alfabeto não estiver vazio, adicionar os simbolos no dropdown
        {
            List<string> options = new List<string>();
            for (int i = 0; i< alfabeto.Length; i++ )
            {
                options.Add(alfabeto[i].ToString());
            }
            dropdownSimbolos.ClearOptions();
            dropdownSimbolos.AddOptions(options);
        }

        menuNovaTransObj.SetActive(true);
        FecharMenuWokspace();

    }
    public void FecharMenuNovaTrans()
    {
        menuNovaTransObj.SetActive(false);
        novaTransFlag = false;
        AbrirMenuWokspace();
    }
    public void AbrirMenuWokspace()
    {

        menuWorkspaceObj.SetActive(true);

    }
    public void FecharMenuWokspace()
    {
        menuWorkspaceObj.SetActive(false);
    }
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
        menuEstadoObj.SetActive(true);
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
    private void OnMouseDown()
    {
        FecharMenuEstado();
    }
}
