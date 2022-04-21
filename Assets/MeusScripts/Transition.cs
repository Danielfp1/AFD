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

    public GameObject workspaceCanvas;

    public TMP_Text simbolosText;
    //public GameObject simbolosObj;


    private void Awake()
    {
        workspaceCanvas = GameObject.FindGameObjectWithTag("WorkspaceCanvas");
        lineRender = GetComponent<LineRenderer>();

        
        //linkar os estados

        pontoA = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        pontoB = workspaceCanvas.GetComponent<Workspace>().GetEstadoAlvo();
        pontoAB = workspaceCanvas.GetComponent<Workspace>().GetEstadoAlvo();


        char[] alfabeto = workspaceCanvas.GetComponent<Workspace>().GetAlfabeto();
        simbolosText.text = alfabeto[workspaceCanvas.GetComponent<Workspace>().GetSimboloSelecionado()].ToString();

        //FazerLinhaReta(pontoA, pontoB);
        //FazerLinhaCurva(pontoA, pontoAB, pontoB);
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
        if (pontoA == null ^ pontoB == null) //Se os estados não existirem mais se auto destruir
        {
            Destroy(transicao);
        }
        else
        {
            FazerLinhaReta(pontoA, pontoB);
            //FazerLinhaCurva(pontoA, pontoAB, pontoB);

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
    void FazerLinhaCurva(GameObject pontoA, GameObject pontoAB, GameObject pontoB)
    {
        lineRender.positionCount = 50;

        Vector2 pontoAPos = pontoA.GetComponent<Collider2D>().ClosestPoint(pontoB.transform.position);
        Vector2 pontoBPos = pontoB.GetComponent<Collider2D>().ClosestPoint(pontoA.transform.position);

        lineRender.SetPosition(0, pontoAPos);
        lineRender.SetPosition(1, pontoBPos);

        float t = 0;
        Vector2 aux = new Vector2(0, 0);
        Vector2 PontoABPos = new Vector2(pontoAB.transform.position.x, pontoAB.transform.position.y);
        for (int i = 0; i < lineRender.positionCount; i++)
        {
            aux = (1 - t) * (1 - t) * pontoAPos + 2 * (1 - t) * t * PontoABPos + t * t * pontoBPos;
            lineRender.SetPosition(i, aux);
            t += (1 / (float)lineRender.positionCount);
        }

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
}
