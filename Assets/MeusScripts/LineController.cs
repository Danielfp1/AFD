using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    public LineRenderer lineRender;
    public Collider2D[] pontos;
    public GameObject simboloTrans;

    Vector3 coordenadas;
    float offsetX;
    float offsetY;

    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
    }

    public void FazerLinha(Collider2D[] pontos)
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
                offsetX = pontos[i].bounds.extents.x;
                offsetY = pontos[i].bounds.extents.y;

                coordenadas.x = coordenadas.x + offsetX;
                coordenadas.y = coordenadas.y + offsetY;

                lineRender.SetPosition(i, coordenadas);
            }
        }
    }
}
