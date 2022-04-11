using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Estado : MonoBehaviour
{
    public TMP_Text nome;

    public float posX;
    public float posY;
    public GameObject estadoAtual;
    public GameObject estadoAlvo;
    private GameObject workspace;
    public GameObject transPrefab;

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
    private Vector3 posicaoA;
    private Vector3 posicaoB;



    private void Update()
    {
        timeNow = Time.fixedTime;
        if (pressingFlag)
        {
            VerificarAbrirMenuEstado();
        }
    }
    void Awake()
    {
        workspace = GameObject.FindGameObjectWithTag("WorkspaceCanvas");
        cam = Camera.main;
    }
    void OnMouseDrag()
    {
        if (draggingFlag)
        {
            posicaoA = transform.position;
            posicaoB = GetPosicaoMouse() + dragOffset;
            float distancia = Vector3.Distance(posicaoA, posicaoB);
            //Debug.Log("A distancia é:"+distancia);
            if (transform.position != (GetPosicaoMouse() + dragOffset) && (0.09 < Vector3.Distance(posicaoA, posicaoB)))
            {

                movingFlag = true; // Estado esta sendo movido

            }
            transform.position = GetPosicaoMouse() + dragOffset;
        }
    }
    Vector3 GetPosicaoMouse()
    {
        var posicaoMouse = cam.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0;
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
        dragOffset = transform.position - GetPosicaoMouse();
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
        if (!movingFlag) // Se não estiver arrastando
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
                workspace.GetComponent<Workspace>().FecharMenuNovaTrans();
                workspace.GetComponent<Workspace>().SetEstadoAlvo(gameObject);
                //Simbolo Selecionado
                workspace.GetComponent<Workspace>().SetSimboloSelecionado(workspace.GetComponent<Workspace>().dropdownSimbolos.value);
                //Cria a transição
                GameObject transObj = Instantiate(transPrefab);
            }
        }
    }
}
