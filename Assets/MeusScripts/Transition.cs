using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Transition : MonoBehaviour
{
    public GameObject transicao;

    public LineRenderer lineRender;
    public GameObject pontoA;
    public GameObject pontoB;
    public GameObject pontoAB;
    public string transistionArrow;
    public string transistionState;
    public string transistionReverseArrow;

    public GameObject workspaceCanvas;

    public TMP_Text simbolosText;
    //public GameObject simbolosObj;

    //flags
    bool segundaCurva = false;




    private void Awake()
    {
        workspaceCanvas = GameObject.FindGameObjectWithTag("WorkspaceCanvas");
        lineRender = GetComponent<LineRenderer>();
        transistionArrow = "";
        transistionState = "";

        //linkar os estados

        pontoA = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        pontoB = workspaceCanvas.GetComponent<Workspace>().GetEstadoAlvo();
        pontoAB = workspaceCanvas.GetComponent<Workspace>().GetEstadoAlvo();

        //char[] alfabeto = workspaceCanvas.GetComponent<Workspace>().GetAlfabeto();
        simbolosText.text = workspaceCanvas.GetComponent<Workspace>().GetSimbolosSelecionados();

        //Adiciona na Lista de símbolos
        if (!workspaceCanvas.GetComponent<Workspace>().readingFromDb)
        {
            workspaceCanvas.GetComponent<Workspace>().AddListaSymbols(simbolosText.text);
        }
        //Transition Arrow
        transistionArrow += pontoA.GetComponent<Estado>().GetNomeDoEstado();
        transistionState = pontoA.GetComponent<Estado>().GetNomeDoEstado();
        if (!workspaceCanvas.GetComponent<Workspace>().readingFromDb)
        {
            workspaceCanvas.GetComponent<Workspace>().AddListaTransistionStates1(transistionState);
        }
        transistionArrow += " -> ";
        transistionArrow += pontoB.GetComponent<Estado>().GetNomeDoEstado();
        transistionState = pontoB.GetComponent<Estado>().GetNomeDoEstado();
        if (!workspaceCanvas.GetComponent<Workspace>().readingFromDb)
        {
            workspaceCanvas.GetComponent<Workspace>().AddListaTransistionStates2(transistionState);
        }


        //Adiciona na Lista de setas
        workspaceCanvas.GetComponent<Workspace>().AddListaTransistion(transistionArrow);

        transistionReverseArrow += pontoB.GetComponent<Estado>().GetNomeDoEstado();
        transistionReverseArrow += " -> ";
        transistionReverseArrow += pontoA.GetComponent<Estado>().GetNomeDoEstado();


        if (workspaceCanvas.GetComponent<Workspace>().transistionArrows.Contains(transistionReverseArrow))
        {
            segundaCurva = true;
        }
        //Mandar o PAI!!!!!!!!!!!!!!
        //workspaceCanvas.GetComponent<Workspace>().transitions.Add(gameObject);
    }

    public void FazerLinhaReta(GameObject pontoA, GameObject pontoB)
    {
        lineRender.positionCount = 2;

        Vector2 pontoAPos = pontoA.GetComponent<Collider2D>().ClosestPoint(pontoB.transform.position);
        Vector2 pontoBPos = pontoB.GetComponent<Collider2D>().ClosestPoint(pontoA.transform.position);

        lineRender.SetPosition(0, pontoAPos);
        lineRender.SetPosition(1, pontoBPos);

        //Posição dos simbolos
        Vector3 simbolosTextPos = lineRender.bounds.center;
        Vector2 direction = pontoBPos - pontoAPos;
        float angulo = CalcularAngulo(Vector2.right, direction);
        if ((angulo > -160 && angulo < -60) ^ (angulo > 60 && angulo < 120))
        {
            //lado
            simbolosTextPos.x = simbolosTextPos.x - 0.5f;
            simbolosTextPos.z = 91;
        }
        else
        {
            //cima
            simbolosTextPos.y = simbolosTextPos.y + 0.5f;
            simbolosTextPos.z = 91;

        }
        simbolosText.transform.position = simbolosTextPos;
    }

    private void Update()
    {
        //Se os estados não existirem mais se auto destruir
        if ((pontoA == null ^ pontoB == null) ^ ((pontoA == null) && (pontoB == null)))
        {
            Destroy(transicao);
            workspaceCanvas.GetComponent<Workspace>().transistionArrows.Remove(transistionArrow);
            //Mandar o PAI!!!!!!!!!!!!!!
            //workspaceCanvas.GetComponent<Workspace>().transitions.Remove(gameObject);
        }
        else
        {
            if (pontoA == pontoB)
            {
                FazerLinhaLoop(pontoA);
            }
            else if (workspaceCanvas.GetComponent<Workspace>().transistionArrows.Contains(transistionReverseArrow))
            {
                FazerLinhaCurva(pontoA, pontoB);
            }
            else
            {
                FazerLinhaReta(pontoA, pontoB);
            }
        }
    }
    float CalcularAngulo(Vector2 vectorA, Vector2 vectorB)
    {
        var vectorANor = vectorA.normalized;
        var vectorBNor = vectorB.normalized;
        var x = vectorANor.x * vectorBNor.x + vectorANor.y * vectorBNor.y;
        var y = vectorANor.y * vectorBNor.x - vectorANor.x * vectorBNor.y;
        return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }
    void FazerLinhaCurva(GameObject pontoA, GameObject pontoB)
    {
        lineRender.positionCount = 50;

        Vector2 pontoAPos = pontoA.GetComponent<Collider2D>().ClosestPoint(pontoB.transform.position);
        Vector2 pontoBPos = pontoB.GetComponent<Collider2D>().ClosestPoint(pontoA.transform.position);

        lineRender.SetPosition(0, pontoAPos);
        lineRender.SetPosition(1, pontoBPos);

        //Posição dos simbolos
        Vector3 simbolosTextPos = lineRender.bounds.center;
        Vector2 direction = pontoBPos - pontoAPos;
        float angulo = CalcularAngulo(Vector2.right, direction);
        if ((angulo > -160 && angulo < -60) ^ (angulo > 60 && angulo < 120))
        {
            if (segundaCurva)
            {
                //lado reverso
                simbolosTextPos.x = simbolosTextPos.x + 0.8f;
                simbolosTextPos.z = 91;
            }
            else
            {
                //lado
                simbolosTextPos.x = simbolosTextPos.x - 0.8f;
                simbolosTextPos.z = 91;
            }
        }
        else
        {
            if (segundaCurva)
            {
                //cima reverso
                simbolosTextPos.y = simbolosTextPos.y - 0.8f;
                simbolosTextPos.z = 91;
            }
            else
            {
                //cima
                simbolosTextPos.y = simbolosTextPos.y + 0.8f;
                simbolosTextPos.z = 91;
            }
        }
        simbolosText.transform.position = simbolosTextPos;
        /////


        float t = 0;
        Vector2 aux = new Vector2(0, 0);
        Vector2 pontoABPos = new Vector2(simbolosText.transform.position.x, simbolosText.transform.position.y);
        for (int i = 0; i < lineRender.positionCount; i++)
        {
            aux = (1 - t) * (1 - t) * pontoAPos + 2 * (1 - t) * t * pontoABPos + t * t * pontoBPos;
            lineRender.SetPosition(i, aux);
            t += (1 / (float)lineRender.positionCount);
        }
    }
    void FazerLinhaLoop(GameObject pontoA)
    {
        lineRender.positionCount = 15;

        Vector2 pontoAPos;
        pontoAPos.x = pontoA.transform.position.x - pontoA.GetComponent<Collider2D>().bounds.extents.x;
        pontoAPos.y = pontoA.transform.position.y + pontoA.GetComponent<Collider2D>().bounds.extents.y;
        pontoAPos = pontoA.GetComponent<Collider2D>().ClosestPoint(pontoAPos);
        Vector2 pontoAPos3;
        pontoAPos3.x = pontoA.transform.position.x + pontoA.GetComponent<Collider2D>().bounds.extents.x;
        pontoAPos3.y = pontoA.transform.position.y + pontoA.GetComponent<Collider2D>().bounds.extents.y;
        pontoAPos3 = pontoA.GetComponent<Collider2D>().ClosestPoint(pontoAPos3);
        Vector2 pontoAPos2;
        pontoAPos2.x = pontoA.transform.position.x;
        pontoAPos2.y = pontoA.transform.position.y + pontoA.GetComponent<Collider2D>().bounds.extents.y + 1.3f;

        lineRender.SetPosition(0, pontoAPos);
        lineRender.SetPosition(1, pontoAPos2);
        lineRender.SetPosition(2, pontoAPos3);

        float t = 0;
        Vector2 aux = new Vector2(0, 0);
        for (int i = 0; i < lineRender.positionCount; i++)
        {
            aux = (1 - t) * (1 - t) * pontoAPos + 2 * (1 - t) * t * pontoAPos2 + t * t * pontoAPos3;
            lineRender.SetPosition(i, aux);
            t += (1 / (float)lineRender.positionCount);
        }

        //Posição dos simbolos
        Vector3 simbolosTextPos = lineRender.bounds.center;
        simbolosTextPos.y = simbolosTextPos.y + 0.8f;
        simbolosText.transform.position = simbolosTextPos;
    }
}
