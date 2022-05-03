using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Database;



public class Workspace : MonoBehaviour
{
    //Menus
    public GameObject menuEstadoObj;
    public GameObject menuNovaTransObj1;
    public GameObject menuNovaTransObj2;
    public GameObject menuWorkspaceObj;
    public GameObject menuEunciadoObj;
    public GameObject enunciadoObj;
    public GameObject quadro;
    public TMP_InputField alfabetoField;
    public TMP_InputField linguagemField;
    public Button buttonEnunciado;
    public GameObject highlight;
    public GameObject simbolsSelection;

    public int quantosEstados = 0;
    public GameObject[] estados = new GameObject[20]; // limite de estados é 20
    public GameObject estadoAtual;
    public GameObject estadoAlvo;

    //flags
    public bool novaTransFlag;
    public bool possuiEstadoInicial;
    public int quantosEstadosFinais;

    //transition
    public string simbolosSelecionados;
    public List<string> transistionArrows;
    public List<string> transistionStates;
    public List<string> transistionSimbols;
    public List<GameObject> transitions;

    //afd
    private char[] alfabeto;
    public GameObject estadoInicial;
    public GameObject[] estadosFinais = new GameObject[20];
    public string Enuniciado;

    public TMP_Dropdown dropdownSimbolos;

    //prefabs
    public GameObject simboloToggleFrefab;

    //Banco de dados
    public GameObject firebase;
    FirebaseFirestore db;

    private void Start()
    {
       if (StateNameController.IdProject != "")
        {

            db = FirebaseFirestore.DefaultInstance;
            StartCoroutine(AbrirWorkspace());

            Debug.Log("Abrindo Projeto");
            FecharMenuEunciado();
            buttonEnunciado.interactable = false;

        }
    }
    public IEnumerator AbrirWorkspace()
    {
        var DBTask2 = db.Collection("exercises").Document(StateNameController.IdProject).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            FirestoreStruct firestoreStruct = task.Result.ConvertTo<FirestoreStruct>();
            SetEnunciado(firestoreStruct.Enunciado);
            enunciadoObj.GetComponent<Enunciado>().AtulizarEnunciado();
        });
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
    }
    public void AddListaTransistion(string transistionArrow)
    {
        transistionArrows.Add(transistionArrow);
    }
    public void AddListaStates(string state)
    {
        transistionStates.Add(state);
    }
    public void AddListaSimbols(string simbol)
    {
        transistionSimbols.Add(simbol);
    }

    public void ApagarWorkspace()
    {
        int childs = quadro.transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            GameObject.Destroy(quadro.transform.GetChild(i).gameObject);
        }

        alfabetoField.text = "";
        quantosEstados = 0;
        estados = new GameObject[20];
        estadoAtual = null;
        estadoAlvo = null;
        quantosEstadosFinais = 0;
        possuiEstadoInicial = false;
        alfabeto = null;
        estadoInicial = null;
        estadosFinais = new GameObject[20];
        transistionArrows = new List<string>();
        Enuniciado = "<<Clique Aqui>>";
        buttonEnunciado.interactable = true;
        enunciadoObj.GetComponent<Enunciado>().ZerarEnunciado();
    }

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

    public void SetSimbolosSelecionados()
    {
        int count = 0;
        string simbolosSelecionadosAux = "";
        Component[] togglesTicks;
        togglesTicks = simbolsSelection.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in togglesTicks)
        {
            if (toggle.isOn == true)
            {
                simbolosSelecionadosAux += alfabeto[count].ToString();
                simbolosSelecionadosAux += ", ";
            }
            count++;
        }
        simbolosSelecionadosAux += ".";
        simbolosSelecionadosAux=simbolosSelecionadosAux.Replace(", .","");
        this.simbolosSelecionados = simbolosSelecionadosAux;
        int childs = simbolsSelection.transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            GameObject.Destroy(simbolsSelection.transform.GetChild(i).gameObject);
        }
    }
    public string GetSimbolosSelecionados()
    {
        return simbolosSelecionados;
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
        EsconderQuadro();
        FecharMenuWokspace();

    }
    public void FecharMenuEunciado()
    {
        menuEunciadoObj.SetActive(false);
        MostrarQuadro();
        AbrirMenuWokspace();
    }

    public void OkMenuEnunciado()
    {
        if (alfabetoField.text != "")
        {
            string alfabetoJunto = alfabetoField.text;
            alfabetoJunto = alfabetoJunto.Replace(" ", "");
            alfabetoJunto = alfabetoJunto.Replace(",", "");
            char[] simbolos = new char[alfabetoJunto.Length];
            simbolos = alfabetoJunto.ToCharArray();
            SetAlfabeto(simbolos);
            SetEnunciado(linguagemField.text);
            enunciadoObj.GetComponent<Enunciado>().AtulizarEnunciado();
            enunciadoObj.GetComponent<Enunciado>().AtulizarAlfabeto();
            buttonEnunciado.interactable = false;
            FecharMenuEunciado();
        }
        else
        {
            SSTools.ShowMessage("Alfabeto em branco", SSTools.Position.bottom, SSTools.Time.threeSecond);
            //SSTools.ShowMessage("Linguagem em branco", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }
    public void SetEnunciado(string linguagem)
    {
        this.Enuniciado = linguagem;
    }
    public string GetEnunciado()
    {
        return this.Enuniciado;
    }

    public void AbrirMenuNovaTrans()
    {
        EsconderQuadro();
        if (alfabeto != null) // se o alfabeto não estiver vazio, adicionar os simbolos no dropdown
        {
            //List<string> options = new List<string>();
            for (int i = 0; i < alfabeto.Length; i++)
            {
                //options.Add(alfabeto[i].ToString());
                GameObject simboloToggle = Instantiate(simboloToggleFrefab, simbolsSelection.transform);
                simboloToggle.transform.GetChild(1).GetComponent<Text>().text = alfabeto[i].ToString();
            }
            //dropdownSimbolos.ClearOptions();
            //dropdownSimbolos.AddOptions(options);
        }
        menuNovaTransObj1.SetActive(true);
        FecharMenuWokspace();
    }

    public void MenuNovaTransProx()
    {
        MostrarQuadro();
        novaTransFlag = true;
        FecharMenuNovaTrans1();
        //Destacar símbolos
        highlight.SetActive(true);
        SetSimbolosSelecionados();
        menuNovaTransObj2.SetActive(true);
    }

    public void FecharMenuNovaTrans1()
    {
        menuNovaTransObj1.SetActive(false);
        MostrarQuadro();
        AbrirMenuWokspace();
        int childs = simbolsSelection.transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            GameObject.Destroy(simbolsSelection.transform.GetChild(i).gameObject);
        }
    }
    public void FecharMenuNovaTrans2()
    {
        highlight.SetActive(false);
        menuNovaTransObj2.SetActive(false);
        novaTransFlag = false;
        AbrirMenuWokspace();
        MostrarQuadro();
        menuNovaTransObj2.SetActive(false);
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
        GameObject[] newEstados = new GameObject[20];
        for (int i = 0; i < 20; i++)
        {
            if (estado == estados[i])
            {
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
            enunciadoObj.GetComponent<Enunciado>().AtulizarEstadoInicial();
        }
        if (estado.GetComponent<Estado>().GetFinal()) //Se for estado final
        {
            RemoverEstadoFinal(estado);
            enunciadoObj.GetComponent<Enunciado>().AtulizarEstadosFinais();
        }
        Destroy(estado);
        SetQuantosEstados(GetQuantosEstados() - 1);
        enunciadoObj.GetComponent<Enunciado>().AtulizarEstados();
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
                if (nomes[i] == nome)
                {
                    aux = true;
                    flagMesmoNome = true;
                }
            }
        }
        return aux;
    }

    public void MostrarQuadro()
    {
        quadro.SetActive(true);
    }

    public void EsconderQuadro()
    {
        quadro.SetActive(false);
    }

}
