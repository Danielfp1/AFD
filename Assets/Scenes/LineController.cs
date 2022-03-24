using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public LineRenderer lineRender;
    public Collider2D[] pontos;

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
        for(int i=0; i < pontos.Length; i++)
        {
            lineRender.SetPosition(i, pontos[i].transform.position);
        }
    }
}
