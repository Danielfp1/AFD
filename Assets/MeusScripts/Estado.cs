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
    private GameObject workspaceCanvas;
    public GameObject transPrefab;

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
        return nome.text.ToString();
    }
    public void setNomeDoEstado(string nomeDoEstado)
    {
        nome.text = nomeDoEstado;
    }
    void OnMouseOver() //será que fica muito pesado?
    {
        if (workspaceCanvas.GetComponent<Workspace>().GetNovaTransFlag())
        {
            estadoAlvo = gameObject;
            workspaceCanvas.GetComponent<Workspace>().SetEstadoAlvo(estadoAlvo);
        }
        else
        estadoAtual = gameObject;
    }

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        if (!workspaceCanvas.GetComponent<Workspace>().GetNovaTransFlag())
        {
            workspaceCanvas.GetComponent<Workspace>().abrirMenuEstado(estadoAtual);
        }
        else
        {
            workspaceCanvas.GetComponent<Workspace>().SetNovaTransFlag(false);
            GameObject estado1 = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
            GameObject estado2 = workspaceCanvas.GetComponent<Workspace>().GetEstadoAlvo();
            Debug.Log(estado1.GetComponent<Estado>().getNomeDoEstado());
            Debug.Log(estado2.GetComponent<Estado>().getNomeDoEstado());
            GameObject transObj = Instantiate(transPrefab, workspaceCanvas.transform);
        }

    }
}
