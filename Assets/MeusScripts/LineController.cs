using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    public LineRenderer lineRender;
    public GameObject[] pontos = new GameObject[2];
    public GameObject workspaceCanvas;

    Vector3 coordenadas;
    float offsetX;
    float offsetY;

    private void Awake()
    {
        workspaceCanvas = GameObject.FindGameObjectWithTag("WorkspaceCanvas");
        lineRender = GetComponent<LineRenderer>();
        pontos[0] = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        pontos[1] = workspaceCanvas.GetComponent<Workspace>().GetEstadoAlvo();
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
                //offsetX = pontos[i].bounds.extents.x;
                //offsetY = pontos[i].bounds.extents.y;

                //coordenadas.x = coordenadas.x + offsetX;
                //coordenadas.y = coordenadas.y + offsetY;

                lineRender.SetPosition(i, coordenadas);
            }
        }
    }
}
