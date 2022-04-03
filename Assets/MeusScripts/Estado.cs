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

    private Vector3 _dragOffset;
    private Camera _cam;
    //colocar em private depois
    public float timeDown;
    public float timeUp;
    public float timeNow;

    //flags
    public bool draggingFlag;
    public bool pressingFlag;

    // distancia
    public Vector3 posicaoA;
    public Vector3 posicaoB;



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
        _cam = Camera.main;
    }
    void OnMouseDrag()
    {
        posicaoA = transform.position;
        posicaoB = GetPosicaoMouse() + _dragOffset;
        float distancia = Vector3.Distance(posicaoA, posicaoB);
        //Debug.Log("A distancia é:"+distancia);
        if (transform.position != (GetPosicaoMouse() + _dragOffset) && (0.09 < Vector3.Distance(posicaoA, posicaoB)))
        {

            draggingFlag = true; // Estado esta sendo arrastado
            
        }
        transform.position = GetPosicaoMouse() + _dragOffset;
    }

    Vector3 GetPosicaoMouse()
    {
        var posicaoMouse = _cam.ScreenToWorldPoint(Input.mousePosition);
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
        _dragOffset = transform.position - GetPosicaoMouse();
        timeDown = timeNow;
        pressingFlag = true;


    }
    private void OnMouseUp()
    {
        timeUp = timeNow;
        pressingFlag = false; // Estado não esta sendo pressionado 
        draggingFlag = false; // Estado não esta sendo arrastado
    }

    private void VerificarAbrirMenuEstado()
    {
        if (!draggingFlag) // Se não estiver arrastando
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

                //Cria a transição
                GameObject transObj = Instantiate(transPrefab);
            }
        }
    }
}
