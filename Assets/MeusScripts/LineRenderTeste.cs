using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderTeste : MonoBehaviour
{
    [SerializeField] public GameObject[] pontos;
    [SerializeField] public LineController linha;

    private void Start()
    {
        linha.FazerLinha(pontos);
    }
}