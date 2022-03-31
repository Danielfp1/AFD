using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEstado_Script : MonoBehaviour
{
    public GameObject workspaceCanvas;
    public GameObject menuEstadoObj;

    // Update is called once per frame


    public void OnMouseOver()
    {
        //Debug.Log("O mouse esta em cima do menu");
    }

    public void OnMouseExit()
    {
        menuEstadoObj.SetActive(false);
    }


    //Botões
    public void DeletarEstado()
    {
        GameObject estado = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        Destroy(estado);
        Debug.Log("Morre!");
        menuEstadoObj.SetActive(false);
    }

    public void NovaTrans()
    {
        GameObject estado1 = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        workspaceCanvas.GetComponent<Workspace>().SetNovaTransFlag(true);
        workspaceCanvas.GetComponent<Workspace>().FecharMenuEstado();
    }

    public void SetFinal()
    {
        GameObject estado = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(2).gameObject.activeSelf)
        {
            estado.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            estado.transform.GetChild(2).gameObject.SetActive(false);
        }
        workspaceCanvas.GetComponent<Workspace>().FecharMenuEstado();

    }
    public void SetInicial()
    {
        GameObject estado = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(3).gameObject.activeSelf)
        {
            estado.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            estado.transform.GetChild(3).gameObject.SetActive(false);
        }
        workspaceCanvas.GetComponent<Workspace>().FecharMenuEstado();
    }
}
