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

    void Awake()
    {
        workspace = GameObject.FindGameObjectWithTag("WorkspaceCanvas");
    }
    void OnMouseDrag()
    {
        transform.position = GetPosicaoMouse();
    }

    Vector3 GetPosicaoMouse()
    {
        var posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        if (!workspace.GetComponent<Workspace>().GetNovaTransFlag())
        {
            //Estado Atual
            workspace.GetComponent<Workspace>().AbrirMenuEstado(gameObject);
        }
        else
        {
            //Estado Alvo
            workspace.GetComponent<Workspace>().SetNovaTransFlag(false);
            workspace.GetComponent<Workspace>().SetEstadoAlvo(gameObject);

            //Cria a transição
            GameObject transObj = Instantiate(transPrefab, workspace.transform);
        }

    }
}
