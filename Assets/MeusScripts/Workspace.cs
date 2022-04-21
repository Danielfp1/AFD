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
    public GameObject enunciado;

    public int quantosEstados = 0;
    public GameObject[] estados = new GameObject[20]; // limite de estados é 20
    public GameObject estadoAtual;
    public GameObject estadoAlvo;
    
    //flags
    public bool novaTransFlag;
    public int quantosEstadosFinais;
    public bool possuiEstadoInicial;

    //transition
    public int simboloSelecionado;


    //afd
    private char[] alfabeto;
    public GameObject estadoInicial;
    public GameObject[] estadosFinais = new GameObject[20];
    public TMP_Dropdown dropdownSimbolos;

    public void SetEstadoInicial(GameObject estadoInicial)
    {
        this.estadoInicial = estadoInicial;
    }
    public GameObject GetEstadoInicial()
    {
        return estadoInicial;
    }
    public void RemoverEstadoInicial()
    {
        estadoInicial = null;
    }
    public void RemoverEstadoFinal(GameObject estado)
    {
       GameObject[] newEstadosFinais = estadosFinais;
       for (int i = 0; i < newEstadosFinais.Length; i++)
        {
            if (estado == estadosFinais[i])
            {
                newEstadosFinais[i] = null;
            }
            else
            {
                newEstadosFinais[i] = estadosFinais[i];
            }
        }
       this.estadosFinais = newEstadosFinais;
       SetQuantosEstadosFinais(GetQuantosEstadosFinais() - 1);
    }
    public void AddEstadoFinal(GameObject novoEstado)
    {
        GameObject[] newEstadosFinais = new GameObject[20];
        if (GetQuantosEstadosFinais() == 0)
        {
            estadosFinais[0] = novoEstado;
        }
        bool estadoAdicionado = false;
        if (quantosEstadosFinais != 20) //Se o array não estiver cheio...
        {
            for (int i = 0; i < 20; i++) //varrer
            {
                if ((estadosFinais[i] == null) && (estadoAdicionado == false)) // Se achar vaga...
                {
                    estadoAdicionado = true; //flag
                    newEstadosFinais[i] = novoEstado; //Incrementa nessa vaga
                }
                else if (i < 20)
                {
                    newEstadosFinais[i] = estadosFinais[i];
                }
            }
            this.estadosFinais = newEstadosFinais;
        }
    }
    public GameObject[] GetEstadosFinais()
    {
        return estadosFinais;
    }

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
        alfabetoJunto = alfabetoJunto.Replace(",","");
        char[] simbolos = new char[alfabetoJunto.Length];
        simbolos = alfabetoJunto.ToCharArray();
        SetAlfabeto(simbolos);
        enunciado.GetComponent<Enunciado>().AtulizarAlfabeto();
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
        GameObject[] newEstados = new GameObject[20];
        bool estadoAdicionado = false;
        if (quantosEstados != 20) //Se o array não estiver cheio...
        {
            for (int i = 0; i < 20; i++) //varrer
            {
                if ((estados[i] == null) && (estadoAdicionado == false)) // Se achar vaga...
                {
                    newEstados[i] = novoEstado; //Incrementa nessa vaga
                    estadoAdicionado = true; //flag
                }
                else if (i < 20)
                {
                    newEstados[i] = estados[i];
                }
            }
            estados = newEstados;
            SetQuantosEstados(quantosEstados + 1);
        }
        
    }
    public void AbrirMenuEstado(GameObject estadoAtual) //Passar estado como parametro!!! e pegar outro para trasisção
    {
        menuEstadoObj.SetActive(true);
        if (estadoAtual.GetComponent<Estado>().GetInicial() && GetPossuiEstadoInicial()) //verifica se o estado é inicial 
        {
            menuEstadoObj.GetComponent<MenuEstado>().SetButtonEstado(true);
        }
        else if (!GetPossuiEstadoInicial()) //Verifica se já existe um estado inicial
        {
            menuEstadoObj.GetComponent<MenuEstado>().SetButtonEstado(true);
        }
        else
        {
            menuEstadoObj.GetComponent<MenuEstado>().SetButtonEstado(false);
        }
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
    public void SetQuantosEstadosFinais(int quantidade)
    {
        this.quantosEstadosFinais = quantidade;
    }
    public int GetQuantosEstadosFinais()
    {
        return this.quantosEstadosFinais;
    }
    public void SetPossuiEstadoInicial(bool flag)
    {
        this.possuiEstadoInicial = flag;
    }
    public bool GetPossuiEstadoInicial()
    {
        return this.possuiEstadoInicial;
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

    public void RemoverEstado(GameObject estado)
    {
        GameObject[] newEstados= new GameObject[20];
        for(int i=0; i < 20; i++)
        {
            if (estado == estados[i])
            {
                Debug.Log("Acheii!");
                newEstados[i] = null;
            }
            else
            {
                newEstados[i] = estados[i];
            }
        }
        estados = newEstados;
        if (estado.GetComponent<Estado>().GetInicial()) //Se for estado inicial
        {
            SetPossuiEstadoInicial(false);
            SetEstadoInicial(null);
            enunciado.GetComponent<Enunciado>().AtulizarEstadoInicial();
        }
        if (estado.GetComponent<Estado>().GetFinal()) //Se for estado final
        {
            RemoverEstadoFinal(estado);
            enunciado.GetComponent<Enunciado>().AtulizarEstadosFinais();
        }
        Destroy(estado);
        SetQuantosEstados(GetQuantosEstados() - 1);
        enunciado.GetComponent<Enunciado>().AtulizarEstados();
    }
    public string[] GetNomeDosEstados()
    {
        string[] nomes = new string[20];
        for (int i = 0; i < 20; i++)
        {
            if (estados[i] != null)
            {
                nomes[i] = estados[i].GetComponent<Estado>().GetNomeDoEstado();
            }
        }
        return nomes;
    }

    public string[] GetNomeDosEstadosFinais()
    {
        string[] nomes = new string[20];
        for (int i = 0; i < 20; i++)
        {
            if (estadosFinais[i] != null)
            {
                nomes[i] = estadosFinais[i].GetComponent<Estado>().GetNomeDoEstado();
            }
        }
        return nomes;
    }
    public bool TemONome(string nome)
    {
        string[] nomes = GetNomeDosEstados();
        bool aux = false;
        bool flagMesmoNome = false;
        for (int i = 0; i < estados.Length; i++)
        {
            if ((estados[i] != null) && !flagMesmoNome)
            {
                if(nomes[i] == nome)
                {
                    aux = true;
                    flagMesmoNome = true;
                }
            }
        }
        return aux;
    }
}
