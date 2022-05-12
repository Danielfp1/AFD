﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Estado : MonoBehaviour
{
    public TMP_Text nome;

    public float posX;
    public float posY;

    public bool inicial;
    public bool final;


    //GameObjets
    public GameObject estadoAtual;
    public GameObject estadoAlvo;
    private GameObject workspace;
    public GameObject transPrefab;
    public GameObject quadro;

    private Vector3 dragOffset;
    private Camera cam;

    //Variáveis de Tempo
    private float timeDown;
    private float timeUp;
    private float timeNow;

    //flags
    public bool movingFlag;
    public bool pressingFlag;
    public bool draggingFlag;

    // distancia
    private Vector2 posicaoA;
    private Vector2 posicaoB;

    //Simbolos restantes
    public List<string> simbolosRestantes;

    void Awake()
    {
        workspace = GameObject.FindGameObjectWithTag("WorkspaceCanvas");
        quadro = GameObject.FindGameObjectWithTag("Quadro");
        cam = Camera.main;
        this.simbolosRestantes = workspace.GetComponent<Workspace>().GetAlfabetoString().ToList<string>();
    }
    public List<string> GetSimbolosRestantes()
    {
        return this.simbolosRestantes;
    }
    public void RemoverSimbolosRestantes(string simbolos)
    {
        simbolos=simbolos.Replace(" ", "");
        List<string> remover = new List<string>();
        remover = simbolos.Split(',').ToList<string>();
        foreach (string simboloRemover in remover)
        {
            this.simbolosRestantes.Remove(simboloRemover);
        }
    }
    public void AdicionarSimbolosRestantes(string simbolos)
    {
        simbolos = simbolos.Replace(" ", "");
        List<string> adicionar = new List<string>();
        adicionar = simbolos.Split(',').ToList<string>();
        foreach (string simboloAdicionar in adicionar)
        {
            this.simbolosRestantes.Add(simboloAdicionar);
        }
    }
    public void SetFinal(bool flag)
    {
        this.final = flag;
    }
    public bool GetFinal()
    {
        return this.final;
    }
    public void SetInicial(bool flag)
    {
        this.inicial = flag;
    }
    public bool GetInicial()
    {
        return this.inicial;
    }

    private void Update()
    {
        timeNow = Time.fixedTime;
        if (pressingFlag)
        {
            VerificarAbrirMenuEstado();
        }
        if (inicial)
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(3).gameObject.SetActive(false);
        }
        if (final)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }
    void OnMouseDrag()
    {
        if (draggingFlag)
        {
            posicaoA = transform.position;
            posicaoB = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector2)dragOffset;
            float distancia = Vector2.Distance(posicaoA, posicaoB);
            //Debug.Log("A distancia �:" + distancia);
            if ((Vector2)transform.position != ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)dragOffset) && (0.8 < distancia))
            {

                movingFlag = true; // Estado esta sendo movido

            }
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)dragOffset;
            posX = transform.localPosition.x;
            posY = transform.localPosition.y;
        }
    }
    Vector3 GetPosicaoMouse()
    {
        var posicaoMouse = (Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = -9720;
        posX = posicaoMouse.x;
        posY = posicaoMouse.y;

        return posicaoMouse;
    }

    public string GetNomeDoEstado()
    {
        return nome.text.ToString();
    }
    public void SetNomeDoEstado(string nomeDoEstado)
    {
        nome.text = nomeDoEstado;
    }
    private void OnMouseDown()
    {
        dragOffset = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        timeDown = timeNow;
        pressingFlag = true;
        draggingFlag = true;
        

    }
    private void OnMouseUp()
    {
        timeUp = timeNow;
        pressingFlag = false; // Estado não esta sendo pressionado 
        movingFlag = false; // Estado não esta sendo movido
        draggingFlag = false; // Estado não esta sendo arrastado 
    }
    private void VerificarAbrirMenuEstado()
    {
        if (!movingFlag) // Se não estiver movendo
        {
            if (!workspace.GetComponent<Workspace>().GetNovaTransFlag()) //Se não estiver fazendo transição
            {
                if (timeNow > timeDown + 0.7) // Se for um click longo
                {
                    //Abre o menu e manda o Estado Atual
                    workspace.GetComponent<Workspace>().AbrirMenuEstado(gameObject);
                }
            }
            else
            {
                //Estado Alvo
                workspace.GetComponent<Workspace>().FecharMenuNovaTrans2();
                workspace.GetComponent<Workspace>().SetEstadoAlvo(gameObject);
                //Simbolos Selecionados
                //workspace.GetComponent<Workspace>().SetSimbolosSelecionados();

                //Cria a transição
                GameObject transObj = Instantiate(transPrefab);
                transObj.transform.parent = quadro.transform;
            }
        }
    }

   public float GetPosX()
    {
        return posX;
    }
    public float GetPosY()
    {
        return posY;
    }
    public void SetEstadoPos(float x, float y)
    {
        Vector3 vetorAux;
        vetorAux.x = x;
        this.posX = x;
        vetorAux.y = y;
        this.posY = y;
        vetorAux.z = -9720;
        transform.localPosition = vetorAux;
    }
}
