using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Transition : MonoBehaviour
{

    public LineRenderer lineRender;
    public GameObject[] pontos = new GameObject[2];
    public GameObject workspaceCanvas;

    Vector3 coordenadas;

    public TMP_Text simbolo;

    float offsetX;
    float offsetY;

    private void Awake()
    {
        workspaceCanvas = GameObject.FindGameObjectWithTag("WorkspaceCanvas");
        lineRender = GetComponent<LineRenderer>();

        //Se os estados não existirem mais se auto destruir

        //linkar os estados

        pontos[0] = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        pontos[1] = workspaceCanvas.GetComponent<Workspace>().GetEstadoAlvo();

        char[] alfabeto = workspaceCanvas.GetComponent<Workspace>().GetAlfabeto();
        simbolo.text = alfabeto[workspaceCanvas.GetComponent<Workspace>().GetSimboloSelecionado()].ToString();

        FazerLinha(pontos);
    }

    public void FazerLinha(GameObject[] pontos)
    {
        lineRender.positionCount = pontos.Length;
        this.pontos = pontos;
    }

    private void Update()
    {
        if (pontos != null)
        {

            for (int i = 0; i < pontos.Length; i++)
            {
                coordenadas = pontos[i].transform.position; // salva o valores da coordenada

                //Calculo do Offset Com o 2dColider!!!
                offsetX = pontos[i].GetComponent<Collider2D>().bounds.extents.x;
                offsetY = pontos[i].GetComponent<Collider2D>().bounds.extents.y;

                //coordenadas.x = coordenadas.x + offsetX;
                coordenadas.y = coordenadas.y + offsetY;
                lineRender.SetPosition(i, coordenadas);
            }
            coordenadas = lineRender.bounds.center;
            //coordenadas.y = lineRender.bounds.extents.y;
            //coordenadas.x = lineRender.bounds.extents.x;
            simbolo.transform.position = coordenadas;
        }
    }
}
