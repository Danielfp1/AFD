using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEstado : MonoBehaviour
{
    public GameObject workspace;
    public GameObject menuEstadoObj;


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
        GameObject estado = workspace.GetComponent<Workspace>().GetEstadoAtual();
        Destroy(estado);
        menuEstadoObj.SetActive(false);
    }
    public void NovaTrans()
    {
        workspace.GetComponent<Workspace>().SetNovaTransFlag(true);
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        workspace.GetComponent<Workspace>().AbrirMenuNovaTrans();
    }
    public void SetFinal()
    {
        GameObject estado = workspace.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(2).gameObject.activeSelf)
        {
            estado.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            estado.transform.GetChild(2).gameObject.SetActive(false);
        }
        workspace.GetComponent<Workspace>().FecharMenuEstado();

    }
    public void SetInicial()
    {
        GameObject estado = workspace.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(3).gameObject.activeSelf)
        {
            estado.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            estado.transform.GetChild(3).gameObject.SetActive(false);
        }
        workspace.GetComponent<Workspace>().FecharMenuEstado();
    }
}
