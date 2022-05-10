using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Database;
using System.Linq;



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
    public GameObject[] estados = new GameObject[20]; // limite de estados � 20
    public GameObject estadoAtual;
    public GameObject estadoAlvo;

    //flags
    public bool novaTransFlag;
    public bool possuiEstadoInicial;
    public int quantosEstadosFinais;

    //transition
    public string simbolosSelecionados;
    public List<string> transistionArrows;
    public List<string> transistionStates1;
    public List<string> transistionStates2;
    public List<string> transistionSymbols;
    public List<GameObject> transitions;

    //afd
    public char[] alfabeto;
    public GameObject estadoInicial;
    public GameObject[] estadosFinais = new GameObject[20];
    public string Enuniciado;

    public TMP_Dropdown dropdownSimbolos;

    //prefabs
    public GameObject simboloToggleFrefab;
    public GameObject estadoPrefab;
    public GameObject transPrefab;

    //Banco de dados
    public GameObject firebase;
    FirebaseFirestore db;
    public List<string> estadosBd;
    public List<float> estadosPos;
    public bool readingFromDb;
    [SerializeField] Button salvar;



    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        //salvar.onClick.AddListener(OnHandleClick);
        if (StateNameController.IdProject != "")
        {
            readingFromDb = true;

            StartCoroutine(AbrirWorkspace());
            //firebase.GetComponent<FirestoreManager>().AbrirWorkspace();

            Debug.Log("Abrindo Projeto");
            FecharMenuEunciado();
            buttonEnunciado.interactable = false;
        }
    }
    public void OnHandleClick()
    {
        GetEstadosPos();
        // Struct
        FirestoreStruct firestoreStruct = new FirestoreStruct
        {
            IdUser = StateNameController.IdUser,
            Enunciado = GetEnunciado(),
            alfabeto = GetAlfabetoString(),
            quantosEstados = GetQuantosEstados(),
            estados = GetEstadosBd(),
            estadosPos = GetEstadosPos(),
            transistionStates1 = GetListaTransistionStates1(),
            transistionStates2 = GetListaTransistionStates2(),
            transistionSymbols = GetListaSymbols()
        };

        if (StateNameController.IdProject == "")
        {
            firebase.GetComponent<FirestoreManager>().GenerateId();
        }

        DocumentReference exercisesRef = db.Collection("projectsProfessores").Document(StateNameController.IdUser).Collection("projects").Document(StateNameController.IdProject);
        exercisesRef.SetAsync(firestoreStruct).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Exercício salvo");
            SSTools.ShowMessage("Exercício salvo", SSTools.Position.bottom, SSTools.Time.threeSecond);
            Debug.Log(StateNameController.IdProject);
        });

    }
    public IEnumerator AbrirWorkspace() //Abrir banco de dados
    {
        var DBTask2 = db.Collection("projectsProfessores").Document(StateNameController.IdUser).Collection("projects").Document(StateNameController.IdProject).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            FirestoreStruct firestoreStruct = task.Result.ConvertTo<FirestoreStruct>();
            SetEnunciado(firestoreStruct.Enunciado);
            enunciadoObj.GetComponent<Enunciado>().AtulizarEnunciado();
            SetAlfabeto(firestoreStruct.alfabeto);
            SetQuantosEstados(firestoreStruct.quantosEstados);
            SetEstadosBd(firestoreStruct.estados);
            SetEstadosPos(firestoreStruct.estadosPos);
            SetListaTransistionStates1(firestoreStruct.transistionStates1);
            SetListaTransistionStates2(firestoreStruct.transistionStates2);
            SetListaSymbols(firestoreStruct.transistionSymbols);
        });
        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
        InstanciarEstados();
        enunciadoObj.GetComponent<Enunciado>().AtulizarAlfabeto();
        InstanciarTransitions();
        readingFromDb = false;
    }
    public void InstanciarEstados()
    {
        int quantosEstadosTem = GetQuantosEstados();
        for (int i = 0; i < quantosEstadosTem; i++)
        {
            GameObject estadoObj = Instantiate(estadoPrefab, transform);
            estadoObj.transform.parent = quadro.transform;
            estadoObj.GetComponent<Estado>().SetNomeDoEstado(estadosBd[i]);
            GetComponent<Workspace>().AddEstado(estadoObj,false);
            //estadoObj.GetComponent<Estado>().SetEstadoPos(estadosPos[i], estadosPos[i + 1]);
        }
        enunciadoObj.GetComponent<Enunciado>().AtulizarEstados();
    }
    public void InstanciarTransitions()
    {
        int quantasTrans = transistionSymbols.Count;
        for (int i = 0; i < quantasTrans; i++)
        {
            SetSimbolosSelecionados(transistionSymbols[i]);
            List<GameObject> ListaEstados = this.estados.ToList();
            foreach ( GameObject est in ListaEstados)
            {
                if (est != null)
                {
                    if (est.GetComponent<Estado>().GetNomeDoEstado() == transistionStates1[i])
                    {
                        estadoAtual = est;
                    }
                    if (est.GetComponent<Estado>().GetNomeDoEstado() == transistionStates2[i])
                    {
                        estadoAlvo = est;
                    }
                }
            }
            GameObject transObj = Instantiate(transPrefab);
            transObj.transform.parent = quadro.transform;
        }
        estadoAtual = null;
        estadoAlvo = null;
    }

    public void AddListaTransistion(string transistionArrow)
    {
        if (transistionArrows == null)
        {
            transistionArrows = new List<string>();
        }
        transistionArrows.Add(transistionArrow);
    }
    public void AddListaTransistionStates1(string state)
    {
        if (transistionStates1 == null)
        {
            transistionStates1 = new List<string>();
        }
        transistionStates1.Add(state);
    }
    public void SetListaTransistionStates1(List<string> transistionStates)
    {
        this.transistionStates1 = transistionStates;
    }
    public List<string> GetListaTransistionStates1()
    {
        return transistionStates1;
    }
    public void AddListaTransistionStates2(string state)
    {
        if (transistionStates2 == null)
        {
            transistionStates2 = new List<string>();
        }
        transistionStates2.Add(state);
    }
    public void SetListaTransistionStates2(List<string> transistionStates)
    {
        this.transistionStates2 = transistionStates;
    }
    public List<string> GetListaTransistionStates2()
    {
        return transistionStates2;
    }
    public void AddListaSymbols(string symbol)
    {
        if (transistionSymbols == null)
        {
            transistionSymbols = new List<string>();
        }
        transistionSymbols.Add(symbol);
    }
    public void SetListaSymbols(List<string> transistionSymbols)
    {
        this.transistionSymbols = transistionSymbols;
    }
    public List<string> GetListaSymbols()
    {
        return transistionSymbols;
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
        estadosBd = new List<string>();
        transistionStates1 = new List<string>();
        transistionStates2 = new List<string>();
        transistionSymbols = new List<string>();
    }
    public void LimparCacheDb()
    {
        estadosBd = new List<string>();
        transistionStates1 = new List<string>();
        transistionStates2 = new List<string>();
        transistionSymbols = new List<string>();
    }

    public string[] GetEstadosBd()
    {
        return estadosBd.ToArray();
    }
    public void SetEstadosBd(string[] estadosBd)
    {
        foreach (string estado in estadosBd)
        {
            this.estadosBd.Add(estado);
        }
    }
    public List<float> GetEstadosPos()
    {
        List<float> estadosPos = new List<float>();

        for (int i = 0; i < quantosEstados; i++)
        {
            if (estados[i] != null)
            { 
            estadosPos.Add(this.estados[i].GetComponent<Estado>().GetPosX());
            estadosPos.Add(this.estados[i].GetComponent<Estado>().GetPosY());
            }
        }
        return estadosPos;
    }
    public void SetEstadosPos(List<float> estadosPos)
    {
        this.estadosPos = estadosPos;
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
        if (quantosEstadosFinais != 20) //Se o array n�o estiver cheio...
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

    public void SetSimbolosSelecionados(string SimbolosDoBancoDeDados)
    {
        this.simbolosSelecionados = SimbolosDoBancoDeDados;
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
        simbolosSelecionadosAux = simbolosSelecionadosAux.Replace(", .", "");
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
    public void SetAlfabeto(string[] alfabeto)
    {
        char[] alfabetoChar = new char[alfabeto.Length];
        for (int i = 0; i < alfabeto.Length; i++)
        {
            alfabetoChar[i] = alfabeto[i][0];
        }
        this.alfabeto = alfabetoChar;
    }
    public char[] GetAlfabeto()
    {
        return alfabeto;
    }
    public string[] GetAlfabetoString()
    {
        string[] alfabetoString = new string[alfabeto.Length];
        for (int i = 0; i < alfabeto.Length; i++)
        {
            alfabetoString[i] = alfabeto[i].ToString();
        }
        return alfabetoString;
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
        if (alfabeto != null) // se o alfabeto n�o estiver vazio, adicionar os simbolos no dropdown
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
        //Destacar s�mbolos
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
        if (quantosEstados != 20) //Se o array n�o estiver cheio...
        {
            for (int i = 0; i < 20; i++) //varrer
            {
                if ((estados[i] == null) && (estadoAdicionado == false)) // Se achar vaga...
                {
                    newEstados[i] = novoEstado; //Incrementa nessa vaga
                    estadoAdicionado = true; //flag
                    estadosBd.Add(novoEstado.GetComponent<Estado>().GetNomeDoEstado());
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
    public void AddEstado(GameObject novoEstado, bool abrindoDoBanco)
    {
        GameObject[] newEstados = new GameObject[20];
        bool estadoAdicionado = false;
        if (quantosEstados != 20) //Se o array n�o estiver cheio...
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
        }

    }
    public void AbrirMenuEstado(GameObject estadoAtual) //Passar estado como parametro!!! e pegar outro para trasis��o
    {
        menuEstadoObj.SetActive(true);
        if (estadoAtual.GetComponent<Estado>().GetInicial() && GetPossuiEstadoInicial()) //verifica se o estado � inicial 
        {
            menuEstadoObj.GetComponent<MenuEstado>().SetButtonEstado(true);
        }
        else if (!GetPossuiEstadoInicial()) //Verifica se j� existe um estado inicial
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
        estadosBd.Remove(estado.GetComponent<Estado>().GetNomeDoEstado());
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
