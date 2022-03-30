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
    private GameObject workspaceCanvas;

    void Awake()
    {
        workspaceCanvas = GameObject.FindGameObjectWithTag("WorkspaceCanvas");
    }
    void OnMouseDrag()
    {
        transform.position = getPosicaoMouse();
    }

    Vector3 getPosicaoMouse()
    {
        var posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0;
        posX = posicaoMouse.x;
        posY = posicaoMouse.y;

        return posicaoMouse;

    }

    public string getNomeDoEstado()
    {
        return nome.ToString();
    }
    public void setNomeDoEstado(string nomeDoEstado)
    {
        nome.text = nomeDoEstado;
    }
    void OnMouseOver() //será que fica muito pesado
    {
        estadoAtual = gameObject;
    }

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        workspaceCanvas.GetComponent<Workspace>().abrirMenuEstado(estadoAtual);
    }
}
